using Owin;

namespace Cognition.Web
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMigrations(app);
            ConfigureUnity(app);
            ConfigureAuth(app);
        }
    }
}
