namespace PhoneManagementSystem.WebApi.DataModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using PhoneManagementSystem.Models;

    public class PhoneInfoDataModel
    {
        public static Expression<Func<Phone, PhoneInfoDataModel>> FromPhone
        {
            get
            {
                return p => new PhoneInfoDataModel
                {
                    Number = p.PhoneId,
                    Status = p.PhoneStatus.ToString(),
                    HasRouming = p.HasRouming,
                    CreditLimit = p.CreditLimit.HasValue ? p.CreditLimit.Value : 0,
                    CardType = p.CardType.ToString(),
                    CreatedAt = p.CreatedAt,                   
                };
            }
        }

        public string Number { get; set; }
      
        public string Status { get; set; }

        public bool HasRouming { get; set; }

        public int CreditLimit { get; set; }

        public string CardType { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}