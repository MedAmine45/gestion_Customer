using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectBusinessCustomer.Startup))]


namespace ProjectBusinessCustomer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
