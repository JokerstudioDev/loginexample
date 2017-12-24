using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loginexample.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using loginexample.API.DAC;

namespace loginexample.API
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
            services.AddDbContext<AppDbContext>(options =>
                                                options.UseSqlite(@"Data Source=App_Data/appdb.db"));
            services.AddScoped<IAccountDAC, AccountDAC>();
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                    if (!dbContext.Users.Any())
                    {
                        dbContext.Users.Add(new User { Username = "ploy", Password = "1234" });
                        dbContext.SaveChanges();
                    }
                }
            }

            app.UseMvc();
        }
    }
}
