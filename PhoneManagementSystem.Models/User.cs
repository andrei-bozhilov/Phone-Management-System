namespace PhoneManagementSystem.Models
{ 
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;    
   
    public class User : IdentityUser
    {
        private ICollection<PhoneNumberOrder> givePhoneNumberOrders;
        private ICollection<PhoneNumberOrder> takePhoneNumberOrders;

        public User()
        {
            this.GivePhoneNumberOrders = new HashSet<PhoneNumberOrder>();
            this.TakePhoneNumberOrders = new HashSet<PhoneNumberOrder>();
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

        public int? DepartmetId { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<PhoneNumberOrder> GivePhoneNumberOrders
        { 
            get {return this.givePhoneNumberOrders ;}
            set {this.givePhoneNumberOrders = value ;}
        }

        public virtual ICollection<PhoneNumberOrder> TakePhoneNumberOrders
        {
            get { return this.takePhoneNumberOrders; }
            set { this.takePhoneNumberOrders = value; }
        }
    }
}
