using Beosztasmester.Model;
using Beosztasmester.Services;
using Microsoft.EntityFrameworkCore;

namespace Beosztásmester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();


            builder.Services.AddScoped<IEmployeeService, EmployeesService>();
            builder.Services.AddScoped<IMasterDataService, MasterDataService>();
            builder.Services.AddScoped<IRulesService, RulesService>();

            builder.Services.AddScoped<IRequestsService, RequestsService>();
            builder.Services.AddScoped<IUnavailabilitiesService, UnavailabilitiesService>();

            builder.Services.AddScoped<IIntegrationService, IntegrationService>();

            builder.Services.AddScoped<IDemandsService, DemandsService>();
            builder.Services.AddScoped<IHorizonsService, HorizonsService>();
            builder.Services.AddScoped<IRostersService, RostersService>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddHttpClient<IHorizonsService, HorizonsService>(client =>
            {
                // Docker Compose-ban a solver konténer címe: http://solver:8000
                // Ha helyileg futtatod: http://localhost:8000
                var solverUrl = builder.Configuration["SolverService:BaseUrl"] ?? "http://solver:8000";
                client.BaseAddress = new Uri(solverUrl);
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
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
