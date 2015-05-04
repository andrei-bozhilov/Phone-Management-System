namespace PhoneManagementSystem.WebApi.BindingModels.Admin
{
    using System;
    using PhoneManagementSystem.Models;

    public class AdminGetOrderBindinModel : Pageable, IPageable
    {
        public string PhoneAction { get; set; }

        public string Search { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}