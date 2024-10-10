using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace service.Controllers
{
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public StudentsController()
        {
            _context = new ApplicationDbContext(); // Инициализация контекста
        }

        // GET: api/students
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetStudents(
            int? limit,
            string sort,
            int? offset,
            int? minid,
            int? maxid,
            string like,
            string columns,
            string globalike,
            string format)
        {
            IQueryable<Student> query = _context.Students.AsQueryable();

            // Фильтрация
            if (minid.HasValue)
                query = query.Where(s => s.Id >= minid.Value);
            if (maxid.HasValue)
                query = query.Where(s => s.Id <= maxid.Value);
            if (!string.IsNullOrEmpty(like))
                query = query.Where(s => s.Name.Contains(like));
            if (!string.IsNullOrEmpty(globalike))
                query = query.Where(s => s.Name.Contains(globalike) || s.Phone.Contains(globalike) || s.Id.ToString().Contains(globalike));

            // Сортировка
            if (!string.IsNullOrEmpty(sort) && sort.Equals("NAME", StringComparison.OrdinalIgnoreCase))
                query = query.OrderBy(s => s.Name);
            else
                query = query.OrderBy(s => s.Id);

            // Смещение и лимит
            if (offset.HasValue)
                query = query.Skip(offset.Value);
            if (limit.HasValue)
                query = query.Take(limit.Value);

            // Получение списка студентов
            var students = await query.ToListAsync();

            // Разбор параметра columns
            var selectedColumns = new HashSet<string>();
            if (!string.IsNullOrEmpty(columns))
            {
                selectedColumns = columns.ToUpper().Split(',').Select(c => c.Trim()).ToHashSet();
            }

            // Генерация HATEOAS-ссылок и динамическое формирование результата
            var result = students.Select(student =>
            {
                var studentData = new Dictionary<string, object>();

                // Условно добавляем поля в зависимости от параметра columns
                if (selectedColumns.Count == 0 || selectedColumns.Contains("ID"))
                    studentData["Id"] = student.Id;
                if (selectedColumns.Count == 0 || selectedColumns.Contains("NAME"))
                    studentData["Name"] = student.Name;
                if (selectedColumns.Count == 0 || selectedColumns.Contains("PHONE"))
                    studentData["Phone"] = student.Phone;

                // Добавляем HATEOAS-ссылки
                studentData["Links"] = new List<Link>
                {
                    new Link { Rel = "self", Href = Url.Link("GetStudent", new { id = student.Id }) },
                    new Link { Rel = "update", Href = Url.Link("PutStudent", new { id = student.Id }) },
                    new Link { Rel = "delete", Href = Url.Link("DeleteStudent", new { id = student.Id }) }
                };

                return studentData;
            });

            // Генерация HATEOAS-ссылок для списка
            var response = new
            {
                Students = result,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = Url.Link("GetStudents", new { limit, sort, offset, minid, maxid, like, columns, globalike }) },
                    new Link { Rel = "create", Href = Url.Link("PostStudent", new { }) }
                }
            };

            // Проверка формата и возврат нужного типа ответа
            if (!string.IsNullOrEmpty(format) && format.Equals("xml", StringComparison.OrdinalIgnoreCase))
            {
                var xmlSerializer = new System.Xml.Serialization.XmlSerializer(response.GetType());
                using (var stringWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(stringWriter, response);
                    return Content(stringWriter.ToString(), "application/xml"); // Возвращаем XML
                }
            }

            // По умолчанию возвращаем JSON
            return Ok(response);
        }

        // GET: api/students/{id}
        [HttpGet]
        [Route("{id:int}", Name = "GetStudent")]
        public async Task<IHttpActionResult> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var links = new List<Link>
            {
                new Link { Rel = "self", Href = Url.Link("GetStudent", new { id = student.Id }) },
                new Link { Rel = "update", Href = Url.Link("PutStudent", new { id = student.Id }) },
                new Link { Rel = "delete", Href = Url.Link("DeleteStudent", new { id = student.Id }) }
            };

            // Include links in the response
            return Ok(new { student, Links = links });
        }

        // POST: api/students
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> PostStudent([FromBody] Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetStudent", new { id = student.Id }, student);
        }

        // PUT: api/students/{id}
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutStudent(int id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/students/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}