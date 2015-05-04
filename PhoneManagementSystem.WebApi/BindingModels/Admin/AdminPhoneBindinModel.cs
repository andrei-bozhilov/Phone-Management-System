namespace PhoneManagementSystem.WebApi.BindingModels.Admin
{
    using PhoneManagementSystem.Models;
    using System.ComponentModel.DataAnnotations;

    public class AdminPhoneBindinModel
    {
        public string Search { get; set; }

        public bool? IsAvailable { get; set; }
    }
}