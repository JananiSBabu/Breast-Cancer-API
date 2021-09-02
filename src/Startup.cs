using BreastCancerAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace BreastCancerAPI
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
            services.AddRazorPages();

            services.AddDbContext<PatientContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("PatientContextDb"));
            });

            // add automapper to look for the profiles in the current assembly
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register the PatientSeeder service for Dependency Injection
            services.AddTransient<PatientSeeder>();

            // Adds scoped Service IPatientRepository with implementation in PatientRepository
            services.AddScoped<IPatientRepository, PatientRepository>();

            // For circular reference within entities: ReferenceLoopHandling.Ignore
            services.AddControllers()
                .AddNewtonsoftJson(cfg
                => cfg.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "BreastCancerAPI",
                    Version = "1"
                });
            });

            // API Versioning
            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true; // Add version to headers       
                //opt.ApiVersionReader = new UrlSegmentApiVersionReader();
                //opt.ApiVersionReader = new QueryStringApiVersionReader("ver"); // specify version in URI
                //opt.ApiVersionReader = new HeaderApiVersionReader("X-version");  // specify version in header
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("ver", "version"),
                    new HeaderApiVersionReader("X-version"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
