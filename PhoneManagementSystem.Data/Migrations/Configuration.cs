namespace PhoneManagementSystem.Data.Migrations
{

    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;

    using PhoneManagementSystem.Models;



    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        private Random random = new Random(0);
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var departments = this.SeedDepartment(context);
            var phones = this.SeedPhones(context);
            var users = this.SeedUsers(context);
            this.SeedPhonesUsersOrders(context, phones, users);
        }

        private void SeedPhonesUsersOrders(ApplicationDbContext context, IList<Phone> phones, ICollection<User> users)
        {
            foreach (var user in users)
            {
                var phone = phones[this.random.Next(0, phones.Count)];
                phones.Remove(phone);

                var order = new PhoneNumberOrder()
                {
                    PhoneId = phone.Id,
                    UserId = user.Id,
                    ActionDate = DateTime.Now,
                    PhoneAction = PhoneAction.TakePhone,
                    AdminId = context.Users.Where(x => x.UserName == "admin").Select(x => x.Id).FirstOrDefault()
                };

                var newPhone = context.Phones.Where(x => x.Id == phone.Id).FirstOrDefault();
                newPhone.PhoneStatus = PhoneStatus.Taken;

                context.PhoneNumberOrders.Add(order);
            }

            context.SaveChanges();
        }

        private IList<Department> SeedDepartment(ApplicationDbContext context)
        {

            var departments = new List<Department>();
            var departmentNames = new List<string>
            {
                "Gastroenterology", "Pulmology", "Dermatology"
            };

            foreach (var departmentName in departmentNames)
            {
                var department = new Department { Name = departmentName };
                context.Departments.Add(department);
                departments.Add(department);
            }

            context.SaveChanges();

            return departments;
        }

        private IList<Phone> SeedPhones(ApplicationDbContext context)
        {
            var phones = new List<Phone>();

            for (int i = 0; i < 20; i++)
            {
                var phone = new Phone
                {
                    Number = RandomPhoneNumber(),
                    PhoneStatus = PhoneStatus.Free,
                    CardType = CardType.Unknown,
                    CreditLimit = 50,
                    HasRouming = true
                };

                if (phones.Any(x => x.Number == phone.Number))
                {
                    i--;
                    continue;
                }

                phones.Add(phone);
                context.Phones.Add(phone);

            }

            context.SaveChanges();

            return phones;
        }

        private ICollection<User> SeedUsers(ApplicationDbContext context)
        {
            var usernames = new string[] { "admin", "maria", "peter", "kiro", "didi" };

            var users = new List<User>();
            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 2,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            foreach (var username in usernames)
            {
                var name = username[0].ToString().ToUpper() + username.Substring(1);
                int i = 0;
                var user = new User
                {
                    UserName = username,
                    FullName = name,
                    Email = username + "@gmail.com",
                    EmployeeNumber = ++i,
                    IsActive = true
                };

                var password = username;
                var userCreateResult = userManager.Create(user, password);
                if (userCreateResult.Succeeded)
                {
                    users.Add(user);
                }
                else
                {
                    throw new Exception(string.Join("; ", userCreateResult.Errors));
                }
            }

            // Create "Administrator" role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole("Administrator"));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            // Add "admin" user to "Administrator" role
            var adminUser = users.First(user => user.UserName == "admin");
            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, "Administrator");
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }

            context.SaveChanges();

            return users;
        }


        private string RandomPhoneNumber()
        {
            var middleNumber = new int[] { 993, 933 };
            return "0884"
                + middleNumber[random.Next(0, middleNumber.Length)]
                + random.Next(0, 1000).ToString().PadLeft(3, '0');
        }
    }
}
