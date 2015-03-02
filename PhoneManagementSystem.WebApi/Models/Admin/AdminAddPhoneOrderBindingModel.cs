namespace PhoneManagementSystem.WebApi.Models.Admin
{
    using System.ComponentModel.DataAnnotations;
    public class AdminAddPhoneOrderBindingModel
    {
        [Required]
        [Display(Name="phoneId")]
        public int phoneId { get; set; }

        public string userId { get; set; }
        
      
        [Display(Name="Username")]
        public string Username { get; set; }

       
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        [MinLength(3)]
        public string Password { get; set; }      

       
        [Display(Name="FullName")]
        public string FullName { get; set; } 
    }
}