using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyMusic.API;
using MyMusic.Data;

namespace MyMusic.IntegrationTest
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
            : base(configuration)
        {
            var env = hostingEnvironment.EnvironmentName;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyMusicDBContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("Test"), x => x.MigrationsAssembly("MyMusic.Data")
                    )
                );

            base.ConfigureServices(services);
        }
    }
}
