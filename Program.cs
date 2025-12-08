
using Microsoft.EntityFrameworkCore;
using NexusBackend.Data;
using NexusBackend.Services;
using Hangfire;

namespace NexusBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register the Service
            builder.Services.AddScoped<ISeatService, SeatService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ISeatCleanupService, SeatCleanupService>();
            builder.Services.AddScoped<IEventService, EventService>();

            // 2. REGISTER EF CORE (The Connection)
            builder.Services.AddDbContext<NexusDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //3. Add Hangfire Services
            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add the Hangfire Server (The thing that actually runs the background jobs)
            builder.Services.AddHangfireServer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                    //policy.WithOrigins("https://front-online-exam-portal.netlify.app")
                    //.AllowAnyHeader()
                    //.AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAngularApp");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // 2. Add Hangfire Dashboard (Optional but amazing for debugging)
            // Access it at: https://localhost:7000/hangfire
            app.UseHangfireDashboard();

            app.MapControllers();

            app.Run();
        }
    }
}
