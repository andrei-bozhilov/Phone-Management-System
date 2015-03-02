namespace PhoneManagementSystem.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using System.Text;

    using PhoneManagementSystem.Data;
    using PhoneManagementSystem.Models;
    using PhoneManagementSystem.WebApi.Models.User;    


    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : BaseApiController
    {
        public UserController(IPhoneSystemData data)
            : base(data)
        {
        }

        public UserController()
            : base(new PhoneSystemData())
        {
            this.userManager = new ApplicationUserManager(
                new UserStore<User>(new ApplicationDbContext()));
        }

        private ApplicationUserManager userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager;
            }
        }

        private IAuthenticationManager Authentication
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }
        //Post api/User/Login
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<HttpResponseMessage> LoginUser(LoginUserBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return await this.BadRequest(this.ModelState).ExecuteAsync(new CancellationToken());
            }

            var request = HttpContext.Current.Request;
            var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "/Token";

            using (var client = new HttpClient())
            {
                var requestParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", model.Username),
                    new KeyValuePair<string, string>("password", model.Password)
                };

                var user = this.Data.Users.All().FirstOrDefault(x=>x.UserName == model.Username);
                //check if user is active

                if (!user.IsActive)
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("User is not active", Encoding.UTF8, "application/json")
                    };
                }

                var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var responseCode = tokenServiceResponse.StatusCode;
                                
                var responseMsg = new HttpResponseMessage(responseCode)
                {
                    Content = new StringContent(responseString, Encoding.UTF8, "application/json")
                };

                return responseMsg;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]       
        [Route("Register")]
        public async Task<HttpResponseMessage> RegisterUser(RegisterUserBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return await this.BadRequest(this.ModelState).ExecuteAsync(new CancellationToken());
            }

            var user = new User 
            {
                UserName = model.Username,
                FullName = model.FullName,
                IsActive = true                
            };

            IdentityResult result = 
                await this.UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                 return await this.GetErrorResult(result).ExecuteAsync(new CancellationToken());
            }

            // Auto login after register (successful user registration should return access_token)
            var loginResult = this.LoginUser(new LoginUserBindingModel()
            {
                Username = model.Username,
                Password = model.Password
            });

            return await loginResult;
        }


    }
}
