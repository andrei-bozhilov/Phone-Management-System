namespace PhoneManagementSystem.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System.Net.Http;
    using System.Net;
    using System.Text;
    using System.Web.Http;

    using Microsoft.AspNet.Identity;
    using Newtonsoft.Json;

    using PhoneManagementSystem.Commons;
    using PhoneManagementSystem.Data;
    using PhoneManagementSystem.WebApi.BindingModels;
    using PhoneManagementSystem.WebApi.Properties;
    using System.Data.Entity.Core.Objects;
    using PhoneManagementSystem.WebApi.Providers;
    using System.Reflection;


    public class BaseApiController : ApiController
    {
        public BaseApiController(IPhoneSystemData data)
        {
            this.Data = data;
        }

        protected IPhoneSystemData Data { get; private set; }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }

        /// <summary>
        /// Checks object for null reference
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <param name="objName">Name of the object, goes in exception message</param>
        /// <param name="objId">Id of the object, goes in exception message</param>
        /// <exception cref="HttpResponseException">Thrown when obj is null</exception>
        protected void CheckObjectForNull(object obj, string objName, object objId = null)
        {
            if (obj == null)
            {
                string errorMessage = string.Empty;

                if (objId != null)
                {
                    errorMessage = string.Format("There is no {0} with id {1}", objName, objId);
                }
                else
                {
                    errorMessage = string.Format("There is no such {0}", objName);
                }

                var jsonObj = JsonConvert.SerializeObject(new { Message = errorMessage });

                throw new HttpResponseException(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Content = new StringContent(jsonObj)
                });
            }
        }

        /// <summary>
        /// Check input model for null
        /// <exception cref="HttpResponseException">Returns BadRequest 400</exception>
        /// </summary>
        protected void CheckModelForNull(object model, string modelName)
        {
            if (model == null)
            {
                string errorMessage = string.Format("Invalid model, the {0} is null.", modelName);

                var jsonObj = JsonConvert.SerializeObject(new { Message = errorMessage });

                throw new HttpResponseException(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Content = new StringContent(jsonObj)
                });
            }
        }

        protected IHttpActionResult GetExceptionMessage(Exception ex)
        {
            var exceptionMessage = new StringBuilder();

            while (ex.InnerException != null)
            {
                if (ex.Message.Contains("inner exception"))
                {
                    ex = ex.InnerException;
                    continue;
                }
            }

            exceptionMessage.Append(ex.Message);
            var jsonObj = JsonConvert.SerializeObject(new { Message = exceptionMessage.ToString() });
            return this.BadRequest(jsonObj);
        }

        protected IHttpActionResult OKWithPagingAndFilter<TSource, TResult>(IPageable model, IQueryable<TSource> obj, Expression<Func<TSource, TResult>> selectTo)
        {
            try
            {
                if (model.OrderBy == null)
                {
                    try
                    {
                        obj = obj.OrderByDescending("Id");
                    }
                    catch (Exception)
                    {
                        try
                        {
                            obj = obj.OrderByDescending("PhoneId");
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
                else
                {
                    string[] orderParameter = model.OrderBy.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                    if (string.Equals(orderParameter[1], "asc", StringComparison.CurrentCultureIgnoreCase))
                    {
                        obj = obj.OrderBy(orderParameter[0]);
                    }
                    else if (string.Equals(orderParameter[1], "desc", StringComparison.CurrentCultureIgnoreCase))
                    {
                        obj = obj.OrderByDescending(orderParameter[0]);
                    }
                    else
                    {
                        return this.Content(HttpStatusCode.BadRequest, ReturnMessageProvider.ErrorMessage("Bad order."));
                    }
                }
            }
            catch (Exception)
            {

                return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid sorting." });
            }

            // Apply paging: find the requested page (by given start page and page size)
            int pageSize = Settings.Default.DefaultPageSize;
            if (model.PageSize.HasValue)
            {
                pageSize = model.PageSize.Value;
            }
            var numItems = obj.Count();
            var numPages = (numItems + pageSize - 1) / pageSize;
            if (model.StartPage.HasValue)
            {
                obj = obj.Skip(pageSize * (model.StartPage.Value - 1));
            }

            obj = obj.Take(pageSize);
            var objToReturn = obj.Select(selectTo);

            var type = typeof(TResult);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var order = new List<string>();
            var types = new List<string>();
            var header = new List<string>();

            foreach (var prop in properties)
            {
                order.Add(prop.Name);
                types.Add(prop.PropertyType.Name);
                header.Add(prop.Name.SplitCamelCase());
            }

            return this.Ok(
                new
                {
                    headers = header,
                    order = order,
                    types = types,
                    numItems,
                    numPages,
                    result = objToReturn
                }
            );
        }

        public IHttpActionResult BadMessageRequest(string message)
        {
            return this.Content(HttpStatusCode.BadRequest, new { Message = message });
        }
    }
}