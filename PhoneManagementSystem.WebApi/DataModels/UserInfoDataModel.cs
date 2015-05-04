namespace PhoneManagementSystem.WebApi.DataModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using PhoneManagementSystem.Models;

    public class UserInfoDataModel
    {
        public static Expression<Func<User, UserInfoDataModel>> FromUser
        {
            get
            {
                return u => new UserInfoDataModel()
                {
                    Id = u.Id,
                    Username = u.UserName,
                    FullName = u.FullName,
                    JobTitle = u.JobTitle.Name,
                    Department = u.Department.Name,
                    IsActive = u.IsActive,
                };
            }
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string JobTitle { get; set; }

        public string Department { get; set; }
        public bool IsActive { get; set; }
    }
}
