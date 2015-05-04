namespace PhoneManagementSystem.WebApi.DataModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using PhoneManagementSystem.Models;

    public class PhoneWtihUserInfoDataModel
    {
        public static Expression<Func<Phone, PhoneWtihUserInfoDataModel>> FromPhone
        {
            get
            {
                return p => new PhoneWtihUserInfoDataModel
                {
                    Number = p.PhoneId,
                    FullName = (p.PhoneStatus == PhoneStatus.Free) ? null : p.PhoneNumberOrders
                    .OrderByDescending(o => o.ActionDate).AsQueryable()
                    .Select(o => o.User.FullName).FirstOrDefault(),

                    JobTitle = (p.PhoneStatus == PhoneStatus.Free) ? null : p.PhoneNumberOrders
                    .OrderByDescending(o => o.ActionDate).AsQueryable()
                    .Select(o => o.User.JobTitle.Name).FirstOrDefault(),

                    Department = (p.PhoneStatus == PhoneStatus.Free) ? null : p.PhoneNumberOrders
                    .OrderByDescending(o => o.ActionDate).AsQueryable()
                    .Select(o => o.User.Department.Name).FirstOrDefault(),

                    Status = p.PhoneStatus.ToString(),
                    HasRouming = p.HasRouming,
                    CreditLimit = p.CreditLimit.HasValue ? p.CreditLimit.Value : 0,
                    CardType = p.CardType.ToString(),
                    CreatedAt = p.CreatedAt,
                };
            }
        }

        public string Number { get; set; }

        public string FullName { get; set; }

        public string JobTitle { get; set; }

        public string Department { get; set; }

        public string Status { get; set; }

        public bool HasRouming { get; set; }

        public int CreditLimit { get; set; }

        public string CardType { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}