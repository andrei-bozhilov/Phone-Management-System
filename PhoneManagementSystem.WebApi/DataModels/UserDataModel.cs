namespace PhoneManagementSystem.WebApi.DataModels
{
    using System;
    using System.Linq.Expressions;

    using PhoneManagementSystem.Models;

    public class UserDataModel
    {
        public static Expression<Func<User, UserDataModel>> FromUser
        {
            get
            {
                return u => new UserDataModel()
                {
                    Id = u.Id,
                    Username = u.UserName,
                    FullName = u.FullName,
                    JobTitle = u.JobTitle.Name,
                    DepartmentName = u.Department.Name,
                    IsActive = u.IsActive
                };
            }
        }
        public string Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string JobTitle { get; set; }

        public string DepartmentName { get; set; }

        public bool IsActive { get; set; }
    }
}