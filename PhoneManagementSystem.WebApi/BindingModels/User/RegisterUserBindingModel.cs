using System.ComponentModel.DataAnnotations;
namespace PhoneManagementSystem.WebApi.BindingModels.User
{
    public class RegisterUserBindingModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(3)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Display(Name = "JobTitleId")]
        public int? JobTitleId { get; set; }

        [Display(Name = "DepartmentId")]
        public int? DepartmentId { get; set; }
    }
}