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


            //conexiones directas que lanzaban excepción si fallaban.
            // builder.Services.AddDbContext<ZoologicoAPIContext>(options =>
            //     //options.UseSqlServer(builder.Configuration.GetConnectionString("ZoologicoAPIContext.sqlserver") ?? throw new InvalidOperationException("Connection string 'ZoologicoAPIContext' not found."))
            //     //options.UseNpgsql(builder.Configuration.GetConnectionString("ZoologicoAPIContext.postgresql") ?? throw new InvalidOperationException("Connection string 'ZoologicoAPIContext' not found."))
            //     //options.UseOracle(builder.Configuration.GetConnectionString("ZoologicoAPIContext.oracle") ?? throw new InvalidOperationException("Connection string 'ZoologicoAPIContext' not found."))
            //     //options.UseMySql(builder.Configuration.GetConnectionString("ZoologicoAPIContext.mariadb") ?? throw new InvalidOperationException("Connection string 'ZoologicoAPIContext' not found."),Microsoft.EntityFrameworkCore.ServerVersion.Parse("12.0.2-MariaDB")
            // );

            builder.Services.AddDbContext<ZoologicoAPIContext>(options =>
            {
                // nos intentamos conectar a la variable de entorno con el mismo nombre que le pusimos
                var envConnection = Environment.GetEnvironmentVariable("DefaultConnection");

                // Modificamos el cambio de json que se usa de forma local a la variable de entorno de render
                if (!string.IsNullOrEmpty(envConnection))
                {
                    options.UseNpgsql(envConnection);
                }
                else
                {
                    // Si no existe, usamos la appsettings.json (local)
                    var localConnection = builder.Configuration.GetConnectionString("ZoologicoAPIContext.postgresql")
                        ?? builder.Configuration.GetConnectionString("DefaultConnection"); // Backup por si acaso

                    if (string.IsNullOrEmpty(localConnection))
                    {
                        throw new InvalidOperationException("No se encontró cadena de conexión ni en Render (Env Var) ni en Local (appsettings).");
                    }

                    options.UseNpgsql(localConnection);
                }
            });


            // builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure JSON options (ACTIVADO)
            builder.Services
                .AddControllers()
                .AddNewtonsoftJson(
                    options =>
                    options.SerializerSettings.ReferenceLoopHandling
                    = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            var app = builder.Build();


             // modificamos esto para que swager no se conecte desde la pc
            // if (app.Environment.IsDevelopment())
            // {
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            // }
            // ahora podremos usar swager desde la nube
            app.UseSwagger();
            app.UseSwaggerUI();

             // control para que render cree las tablas si es que está vacía
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ZoologicoAPIContext>();
                    context.Database.EnsureCreated(); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creando la Base de Datos: " + ex.Message);
                }
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}