namespace PhoneManagementSystem.WebApi.DataModels
{
    using System;
    using System.Linq.Expressions;

    using PhoneManagementSystem.Models;

    public class OrderDataModel
    {
        public static Expression<Func<PhoneNumberOrder, OrderDataModel>> FromOrder
        {
            get
            {
                return o => new OrderDataModel()
                {
                    OrderId = o.Id,
                    UserName = o.User.FullName,
                    Phone = o.Phone.PhoneId,
                    Admin = o.Admin.UserName,
                    Date = o.ActionDate,
                    Action = o.PhoneAction.ToString()
                };
            }
        }

        public int OrderId { get; set; }

        public string UserName { get; set; }

        public string Phone { get; set; }

        public string Admin { get; set; }

        public DateTime Date { get; set; }

        public string Action { get; set; }
    }
}