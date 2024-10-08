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
        public async Task<IActionResult> GetStudents(
                [FromQuery] int? limit,
                [FromQuery] string? sort,
                [FromQuery] int? offset,
                [FromQuery] int? minid,
                [FromQuery] int? maxid,
                [FromQuery] string? like,
                [FromQuery] string? columns,
                [FromQuery] string? globalike,
                [FromQuery] string? format
            )
        {
            IQueryable<Student> query = _context.Students.AsQueryable();

            // ����������
            if (minid.HasValue)
                query = query.Where(s => s.Id >= minid.Value);
            if (maxid.HasValue)
                query = query.Where(s => s.Id <= maxid.Value);
            if (!string.IsNullOrEmpty(like))
                query = query.Where(s => s.Name.Contains(like));
            if (!string.IsNullOrEmpty(globalike))
                query = query.Where(s => s.Name.Contains(globalike) || s.Phone.Contains(globalike) || s.Id.ToString().Contains(globalike));

            // ����������
            if (!string.IsNullOrEmpty(sort) && sort.Equals("NAME", StringComparison.OrdinalIgnoreCase))
                query = query.OrderBy(s => s.Name);
            else
                query = query.OrderBy(s => s.Id);

            // �������� � �����
            if (offset.HasValue)
                query = query.Skip(offset.Value);
            if (limit.HasValue)
                query = query.Take(limit.Value);

            // ��������� ������ ���������
            var students = await query.ToListAsync();

            // ������ ��������� columns
            var selectedColumns = new HashSet<string>();
            if (!string.IsNullOrEmpty(columns))
            {
                selectedColumns = columns.ToUpper().Split(',').Select(c => c.Trim()).ToHashSet();
            }

            // ��������� HATEOAS-������ � ������������ ������������ ����������
            var result = students.Select(student =>
            {
                var studentData = new Dictionary<string, object>();

                // ������� ��������� ���� � ����������� �� ��������� columns
                if (selectedColumns.Count == 0 || selectedColumns.Contains("ID"))
                    studentData["Id"] = student.Id;
                if (selectedColumns.Count == 0 || selectedColumns.Contains("NAME"))
                    studentData["Name"] = student.Name;
                if (selectedColumns.Count == 0 || selectedColumns.Contains("PHONE"))
                    studentData["Phone"] = student.Phone;

                // ��������� HATEOAS-������
                studentData["Links"] = new List<Link>
        {
            new Link { Rel = "self", Href = Url.Link(nameof(GetStudent), new { id = student.Id }) },
            new Link { Rel = "update", Href = Url.Link(nameof(PutStudent), new { id = student.Id }) },
            new Link { Rel = "delete", Href = Url.Link(nameof(DeleteStudent), new { id = student.Id }) }
        };

                return studentData;
            });

            // ��������� HATEOAS-������ ��� ������
            var response = new
            {
                Students = result,
                Links = new List<Link>
        {
            new Link { Rel = "self", Href = Url.Link(nameof(GetStudents), new { limit, sort, offset, minid, maxid, like, columns, globalike }) },
            new Link { Rel = "create", Href = Url.Link(nameof(PostStudent), new { }) }
        }
            };

            // �������� ������� � ������� ������� ���� ������
            if (!string.IsNullOrEmpty(format) && format.Equals("xml", StringComparison.OrdinalIgnoreCase))
            {
                var xmlSerializer = new System.Xml.Serialization.XmlSerializer(response.GetType());
                using (var stringWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(stringWriter, response);
                    return Content(stringWriter.ToString(), "application/xml"); // ���������� XML
                }
            }

            // �� ��������� ���������� JSON
            return Ok(response);
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
