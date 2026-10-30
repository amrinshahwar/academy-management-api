using AcademyManagementApi.Data;
using AcademyManagementApi.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace AcademyManagementApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.ReferenceHandler =
                    ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication("MyApi")
                .AddCookie("MyApi", option =>
                {
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                    option.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };

                    option.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = 403;
                        return Task.CompletedTask;
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var cs = builder.Configuration
                .GetConnectionString("AcademyDbContext");

            builder.Services.AddDbContext<AcademyDbContext>(
                opt => opt.UseSqlServer(cs));

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}