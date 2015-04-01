namespace PhoneManagementSystem.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PhoneManagementSystem.Data;

    [RoutePrefix("api/JobTitles")]
    public class JobTitlesController : BaseApiController
    {
        public JobTitlesController(PhoneSystemData data)
            : base(data)
        {
        }

        public JobTitlesController()
            : base(new PhoneSystemData())
        {
        }

        [HttpGet]
        public IHttpActionResult GetAllJobTitles()
        {
            var jobTitles = this.Data.JobTitles.All().OrderBy(jobT => jobT.Name).Select(jobT => new
            {
                id = jobT.Id,
                jobTitleName = jobT.Name
            }).ToList();

            return this.Ok(jobTitles);
        }

        public IHttpActionResult GetJobTitleById(int id)
        {
            var jobTitle = this.Data.JobTitles.All()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    id = x.Id,
                    jobTitleName = x.Name
                }).FirstOrDefault();

            if (jobTitle == null)
            {
                return BadRequest("There is no Job Title with " + id + ".");
            }

            return this.Ok(jobTitle);
        }
    }
}
