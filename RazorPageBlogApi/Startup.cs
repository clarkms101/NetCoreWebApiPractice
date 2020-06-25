using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RazorPageBlogApi.Data;
using RazorPageBlogApi.Filter;
using RazorPageBlogApi.HealthCheck;
using System.Linq;
using System.Reflection;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0); // asp.net core������
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true; // ��Client�ݥi�H�ݨ�ثe���䴩���������M��
                o.AssumeDefaultVersionWhenUnspecified = true; // �b�����w������,���w�]����
                o.DefaultApiVersion = new ApiVersion(1, 0); // �w�]����
            });

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

            // ���USwagger���;�,�i�H�w�q�h�Ӥ���
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "Blog API",
                            Version = "v1"
                        });
                    c.SwaggerDoc(
                        "v2",
                        new OpenApiInfo
                        {
                            Title = "Blog API",
                            Version = "v2"
                        });

                    // �ѨMAPI�������ѽĬ�
                    c.OperationFilter<RemoveVersionParameterFilter>();
                    c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
                    c.EnableAnnotations();
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                    // �ϥΤϮg�ӫ��wXML�ɮ׸��|
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // �ҥ�Swagger����ҫ��M�����n��
            app.UseSwagger();

            // �ҥ�Swagger UI�u��,�ë��w���I
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Blog API V2");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    var json = JsonConvert.SerializeObject(report);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
