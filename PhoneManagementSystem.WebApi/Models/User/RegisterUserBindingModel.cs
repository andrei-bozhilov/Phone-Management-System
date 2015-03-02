using System.ComponentModel.DataAnnotations;
namespace PhoneManagementSystem.WebApi.Models.User
{
    public class RegisterUserBindingModel
    {
        [Required]
        [Display(Name="Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        [MinLength(3)]
        public string Password { get; set; }       

        [Required]
        [Display(Name="FullName")]
        public string FullName { get; set; }        
    }
}