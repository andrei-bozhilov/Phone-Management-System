namespace PhoneManagementSystem.WebApi.Models.Admin
{
    using System.ComponentModel.DataAnnotations;

    using PhoneManagementSystem.Models;
    public class AdminGetOrderBindinModel
    {
        public PhoneAction? PhoneAction { get; set; }

        [Display(Name = "adminId")]
        public string AdminId { get; set; }

        [Display(Name = "userId")]
        public string UserId { get; set; }
    }
}