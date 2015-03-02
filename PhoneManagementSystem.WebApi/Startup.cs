using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;

[assembly: OwinStartup(typeof(PhoneManagementSystem.WebApi.Startup))]

namespace PhoneManagementSystem.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           

            ConfigureAuth(app);

            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }

        public string TokenEndpointPath { get; set; }
    }
}
