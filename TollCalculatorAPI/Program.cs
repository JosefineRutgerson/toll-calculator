using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Min service för att hämta mina användare från min mockdata.json fil
builder.Services.AddSingleton<IUserRepository, UserRepository>();


app.UseHttpsRedirection();
app.MapControllers();



app.Run();


