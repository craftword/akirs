using Akirs.client.Controllers;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Akirs.client.Startup))]
namespace Akirs.client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            app.UseHangfireDashboard("/akirsDash");
            //app.UseHangfireDashboard("/myJobDashboard", new DashboardOptions()
            //{
            //    Authorization = new[] { new HangfireAthorizationFilter() }
            //});
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget!"));
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring!"), Cron.Minutely);
            HomeController obj = new HomeController();
            RecurringJob.AddOrUpdate(() => obj.SendNotification(), Cron.Minutely);
            app.UseHangfireServer();

        }
    }
}
