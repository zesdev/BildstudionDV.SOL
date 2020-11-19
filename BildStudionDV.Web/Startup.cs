using BildstudionDV.BI.Context;
using BildstudionDV.BI.Database;
using BildstudionDV.BI.MainLogic;
using BildstudionDV.BI.ViewModelLogic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BildStudionDV.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var username = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["username"].Value;
            var password = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["password"].Value;

            var context = new BildStudionDVContext(username, password);

            var deljobbDb = new DelJobb(context);
            var jobbDb = new Jobb(context, deljobbDb);
            var kundDb = new Kund(context, jobbDb);
            //kundjobbslogic
            var deljobbVm = new DelJobbVMLogic(deljobbDb);
            var jobbVM = new JobbVMLogic(jobbDb, deljobbVm);
            var kundVM = new KundVMLogic(kundDb, jobbVM);

            var inventarieDb = new Inventarie(context);
            var gruppDb = new Grupp(context, inventarieDb);
            var enhetDb = new Enhet(context, gruppDb);
            //inventarielogic
            var inventarieVM = new InventarieVMLogic(inventarieDb);
            var gruppVM = new GruppVMLogic(gruppDb, inventarieVM);
            var enhetVM = new EnhetVMLogic(enhetDb, gruppVM);

            //userlogic
            var usersDb = new UserProfiles(context);
            var userProfileVM = new UserProfileVMLogic(usersDb);

            var närvaroDb = new Närvaro(context);
            var deltagareDb = new Deltagare(context, närvaroDb);

            //närvarologic
            var deltagarVM = new DeltagareVMLogic(deltagareDb);
            var närvaroVM = new NärvaroVMLogic(närvaroDb, deltagareDb);

            DeltagarViewLogic deltagarViewLogic = new DeltagarViewLogic(deltagarVM, närvaroVM);
            MatlistaLogic matListaLogic = new MatlistaLogic(context, närvaroVM, deltagarVM);

            services.Add(new ServiceDescriptor(typeof(IDelJobbVMLogic), deljobbVm));
            services.Add(new ServiceDescriptor(typeof(IJobbVMLogic), jobbVM));
            services.Add(new ServiceDescriptor(typeof(IKundVMLogic), kundVM));
            services.Add(new ServiceDescriptor(typeof(IInventarieVMLogic), inventarieVM));
            services.Add(new ServiceDescriptor(typeof(IGruppVMLogic), gruppVM));
            services.Add(new ServiceDescriptor(typeof(IEnhetVMLogic), enhetVM));
            services.Add(new ServiceDescriptor(typeof(IUserProfileVMLogic), userProfileVM));
            services.Add(new ServiceDescriptor(typeof(IDeltagareVMLogic), deltagarVM));
            services.Add(new ServiceDescriptor(typeof(INärvaroVMLogic), närvaroVM));
            services.Add(new ServiceDescriptor(typeof(IDeltagarViewLogic), deltagarViewLogic));
            services.Add(new ServiceDescriptor(typeof(IMatlistaLogic), matListaLogic));

            services.AddAuthentication("CookieAuthentication")
         .AddCookie("CookieAuthentication", config =>
         {
             config.Cookie.Name = "UserLoginCookie";
             config.LoginPath = "/Login/UserLogin";
         });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.  
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // who are you?  
            app.UseAuthentication();

            // are you allowed?  
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
