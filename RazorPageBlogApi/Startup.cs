using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RazorPageBlogApi.Data;
using RazorPageBlogApi.HealthCheck;

namespace RazorPageBlogApi
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

            services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: Configuration["ConnectionStrings:DefaultConnection"],
                    healthQuery: "select 1",
                    name: "MSSQL Check",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] { "database", "sqlServer" })
                .AddCheck<MemoryHealthCheck>("Memory Health Check");

            services.AddDbContext<RazorPageBlogDbContext>(
                options =>
                    options.UseSqlServer(
                        Configuration
                            .GetConnectionString("DefaultConnection")));

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHealthChecks("/health");
            });

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    var json = JsonConvert.SerializeObject(report);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                }
            });
        }
    }
}
