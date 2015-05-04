namespace PhoneManagementSystem.WebApi.DataModels
{
    using System;
    using System.Linq.Expressions;

    using PhoneManagementSystem.Models;

    public class DepartmentDataModel
    {
        public static Expression<Func<Department, DepartmentDataModel>> FromDepartment
        {
            get
            {
                return d => new DepartmentDataModel()
                {
                    Id = d.Id,
                    Name = d.Name
                };
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}