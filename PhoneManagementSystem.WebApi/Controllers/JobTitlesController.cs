namespace PhoneManagementSystem.WebApi.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;

    using PhoneManagementSystem.Commons;
    using PhoneManagementSystem.Data;
    using PhoneManagementSystem.Models;
    using PhoneManagementSystem.WebApi.BindingModels;
    using PhoneManagementSystem.WebApi.DataModels;
    using PhoneManagementSystem.WebApi.Providers;
    using PhoneManagementSystem.WebApi.BindingModels.Admin;

    [RoutePrefix("api/JobTitles")]
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class JobTitlesController : BaseApiController
    {
        private const string ModelName = "job title";

        public JobTitlesController(PhoneSystemData data)
            : base(data)
        {
        }

        public JobTitlesController()
            : base(new PhoneSystemData())
        {
        }

        /// <summary>
        /// Get job titles order by name
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ResponseType(typeof(JobTitleDataModel))]
        public IHttpActionResult GetAllJobTitles()
        {
            var jobTitles = this.Data.JobTitles.All()
                .OrderBy(j => j.Name)
                .Select(JobTitleDataModel.FromJobTitle);

            return this.Ok(jobTitles);
        }

        /// <summary>
        /// Get Job Title by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ResponseType(typeof(JobTitleDataModel))]
        public IHttpActionResult GetJobTitleById(int id)
        {
            var jobTitle = this.Data.JobTitles.All()
                .Where(x => x.Id == id)
                .Select(JobTitleDataModel.FromJobTitle)
                .FirstOrDefault();

            this.CheckObjectForNull(jobTitle, ModelName, id);

            return this.Ok(jobTitle);
        }

        /// <summary>
        /// Get all job titles that satisfied search criteria by title with paging
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]       
        [Route("Paging")]
        [ResponseType(typeof(JobTitleDataModel))]
        public IHttpActionResult GetJobTitlesWithFilter(AdminGetJobTitlesBindingModel model)
        {
            if (model == null)
            {
                model = new AdminGetJobTitlesBindingModel();
            }

            var jobTitles = this.Data.JobTitles.All();

            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                jobTitles = jobTitles.Where(t => t.Name.Contains(model.Search));
            }

            return this.OKWithPagingAndFilter(model, jobTitles, JobTitleDataModel.FromJobTitle);
        }

        /// <summary>
        /// Create Job title
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(JobTitleDataModel))]
        public IHttpActionResult CreateJobTitle(JobTitleBindingModel model)
        {
            this.CheckModelForNull(model, ModelName);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var jobTitle = new JobTitle()
            {
                Name = model.Name
            };

            try
            {
                this.Data.JobTitles.Add(jobTitle);
                this.Data.SaveChanges();
            }
            catch (Exception ex)
            {
                this.GetExceptionMessage(ex);
            }

            model.Id = jobTitle.Id;
            return this.CreatedAtRoute(GlobalConstants.DefaultApi, new { Id = jobTitle.Id }, model);
        }

        /// <summary>
        /// Modify Job Title
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult EditJobTitle(int id, JobTitleBindingModel model)
        {
            this.CheckModelForNull(model, ModelName);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var jobTitle = this.Data.JobTitles.GetById(id);
            this.CheckObjectForNull(jobTitle, ModelName, id);

            jobTitle.Name = model.Name;

            try
            {
                this.Data.JobTitles.Update(jobTitle);
                this.Data.SaveChanges();
            }
            catch (Exception ex)
            {

                this.GetExceptionMessage(ex);
            }

            return this.Ok(ReturnMessageProvider.SuccessEdit(id, ModelName));
        }

        /// <summary>
        /// Delete Job Title
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteJobTitle(int id)
        {
            var jobTitle = this.Data.JobTitles.GetById(id);
            this.CheckObjectForNull(jobTitle, ModelName, id);

            try
            {
                this.Data.JobTitles.Delete(jobTitle);
                this.Data.SaveChanges();
            }
            catch (Exception ex)
            {
                this.GetExceptionMessage(ex);
            }

            return this.Ok(ReturnMessageProvider.SuccessDelete(id, ModelName));
        }
    }
}
