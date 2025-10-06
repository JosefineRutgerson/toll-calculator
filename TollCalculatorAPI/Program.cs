using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TollCalculatorAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // your React dev server
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();


//Mina controllers
builder.Services.AddControllers();

// Min service för att hämta mina användare från min mockdata.json fil
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITollCalculatorService, TollCalculatorService>();


var app = builder.Build();

// Use CORS policy
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();


app.UseHttpsRedirection();
app.MapControllers();

app.Run();


