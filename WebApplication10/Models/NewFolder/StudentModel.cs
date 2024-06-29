using System.ComponentModel.DataAnnotations;

namespace WebApplication10.Models.NewFolder
{
    public class StudentModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string FatherName { get; set; }


        [Required(ErrorMessage = "The Grade field is required.")]
        [Range(1, 12, ErrorMessage = "Grade must be between 1 and 12.")]
        public int Grade { get; set; }

        // Add more properties as needed, such as:
        // public DateTime DateOfBirth { get; set; }
        // public string Address { get; set; }
        // public string Email { get; set; }
        // etc.

    }
}
