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

    [RoutePrefix("api/Departments")]
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class DepartmentsController : BaseApiController
    {
        private const string ModelName = "department";

        public DepartmentsController(PhoneSystemData data)
            : base(data)
        {
        }

        public DepartmentsController()
            : base(new PhoneSystemData())
        {
        }

        /// <summary>
        /// Get list of departments sorted by name
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ResponseType(typeof(DepartmentDataModel))]
        public IHttpActionResult GetDepartment()
        {
            var department = this.Data.Departments.All()
                .OrderBy(dep => dep.Name)
                .Select(DepartmentDataModel.FromDepartment);

            return Ok(department);
        }

        /// <summary>
        /// Get department by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ResponseType(typeof(DepartmentDataModel))]
        public IHttpActionResult GetDepartmentById(int id)
        {
            var department = this.Data.Departments.All()
                .Where(d => d.Id == id)
                .Select(DepartmentDataModel.FromDepartment)
                .FirstOrDefault();

            this.CheckObjectForNull(department, ModelName, id);

            return this.Ok(department);
        }

        /// <summary>
        /// Get all departments that satisfied search criteria by name with paging
        /// </summary>
        /// <param name="searchInName"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(DepartmentDataModel))]
        [Route("Paging")]
        public IHttpActionResult GetDepartmentsWithFilter(AdminGetDepartmentsBindingModel model)
        {
            if (model == null)
            {
                model = new AdminGetDepartmentsBindingModel();
            }
            var department = this.Data.Departments.All();

            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                department = department.Where(d => d.Name.Contains(model.Search));
            }

            return this.OKWithPagingAndFilter(model, department, DepartmentDataModel.FromDepartment);
        }

        /// <summary>
        /// Create new Department
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(DepartmentBindingModel))]
        public IHttpActionResult CreateDepartment(DepartmentBindingModel model)
        {
            this.CheckModelForNull(model, ModelName);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var department = new Department()
            {
                Name = model.Name
            };

            try
            {
                this.Data.Departments.Add(department);
                this.Data.SaveChanges();
            }
            catch (Exception ex)
            {
                this.GetExceptionMessage(ex);
            }

            model.Id = department.Id;

            return this.CreatedAtRoute(GlobalConstants.DefaultApi, new { Id = department.Id }, model);
        }

        /// <summary>
        /// Modify department
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult EditDepartment(int id, DepartmentBindingModel model)
        {
            this.CheckModelForNull(model, ModelName);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var department = this.Data.Departments.GetById(id);
            this.CheckObjectForNull(department, ModelName, id);

            department.Name = model.Name;

            try
            {
                this.Data.Departments.Update(department);
                this.Data.SaveChanges();
            }
            catch (Exception ex)
            {

                this.GetExceptionMessage(ex);
            }

            return this.Ok(ReturnMessageProvider.SuccessEdit(id, ModelName));
        }

        /// <summary>
        /// Delete department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteDepartment(int id)
        {
            var department = this.Data.Departments.GetById(id);
            this.CheckObjectForNull(department, ModelName, id);

            try
            {
                this.Data.Departments.Delete(department);
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