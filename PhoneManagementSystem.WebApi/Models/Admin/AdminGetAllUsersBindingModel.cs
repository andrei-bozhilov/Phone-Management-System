namespace PhoneManagementSystem.WebApi.Models.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class AdminGetAllUsersBindingModel
    {       
        public bool? IsActive { get; set; }
        
        public int? DepartmentId { get; set; }

        public bool? IsAdmin { get; set; }
    }
}