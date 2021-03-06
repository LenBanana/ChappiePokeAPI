using ChappiePokeAPI.DataAccess;
using ChappiePokeAPI.Hubs;
using HelperMethods.Helper;
using HelperMethods.Models;
using HelperVariables.Globals;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ChappiePokeAPI
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
            SmtpSettings smtpSettings = Configuration.GetSection("Smtp").Get<SmtpSettings>();
            Paths.AssetUploadPath = Configuration.GetSection("AssetPath").Value;
            EmailSender._smtpSettings = smtpSettings;
            Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
            Console.WriteLine(System.IO.Directory.GetCurrentDirectory() + Paths.AssetUploadPath);
            if (!System.IO.Directory.Exists(Paths.AssetUploadPath))
            {
                System.IO.Directory.CreateDirectory(Paths.AssetUploadPath);
            }
            services.AddDbContextPool<PokeDBContext>(options =>
            {
                options.UseLazyLoadingProxies().UseMySql(Configuration.GetConnectionString("PokeDB")).ConfigureWarnings(w => w.Ignore(CoreEventId.DetachedLazyLoadingWarning));
            });
            services.AddCors(o => {
                o.AddPolicy("MyPolicy", builder =>
                {
                    builder.WithOrigins(new string[] { "http://localhost:4200", "https://localhost:4200", "https://dreckbu.de", "https://*.dreckbu.de", "https://dreckbu.de/*" })
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
                //o.AddPolicy("AllowAll", builder =>
                //{
                //    builder.AllowAnyOrigin()
                //           .AllowAnyMethod()
                //           .AllowAnyHeader();
                //});
            });
            services.AddSignalR(options => {
                options.EnableDetailedErrors = false;
                options.MaximumReceiveMessageSize = 50000;
            }).AddJsonProtocol(o =>
            {
                o.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
                o.PayloadSerializerOptions.PropertyNamingPolicy = null;
                o.PayloadSerializerOptions.DictionaryKeyPolicy = null;
            });
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PokeDBContext pokeDBContext)
        {
            pokeDBContext.Database.EnsureCreated();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("MyPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MainHub>("/mainsocket");
            });
        }
    }
}
