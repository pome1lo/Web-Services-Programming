using WebApplication.Models;

namespace WebAPI.Models
{
    public class StudentResponse
    {
        public IEnumerable<StudentData> Students { get; set; }
        public List<Link> Links { get; set; }
    }

    public class StudentData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public List<Link> Links { get; set; }
    }
}
