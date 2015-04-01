
namespace PhoneManagementSystem.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using PhoneManagementSystem.Data.Repository;
    using PhoneManagementSystem.Models;

    public class PhoneSystemData : IPhoneSystemData
    {
        private DbContext context;
        private IDictionary<Type, object> repositories;

        public PhoneSystemData()
            : this(new ApplicationDbContext())
        {
        }
        public PhoneSystemData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }
        public IRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public IRepository<IdentityRole> UserRoles
        {
            get
            {
                return this.GetRepository<IdentityRole>();
            }
        }

        public IRepository<Department> Departments
        {
            get
            {
                return this.GetRepository<Department>();
            }
        }

        public IRepository<Phone> Phones
        {
            get
            {
                return this.GetRepository<Phone>();
            }
        }

        public IRepository<PhoneNumberOrder> PhoneNumberOrders
        {
            get
            {
                return this.GetRepository<PhoneNumberOrder>();
            }
        }

        public IRepository<JobTitle> JobTitles
        {
            get
            {
                return this.GetRepository<JobTitle>();
            }
        }

        public IRepository<InvoiceData> InvoiceDatas
        {
            get
            {
                return this.GetRepository<InvoiceData>();
            }
        }

        public IRepository<InvoiceInfo> InvoiceInfos
        {
            get
            {
                return this.GetRepository<InvoiceInfo>();
            }
        }

        public IRepository<Service> Services
        {
            get
            {
                return this.GetRepository<Service>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);

            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository =
                    Activator.CreateInstance(typeof(EFRepository<T>), context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}
