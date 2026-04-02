using Microsoft.EntityFrameworkCore;
using Practice.Persistence;
using Practice.Services.Services;
using Practice.Web.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbContext"));
});

builder.Services

    .AddScoped<StateService>();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

app.UseCors(option =>
{
    option.AllowAnyHeader();
    option.AllowAnyMethod();
    option.AllowAnyOrigin();
});

RouteGroupBuilder apiGroup = app.MapGroup("api");

apiGroup
    .MapStateEndpoints();

app.MapGet("/", () => $"Running in {app.Environment.EnvironmentName} right now.");

app.Run();
