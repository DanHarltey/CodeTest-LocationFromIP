using LocationFromIP.CodeTest.Core;
using LocationFromIP.CodeTest.Infrastructure;

namespace LocationFromIP.CodeTest.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = CreateHostBuilder(args);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            app.Run();
        }

        public static WebApplicationBuilder CreateHostBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            CoreServices.Register(builder.Services);
            InfrastructureServices.Register(builder.Services, builder.Configuration);

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            return builder;
        }
    }
}