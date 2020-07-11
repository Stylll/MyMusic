using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MyMusic.Core;
using MyMusic.Data;
using MyMusic.Services;
using MyMusic.Core.Services;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;

namespace MyMusic.API
{
    public class Startup
    {

        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // System.Console.WriteLine(Configuration.GetConnectionString("Default"));

            services.AddMvc();
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<MyMusicDBContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("Default"), x => x.MigrationsAssembly("MyMusic.Data")
                    )
                );
            services.AddTransient<IMusicService, MusicService>();
            services.AddTransient<IArtistService, ArtistService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My Music API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id}");
            });

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(option =>
            {
                option.RoutePrefix = "";
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "My Music V1");
            });

            /*
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
            */
        }
    }
}
