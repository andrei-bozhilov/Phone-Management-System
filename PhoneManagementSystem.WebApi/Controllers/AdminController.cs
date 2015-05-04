namespace PhoneManagementSystem.WebApi.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using System.Threading.Tasks;
    using System.Text;
    using System.Net;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using LinqKit;

    using PhoneManagementSystem.Commons;
    using PhoneManagementSystem.Data;
    using PhoneManagementSystem.Models;
    using PhoneManagementSystem.WebApi.BindingModels.Admin;
    using PhoneManagementSystem.WebApi.BindingModels.User;
    using PhoneManagementSystem.WebApi.DataModels;

    [Authorize(Roles = GlobalConstants.AdminRole)]
    [RoutePrefix("api/admin")]
    public class AdminController : BaseApiController
    {
        private ApplicationUserManager userManager;

        public AdminController(IPhoneSystemData data)
            : base(data)
        {
        }

        public AdminController()
            : base(new PhoneSystemData())
        {
            this.userManager = new ApplicationUserManager(new UserStore<User>(new ApplicationDbContext()));
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager;
            }
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
        public IHttpActionResult GetAllUsers()
        {
            var users = this.Data.Users.All()
                .OrderBy(u => u.UserName)
                .Select(UserDataModel.FromUser);

            return this.Ok(users);
        }

        [HttpGet]
        [Route("Users/UserInfo")]
        public IHttpActionResult GetUserWithPhoneInfo([FromUri]AdminGetUserWithPhoneInfoBindingModel model)
        {
            if (model == null)
            {
                return this.BadMessageRequest("Invalid request. You should pass phone number or user's full name");
            }

            string userId = null;

            if (!string.IsNullOrWhiteSpace(model.FullName))
            {
                userId = this.Data.Users.All()
                    .Where(u => u.FullName == model.FullName)
                    .Select(u => u.Id)
                    .FirstOrDefault();

                this.CheckObjectForNull(userId, "user", userId);

                var takePhoneDate = this.Data.PhoneNumberOrders.All()
                    .Where(o => o.PhoneId == model.Phone
                        && (o.PhoneAction == PhoneAction.GiveBackPhone
                        || o.PhoneAction == PhoneAction.GetPhoneForPrivateUse))
                    .OrderByDescending(o => o.ActionDate)
                    .Select(o => o.ActionDate)
                    .FirstOrDefault();

                var giveBackPhoneDate = this.Data.PhoneNumberOrders.All()
                    .Where(o => o.PhoneId == model.Phone && o.PhoneAction == PhoneAction.GiveBackPhone)
                    .OrderByDescending(o => o.ActionDate)
                    .Select(o => o.ActionDate)
                    .FirstOrDefault();

                if (takePhoneDate < giveBackPhoneDate)
                {
                    return this.BadRequest("This phone is turn back and it's not have been taken by no one.");
                }
            }
            else if (!string.IsNullOrWhiteSpace(model.Phone))
            {
                var takePhoneDate = this.Data.PhoneNumberOrders.All()
                     .Where(o => o.PhoneId == model.Phone && o.PhoneAction == PhoneAction.TakePhone)
                     .OrderByDescending(o => o.ActionDate)
                     .Select(o => o.ActionDate)
                     .FirstOrDefault();

                var giveBackPhoneDate = this.Data.PhoneNumberOrders.All()
                    .Where(o => o.PhoneId == model.Phone
                        && (o.PhoneAction == PhoneAction.GiveBackPhone
                        || o.PhoneAction == PhoneAction.GetPhoneForPrivateUse))
                    .OrderByDescending(o => o.ActionDate)
                    .Select(o => o.ActionDate)
                    .FirstOrDefault();

                if (takePhoneDate < giveBackPhoneDate)
                {
                    return this.BadRequest("This phone is turn back and it's not have been taken by no one.");
                }

                userId = this.Data.PhoneNumberOrders.All()
                    .Where(o => o.PhoneId == model.Phone && o.PhoneAction == PhoneAction.TakePhone)
                    .OrderByDescending(o => o.ActionDate)
                    .Select(o => o.UserId)
                    .FirstOrDefault();

                this.CheckObjectForNull(userId, "user", userId);
            }

            var user = this.Data.Users.All()
                .Where(u => u.Id == userId)
                .Select(UserInfoDataModel.FromUser)
                .FirstOrDefault();

            var phones = this.Data.Phones.All()
                .Where(p => p.PhoneNumberOrders.Any(po => po.User.Id == userId))
                .Select(PhoneInfoDataModel.FromPhone);

            var orders = this.Data.PhoneNumberOrders.All()
                .Where(o => o.UserId == userId)
                .Select(OrderDataModel.FromOrder);

            return this.Ok(new
            {
                user = user,
                phones = phones,
                orders = orders
            });
        }


        [HttpGet]
        [Route("Users/Paging")]
        public IHttpActionResult GetAllUsersWithPaging([FromUri]AdminGetUsersBindingModel model)
        {
            if (model == null)
            {
                model = new AdminGetUsersBindingModel();
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var users = this.Data.Users.All();

            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                users = users.Where(u => u.UserName.Contains(model.Search))
                    .Union(users.Where(u => u.FullName.Contains(model.Search)))
                    .Union(users.Where(u => u.JobTitle.Name.Contains(model.Search)))
                    .Union(users.Where(u => u.Department.Name.Contains(model.Search)));
            }

            if (model.DepartmentId.HasValue)
            {
                users = users.Where(x => x.DepartmentId == model.DepartmentId.Value);
            }

            if (model.IsActive.HasValue)
            {
                users = users.Where(x => x.IsActive == model.IsActive.Value);
            }

            if (model.IsAdmin.HasValue)
            {
                var adminRoleId = this.Data.UserRoles.All()
                    .Where(r => r.Name == GlobalConstants.AdminRole)
                    .Select(r => r.Id).FirstOrDefault();

                if (model.IsAdmin.Value)
                {
                    users = users.Where(u => u.Roles.Any(r => r.RoleId == adminRoleId));
                }
                else
                {
                    users = users.Where(u => !u.Roles.Any(r => r.RoleId == adminRoleId));
                }
            }

            return OKWithPagingAndFilter(model, users, UserDataModel.FromUser);
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

            if (!string.IsNullOrWhiteSpace(model.PhoneAction))
            {
                var phoneActions = model.PhoneAction.Split('|');
                PhoneAction status;

                // http://stackoverflow.com/questions/782339/how-to-dynamically-add-or-operator-to-where-clause-in-linq
                var searchPredicate = PredicateBuilder.False<PhoneNumberOrder>();

                foreach (var phoneAction in phoneActions)
                {
                    if (Enum.TryParse<PhoneAction>(phoneAction, out status))
                    {
                        var closureVariable = status;
                        searchPredicate =
                          searchPredicate.Or(o => o.PhoneAction == closureVariable);
                    }
                    else
                    {
                        return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid phone action." });
                    }
                }

                orders = orders.AsExpandable().Where(searchPredicate);
            }

            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                orders = orders.Where(o => o.User.UserName.Contains(model.Search))
                    .Union(orders.Where(o => o.User.FullName.Contains(model.Search)))
                    .Union(orders.Where(o => o.Phone.PhoneId.Contains(model.Search)))
                    .Union(orders.Where(o => o.Admin.UserName.Contains(model.Search)))
                    .Union(orders.Where(o => o.Admin.FullName.Contains(model.Search)));
            }

            if (model.FromDate.HasValue)
            {
                orders = orders.Where(o => o.ActionDate >= model.FromDate.Value);
            }

            if (model.ToDate.HasValue)
            {
                orders = orders.Where(o => o.ActionDate <= model.ToDate.Value);
            }

            return this.OKWithPagingAndFilter(model, orders, OrderDataModel.FromOrder);
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

            if (phone.status != PhoneStatus.Free)
            {
                return this.BadRequest("Phone has different status than free. Phone Status: " + phone.status);
            }

            var adminId = User.Identity.GetUserId();
            var admin = this.Data.Users.All().FirstOrDefault(x => x.Id == adminId);

            if (admin == null)
            {
                return BadRequest("Invalid user token! Please login again!");
            }

            if (string.IsNullOrWhiteSpace(model.UserId))
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
                    return this.BadRequest(model.UserId); // if the request is bad userId will be exception message                   
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

            var phoneToGive = this.Data.Phones.GetById(model.PhoneId).PhoneStatus = PhoneStatus.Taken;
            this.Data.PhoneNumberOrders.Add(order);

            this.Data.SaveChanges();

            return Ok(new
            {
                Message = "Order is successful"
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
                    return this.BadRequest("First have to take the phone than return it back");
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

            return Ok("Order is successfully removed number " + phone.PhoneId + " and user " + user.FullName);
        }
    }
}
