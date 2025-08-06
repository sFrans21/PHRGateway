//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI
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
            services.AddControllers();

            services.AddSwaggerGen(
                c =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });

            services.AddDbContext<APIDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add IHttpClientFactory
            services.AddHttpClient();

            // Register other services
            services.AddMemoryCache();
            services.AddControllers();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("FlutterCorsPolicy",
            //        builder =>
            //        {
            //            builder.WithOrigins("http://localhost:3000") // Replace with your Flutter app's origin
            //                .AllowAnyHeader()
            //                .AllowAnyMethod();
            //        });
            //});

            //Add authentication for Web API
            //services.AddAuthentication(IISServerDefaults.AuthenticationScheme);

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = DefaultAuthenticationTypes.ApplicationCookie;
            //})
            //    .AddCookie(DefaultAuthenticationTypes.ApplicationCookie, options =>
            //{

            //    options.LoginPath = "/Login";
            //    options.LogoutPath = "/Logout";
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1");
                c.RoutePrefix = string.Empty; // Set the Swagger UI at the app's root
            });
            //app.UseCors("FlutterCorsPolicy");
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIWHD v1");
            //    c.RoutePrefix = "swagger"; // Set the Swagger UI at the app's root
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
