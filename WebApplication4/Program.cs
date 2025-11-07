using AutoMapper;
using Bll.Interfaces;
using Bll.Mapping;
using Bll.Services;
using Dal.Database;
using Dal.Interfaces;
using Dal.UoW;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication4.Middleware;

namespace WebApplication4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------- DAL ----------
            builder.Services.AddScoped<DapperContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ---------- AutoMapper ----------
            builder.Services.AddAutoMapper(typeof(AppMappingProfile));

            // ---------- FluentValidation ----------
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<JobSeekerService>();

            // ---------- BLL ----------
            builder.Services.AddScoped<IJobSeekerService, JobSeekerService>();
            builder.Services.AddScoped<ICvService, CvService>();
            builder.Services.AddScoped<IEducationService, EducationService>();
            builder.Services.AddScoped<IExperienceService, ExperienceService>();

            // ---------- Controllers ----------
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    // Відключаємо стандартну валідацію ModelState
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Зберігаємо оригінальні назви властивостей
                });

            // ---------- OpenAPI + Swagger ----------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Job Seekers API",
                    Version = "v1",
                    Description = "API для управління даними про шукачів роботи, їх резюме, освіту та досвід роботи"
                });
                // Включаємо коментарі XML для Swagger
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
                // Додаємо схеми для ProblemDetails
                c.CustomSchemaIds(type => type.FullName);
            });
            
            // ---------- CORS ----------
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // ---------- Logging ----------
            builder.Services.AddLogging();

            var app = builder.Build();

            // ---------- CORS ----------
            app.UseCors();

            // ---------- Swagger UI ----------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Job Seekers API v1");
                    c.RoutePrefix = "swagger";
                    c.DocumentTitle = "Job Seekers API Documentation";
                    c.DisplayRequestDuration();
                    c.EnableTryItOutByDefault();
                });
            }

            // ---------- Error middleware ----------
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
