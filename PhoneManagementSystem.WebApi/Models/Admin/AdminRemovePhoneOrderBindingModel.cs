namespace PhoneManagementSystem.WebApi.Models.Admin
{
    using PhoneManagementSystem.Models;
    using System.ComponentModel.DataAnnotations;
    public class AdminRemovePhoneOrderBindingModel
    {       
        [Required]
        public string UserId { get; set; }        
    }
}