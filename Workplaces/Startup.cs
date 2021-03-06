using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using Workplaces.Common.Constants;
using Workplaces.Data;
using Workplaces.Data.Interfaces;
using Workplaces.Data.Repositories;
using Workplaces.Service.Interfaces;
using Workplaces.Service.Services;

namespace Workplaces
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
            services.AddCors();
            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList();
                    return new BadRequestObjectResult(errors);
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(GlobalConstants.SwaggerVersion, new OpenApiInfo
                {
                    Title = GlobalConstants.SwaggerTitle,
                    Version = GlobalConstants.SwaggerVersion
                });

                var filePath = Path.Combine(AppContext.BaseDirectory, GlobalConstants.XMLDocument);
                c.IncludeXmlComments(filePath);
            });

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IWorkplacesRepository, WorkplacesRepository>();
            services.AddScoped<IUserWorkplacesRepository, UserWorkplacesRepository>();

            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IWorkplacesService, WorkplacesService>();
            services.AddTransient<IUserWorkplacesService, UserWorkplacesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(GlobalConstants.SwaggerURL, GlobalConstants.SwaggerTitleWithVersion);
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
