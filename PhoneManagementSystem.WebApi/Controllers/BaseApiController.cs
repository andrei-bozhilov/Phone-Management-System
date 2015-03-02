namespace PhoneManagementSystem.WebApi.Controllers
{
    using Microsoft.AspNet.Identity;
    using PhoneManagementSystem.Data;
    using System.Web.Http;
    public class BaseApiController : ApiController
    {
        public BaseApiController()            
        {           
        }

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
    }
}