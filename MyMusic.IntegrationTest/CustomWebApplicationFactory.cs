using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using MyMusic.Data;
using MyMusic.IntegrationTest.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore;
using MyMusic.API;
using Microsoft.Extensions.Hosting;

namespace MyMusic.IntegrationTest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<MyMusicDBContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    //db.Database.EnsureCreated(); // just creates the db
                    db.Database.EnsureDeleted();
                    db.Database.Migrate(); // creates the db and runs the migration file
                    // db.Artists.Add(new Core.Models.Artist { Name = "Famous guy" });
                    // db.SaveChanges();

                    try
                    {
                        // Seed the database with test data.
                        logger.LogError("Logging stuff here =========================================");
                        Utilities.SetupDb(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
                    }
                }
            });

            builder.ConfigureServices(services =>
            {
                //var constring = builder.GetSetting("Default");

                //var descriptor = services.SingleOrDefault(
                //    d => d.ServiceType ==
                //        typeof(DbContextOptions<MyMusicDBContext>));

                //if (descriptor != null)
                //{
                //    services.Remove(descriptor);
                //}

                //services.AddDbContext<MyMusicDBContext>(
                //options => options.UseSqlServer(
                //    "server=STEPHEN-ARIBABA; Database=MyMusics1; user id=sa; password=captain", x => x.MigrationsAssembly("MyMusic.Data")
                //    )
                //);

                // Build the service provider.
                //var sp = services.BuildServiceProvider();

                //// Create a scope to obtain a reference to the database
                //// context (ApplicationDbContext).
                //using (var scope = sp.CreateScope())
                //{
                //    var scopedServices = scope.ServiceProvider;
                //    var db = scopedServices.GetRequiredService<MyMusicDBContext>();
                //    var logger = scopedServices
                //        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                //    // Ensure the database is created.
                //    //db.Database.EnsureCreated(); // just creates the db
                //    db.Database.EnsureDeleted();
                //    db.Database.Migrate(); // creates the db and runs the migration file
                //    // db.Artists.Add(new Core.Models.Artist { Name = "Famous guy" });
                //    // db.SaveChanges();

                //    try
                //    {
                //        // Seed the database with test data.
                //        logger.LogError("Logging stuff here =========================================");
                //        Utilities.SetupDb(db);
                //    }
                //    catch (Exception ex)
                //    {
                //        logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
                //    }
                //}
            });
        }

        //protected override IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<TestStartup>();

        //protected override IHostBuilder CreateHostBuilder()
        //{
        //    var builder = Host.CreateDefaultBuilder()
        //                      .ConfigureWebHostDefaults(x =>
        //                      {
        //                          x.UseStartup<FakeStartup>().UseTestServer();
        //                      });
        //    return builder;
        //}
    }
}
