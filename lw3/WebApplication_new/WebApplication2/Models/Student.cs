using System.ComponentModel.DataAnnotations;

namespace lw3
{

    public partial class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Phone { get; set; }
    }
}
