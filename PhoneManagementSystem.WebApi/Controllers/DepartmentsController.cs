using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PhoneManagementSystem.Data;
using PhoneManagementSystem.Models;

namespace PhoneManagementSystem.WebApi.Controllers
{
    [RoutePrefix("api/department")]
    public class DepartmentsController : BaseApiController
    {
        
        public DepartmentsController(PhoneSystemData data):base(data)
        {
        }

        public DepartmentsController()
            : base(new PhoneSystemData())
        {
        }

        // GET api/Departments
        /// <returns>List of all towns sorted by Id</returns>
        [HttpGet]
        public IEnumerable<Department> GetDepartment()
        {
            var department = this.Data.Departments.All().OrderBy(dep => dep.Name).ToList();
            return department;
        }

        /// <summary>
        ///     GET api/Departments/deprtmentId
        /// </summary>
        /// <returns>Get town by id</returns>
        public IHttpActionResult GetDepartmentById(int id)
        {
            var dep = this.Data.Departments
                .All()
                .FirstOrDefault(x => x.Id == id);
            if (dep == null)
            {
                return this.BadRequest("Department #" + id + " not found!");
            }

            return this.Ok(dep);
        }
    }
}