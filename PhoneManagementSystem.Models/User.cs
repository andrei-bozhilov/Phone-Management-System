namespace PhoneManagementSystem.Models
{ 
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;    
   
    public class User : IdentityUser
    {
        private ICollection<PhoneNumberOrder> userPhoneNumberOrders;
        private ICollection<PhoneNumberOrder> adminPhoneNumberOrders;

        public User()
        {
            this.UserPhoneNumberOrders = new HashSet<PhoneNumberOrder>();
            this.AdminPhoneNumberOrders = new HashSet<PhoneNumberOrder>();
            
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public int EmployeeNumber { get; set; }

        public int? JobTitleId { get; set; }

        public virtual JobTitle JobTitle { get; set; }

        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<PhoneNumberOrder> UserPhoneNumberOrders
        { 
            get {return this.userPhoneNumberOrders ;}
            set {this.userPhoneNumberOrders = value ;}
        }

        public virtual ICollection<PhoneNumberOrder> AdminPhoneNumberOrders
        {
            get { return this.adminPhoneNumberOrders; }
            set { this.adminPhoneNumberOrders = value; }
        }
    }
}
