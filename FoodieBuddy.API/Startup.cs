using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodieBuddy.Domain.MailingList;
using FoodieBuddy.Domain.MenuItems;
using FoodieBuddy.Domain.Reservations;
using FoodieBuddy.Infrastructure.Persistence;
using FoodieBuddy.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace FoodieBuddy.API
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
            services.AddEntityFrameworkSqlServer();

            services.AddDbContext<FoodieBuddyDbContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"))
                    );

            services.AddMvc();

            services.AddCors(config => {
                config.AddPolicy("FoodieBuddyApp", policy =>
                {
                    policy.AllowAnyMethod();
                    policy.AllowAnyMethod();
                    policy.WithOrigins("http://localhost:4200", "http://localhost:4300");

                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info { Title = "FoodieBuddy API" });
            });

            services.AddScoped<IFoodieBuddyDbContext, FoodieBuddyDbContext>();

            services.AddTransient<IMailingService, MailingService>();
            services.AddScoped<IMailingRepository, MailingRepository>();

            services.AddTransient<IMenuItemService, MenuItemService>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();

            services.AddTransient<IReservationService, ReservationService>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("FoodieBuddyApp");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodieBuddy Api v1");
            });

            app.UseMvc();
        }
    }
}
