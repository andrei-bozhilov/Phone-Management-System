namespace PhoneManagementSystem.WebApi.Models.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class AdminGetAllUsersBindingModel
    {
        [Display(Name = "isUserActive")]
        public bool? IsUserActive { get; set; }

        [Display(Name = "departmentId")]
        public int? DepartmentId { get; set; }
    }
}