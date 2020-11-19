using ChappiePokeAPI.DataAccess;
using ChappiePokeAPI.Hubs;
using HelperMethods.Helper;
using HelperMethods.Models;
using HelperVariables.Globals;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddDbContext<PokeDBContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("PokeDB"));
            });
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                        .SetIsOriginAllowed(hostname => true)
                        .WithOrigins(new string[] { "http://localhost:4200", "https://localhost:4200", "https://dreckbu.de", "https://*.dreckbu.de", "https://dreckbu.de/*" })
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));
            services.AddSignalR(options => {
                options.EnableDetailedErrors = false;
                options.MaximumReceiveMessageSize = 50000;
            });
            services.AddControllers();
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
