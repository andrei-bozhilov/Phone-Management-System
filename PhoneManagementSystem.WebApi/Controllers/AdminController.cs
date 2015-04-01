namespace PhoneManagementSystem.WebApi.Controllers
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Text;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using PhoneManagementSystem.Data;
    using PhoneManagementSystem.Models;
    using PhoneManagementSystem.WebApi.Models.Admin;
    using PhoneManagementSystem.WebApi.Models.User;


    [Authorize(Roles = "Administrator")]
    [RoutePrefix("api/admin")]
    public class AdminController : BaseApiController
    {
        public AdminController(IPhoneSystemData data)
            : base(data)
        {
        }

        public AdminController()
            : base(new PhoneSystemData())
        {
            this.userManager = new ApplicationUserManager(new UserStore<User>(new ApplicationDbContext()));
        }

        private ApplicationUserManager userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager;
            }
        }

        [HttpGet]
        [Route("Phones")]
        public IHttpActionResult GetPhones([FromUri] AdminGetPhonesBindinModel model)
        {
            if (model == null)
            {
                model = new AdminGetPhonesBindinModel();
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var phones = this.Data.Phones.All();

            if (model.PhoneStatus.HasValue)
            {
                phones = phones.Where(x => x.PhoneStatus == model.PhoneStatus);
            }

            var phoneToReturn = phones.Select(x => new
            {
                number = x.PhoneId,
                phoneStatus = x.PhoneStatus.ToString(),
                hasRouming = x.HasRouming,
                creditLimit = x.CreditLimit,
                cardType = x.CardType.ToString(),
                userInfo = (x.PhoneStatus == PhoneStatus.Free) ? new
                {
                    username = "",
                    fullname = "",
                    jobTitle = "",
                    department = ""


                } : x.PhoneNumberOrders.OrderByDescending(order => order.ActionDate).Select(user =>
                    new
                    {
                        username = user.User.UserName,
                        fullname = user.User.FullName,
                        jobTitle = user.User.JobTitle.Name,
                        department = user.User.Department.Name,
                    }
                ).FirstOrDefault()

            }).OrderBy(x => x.userInfo.username).ToList();

            return this.Ok(phoneToReturn);
        }

        [NonAction]
        public async Task<string> RegisterUser(RegisterUserBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                var sb = new StringBuilder();

                var errors = ModelState.Select(x => x.Value.Errors);

                foreach (var err in errors)
                {
                    sb.AppendLine(err.ToString());
                }

                return sb.ToString();
            }

            var user = new User
            {
                UserName = model.Username,
                FullName = model.FullName,
                JobTitleId = model.JobTitleId,
                DepartmentId = model.DepartmentId,
                IsActive = true
            };

            IdentityResult result =
                await this.UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var sb = new StringBuilder();

                var errors = result.Errors;

                foreach (var err in errors)
                {
                    sb.AppendLine(err);
                }

                return sb.ToString();
            }

            return user.Id;
        }

        [HttpGet]
        [Route("Users")]
        public IHttpActionResult GetAllUsers([FromUri]AdminGetAllUsersBindingModel model)
        {
            if (model == null)
            {
                model = new AdminGetAllUsersBindingModel();
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var users = this.Data.Users.All();
            if (model.DepartmentId.HasValue)
            {
                users = users.Where(x => x.DepartmentId == model.DepartmentId);
            }

            if (model.IsUserActive.HasValue)
            {
                users = users.Where(x => x.IsActive == model.IsUserActive);
            }

            var userToReturn = users.Select(x => new
            {
                id = x.Id,
                userName = x.UserName,
                fullName = x.FullName,
                jobTitle = x.JobTitle.Name,
                departmentName = x.Department.Name,
                isActive = x.IsActive
            });

            return Ok(userToReturn);
        }

        [HttpGet]
        [Route("Orders")]
        public IHttpActionResult GetAllOrders([FromUri]AdminGetOrderBindinModel model)
        {
            if (model == null)
            {
                model = new AdminGetOrderBindinModel();
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var orders = this.Data.PhoneNumberOrders.All();

            if (model.PhoneAction.HasValue)
            {
                orders = orders.Where(x => x.PhoneAction == model.PhoneAction);
            }

            if (model.AdminId != null)
            {
                orders = orders.Where(x => x.AdminId == model.AdminId);
            }

            if (model.UserId != null)
            {
                orders = orders.Where(x => x.UserId == model.UserId);
            }

            var orderToReturn = orders.OrderByDescending(o => o.ActionDate).Select(o => new
            {
                orderId = o.Id,
                userName = o.User.FullName,
                phone = o.Phone.PhoneId,
                admin = o.Admin.UserName,
                date = o.ActionDate,
                action = o.PhoneAction.ToString()
            }).ToList();

            return Ok(orderToReturn);
        }

        [HttpPost]
        [Route("Orders/GivePhone")]
        public IHttpActionResult AddPhoneOrder([FromBody]AdminAddPhoneOrderBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var phone = this.Data.Phones.All()
                .Where(x => x.PhoneId == model.PhoneId)
                .Select(x => new
                {
                    phoneNumber = x.PhoneId,
                    status = x.PhoneStatus
                })
                .FirstOrDefault();

            if (phone == null)
            {
                return this.BadRequest("There is no number with id " + model.PhoneId);
            }
            else if (phone.status != PhoneStatus.Free)
            {
                return this.BadRequest("Phone has different status than free. Phone Status: " + phone.status);
            }

            var adminId = User.Identity.GetUserId();
            var admin = this.Data.Users.All().FirstOrDefault(x => x.Id == adminId);

            if (admin == null)
            {
                return BadRequest("Invalid user token! Please login again!");
            }

            if (model.UserId == null)
            {
                if (string.IsNullOrWhiteSpace(model.FullName))
                {
                    return BadRequest("Full name is null");
                }

                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    return BadRequest("Password is null");
                }

                if (string.IsNullOrWhiteSpace(model.Username))
                {
                    return BadRequest("Username is null");
                }

                var userModel = new RegisterUserBindingModel()
                {
                    FullName = model.FullName,
                    Username = model.Username,
                    Password = model.Password,
                    DepartmentId = model.DepartmentId,
                    JobTitleId = model.JobTitleId
                };

                var task = Task.Run(async () =>
                   {
                       return await RegisterUser(userModel);
                   });

                var response = task.Result;

                model.UserId = response;

                if (model.UserId.Contains(" "))
                {
                    return this.BadRequest(model.UserId);
                }

                var userIdTest = this.Data.Users.All()
                   .Where(x => x.Id == model.UserId)
                   .Select(x => x.Id)
                   .FirstOrDefault();

                if (userIdTest == null)
                {
                    return this.BadRequest("There is no user with id " + model.UserId);
                }
            }

            var order = new PhoneNumberOrder()
            {
                UserId = model.UserId,
                PhoneId = model.PhoneId,
                ActionDate = DateTime.Now,
                PhoneAction = PhoneAction.TakePhone,
                AdminId = User.Identity.GetUserId()
            };

            var phoneToGive = this.Data.Phones.Find(model.PhoneId).PhoneStatus = PhoneStatus.Taken;
            this.Data.PhoneNumberOrders.Add(order);

            this.Data.SaveChanges();

            return Ok(new
            {
                message = "Order is successful"
            });
        }

        [Route("Orders/ReturnPhone")]
        [HttpPost]
        public IHttpActionResult RemovePhoneOrder([FromBody]AdminRemovePhoneOrderBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = this.Data.Users.All().FirstOrDefault(x => x.Id == model.UserId);
            if (user == null)
            {
                return this.BadRequest("There is no user with this id " + model.UserId);
            }

            var adminId = User.Identity.GetUserId();
            var admin = this.Data.Users.All().FirstOrDefault(x => x.Id == adminId);
            if (admin == null)
            {
                return this.BadRequest("There is no admin with this id " + adminId + " Please login again.");
            }

            var phone = user.AdminPhoneNumberOrders.OrderBy(x => x.ActionDate).LastOrDefault().Phone;

            //check is there is same order
            var orders = this.Data.PhoneNumberOrders.All()
                .Where(x =>
                    x.PhoneId == phone.PhoneId &&
                    x.UserId == user.Id).GroupBy(x => x.PhoneAction).Select(y => new
                    {
                        action = y.Key,
                        number = y.Count()
                    });

            var giveBacks = orders.Where(x => x.action == PhoneAction.GiveBackPhone).Select(x => x.number).FirstOrDefault();
            var takes = orders.Where(x => x.action == PhoneAction.TakePhone).Select(x => x.number).FirstOrDefault();

            if (giveBacks != 0 && takes != 0)
            {
                if (giveBacks == takes)
                {
                    return this.BadRequest("First have to order the phone than return it back");
                }
            }

            user.IsActive = false;
            phone.PhoneStatus = PhoneStatus.Free;

            var newOrder = new PhoneNumberOrder
            {
                ActionDate = DateTime.Now,
                PhoneId = phone.PhoneId,
                UserId = user.Id,
                AdminId = adminId,
                PhoneAction = PhoneAction.GiveBackPhone
            };

            this.Data.PhoneNumberOrders.Add(newOrder);

            this.Data.SaveChanges();

            return Ok("Order is succesfully remove number " + phone.PhoneId + " and user " + user.FullName);
        }
    }
}
