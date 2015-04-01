namespace PhoneManagementSystem.WebApi.Models.Admin
{
    using System.ComponentModel.DataAnnotations;
    public class AdminAddPhoneOrderBindingModel
    {
        [Required]
        public string PhoneId { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]        
        public string Password { get; set; }

        public string FullName { get; set; }

        public int? JobTitleId { get; set; }

        public int? DepartmentId { get; set; }
    }
}