using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] int? limit, [FromQuery] string sort, [FromQuery] int? offset,
            [FromQuery] int? minid, [FromQuery] int? maxid, [FromQuery] string like, [FromQuery] string columns, [FromQuery] string globalike)
        {
            IQueryable<Student> query = _context.Students.AsQueryable();

            // Проверки и фильтрация
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

            // Генерация HATEOAS-ссылок для студентов
            var result = students.Select(student => new
            {
                student.Id,
                student.Name,
                student.Phone,
                Links = new List<Link>
        {
            new Link { Rel = "self", Href = Url.Link(nameof(GetStudent), new { id = student.Id }) },
            new Link { Rel = "update", Href = Url.Link(nameof(PutStudent), new { id = student.Id }) },
            new Link { Rel = "delete", Href = Url.Link(nameof(DeleteStudent), new { id = student.Id }) }
        }
            });

            // Генерация HATEOAS-ссылок для списка
            var response = new
            {
                Students = result,
                Links = new List<Link>
        {
            new Link { Rel = "self", Href = Url.Link(nameof(GetStudents), new { limit, sort, offset, minid, maxid, like, columns, globalike }) },
            new Link { Rel = "create", Href = Url.Link(nameof(PostStudent), new { }) }
        }
            };

            // Возвращаем JSON или XML в зависимости от заголовков запроса
            if (HttpContext.Request.Headers["Accept"].ToString().Contains("application/xml"))
            {
                return Ok(response); // XML-ответ при установке "application/xml"
            }

            return Ok(response); // JSON по умолчанию
        }



        // GET: api/students/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var links = new List<Link>
            {
                new Link { Rel = "self", Href = Url.Link(nameof(GetStudent), new { id = student.Id }) },
                new Link { Rel = "update", Href = Url.Link(nameof(PutStudent), new { id = student.Id }) },
                new Link { Rel = "delete", Href = Url.Link(nameof(DeleteStudent), new { id = student.Id }) }
            };

            // Include links in the response
            return Ok(new { student, Links = links });
        }

        // POST: api/students
        [HttpPost]
        public async Task<IActionResult> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // PUT: api/students/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/students/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
