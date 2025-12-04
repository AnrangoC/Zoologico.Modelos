using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zoologico.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ZoologicoAPIContext>(options =>
                //options.UseSqlServer(builder.Configuration.GetConnectionString("ZoologicoAPIContext.sqlserver") ?? throw new InvalidOperationException("Connection string 'ZoologicoAPIContext' not found."))
                options.UseNpgsql(builder.Configuration.GetConnectionString("ZoologicoAPIContext.postgresql") ?? throw new InvalidOperationException("Connection string 'ZoologicoAPIContext' not found."))
                //options.UseOracle(builder.Configuration.GetConnectionString("ZoologicoAPIContext.oracle") ?? throw new InvalidOperationException("Connection string 'ZoologicoAPIContext' not found."))
                //options.UseMySql(builder.Configuration.GetConnectionString("ZoologicoAPIContext.mariadb") ?? throw new InvalidOperationException("Connection string 'ZoologicoAPIContext' not found."),Microsoft.EntityFrameworkCore.ServerVersion.Parse("12.0.2-MariaDB")
                
            );

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure JSON options
            builder.Services
                .AddControllers()
                .AddNewtonsoftJson(
                    options => 
                    options.SerializerSettings.ReferenceLoopHandling
                    = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
