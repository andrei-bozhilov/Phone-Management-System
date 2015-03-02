namespace PhoneManagementSystem.WebApi.Models.Admin
{
    using System.ComponentModel.DataAnnotations;
    using PhoneManagementSystem.Models;
    public class AdminGetPhonesBindinModel
    {
        public PhoneStatus? PhoneStatus { get; set; }
        
    }
}