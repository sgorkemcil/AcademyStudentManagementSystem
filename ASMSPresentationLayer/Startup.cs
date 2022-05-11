using ASMSDataAccessLayer;
using ASMSEntityLayer.IdentityModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ASMSBusinessLayer.EmailService;
using ASMSBusinessLayer.ContractsBLL;
using ASMSDataAccessLayer.ContractsDAL;
using ASMSBusinessLayer.ImplementationsBLL;
using ASMSDataAccessLayer.ImplementationsDAL;
using ASMSEntityLayer.Mapping;

namespace ASMSPresentationLayer
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
            //Aspnet Core'un connectionString baðlantýsý yapabilmesi için
            //yapýlandýrma servislerine dbcontext nesnesini eklemesi gerekir
            services.AddDbContext<MyContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));


            services.AddControllersWithViews();
            services.AddRazorPages(); //razor sayfalarý için
            services.AddMvc();
            services.AddSession(options =>
            options.IdleTimeout = TimeSpan.FromSeconds(20));
            //oturma zamaný 

            //***************************************************//
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_@.";

            }).AddDefaultTokenProviders().AddEntityFrameworkStores<MyContext>();


            //Mapleme ekliendi
            services.AddAutoMapper(typeof(Maps));

            services.AddSingleton<IEmailSender, EmailSender>(); //Program her ayaða kalktýðýnda intences oluþturuyor
            services.AddScoped<IStudentBusinessEngine, StudentBusinessEngine>();
            services.AddScoped<ASMSDataAccessLayer.ContractsDAL.IUnitOfWork,ASMSDataAccessLayer.ImplementationsDAL.UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,RoleManager<AppRole>roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();     // wwwroot klasörünün eriþimi içindir.

            app.UseRouting();         // Controller/Action/Id
            app.UseSession();         // Oturum mekanýzmasýnýn kullanýlmasý için

            app.UseAuthentication();  // Login Logout iþlemlerinin gerektirdiði oturum iþleyiþlerini kullanabilmek için.
            app.UseAuthorization();   // [Authorize] attribute için (yetki)
            

            //Rolleri oluþturulcak static metot çaðrýldý.
            CreateDefaultData.CreateData.Create(roleManager);

            // MVC ile ayný kod bloðu endpoint'in mekanýzmasýnýn nasýl olacaðý belirleniyor.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
