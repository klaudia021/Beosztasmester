using Beosztasmester.Models;
using Beosztasmester.Services;
using Microsoft.EntityFrameworkCore;

namespace Beosztasmester;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddProblemDetails();

        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IMasterDataService, MasterDataService>();
        builder.Services.AddScoped<IRulesService, RulesService>();

        builder.Services.AddScoped<IRequestsService, RequestsService>();
        builder.Services.AddScoped<IUnavailabilitiesService, UnavailabilitiesService>();

        builder.Services.AddScoped<IIntegrationService, IntegrationService>();

        builder.Services.AddScoped<IDemandsService, DemandsService>();
        builder.Services.AddScoped<IRostersService, RostersService>();

        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddHttpClient<IHorizonsService, HorizonsService>(client =>
        {
            var solverUrl = builder.Configuration["SolverService:BaseUrl"] ?? "http://solver:8000";
            client.BaseAddress = new Uri(solverUrl);
        });

        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseExceptionHandler();

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
