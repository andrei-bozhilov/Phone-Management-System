namespace PhoneManagementSystem.WebApi.DataModels
{
    using System;
    using System.Linq.Expressions;

    using PhoneManagementSystem.Models;

    public class JobTitleDataModel
    {
        public static Expression<Func<JobTitle, JobTitleDataModel>> FromJobTitle
        {
            get
            {
                return j => new JobTitleDataModel()
                {
                    Id = j.Id,
                    Name = j.Name
                };
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}