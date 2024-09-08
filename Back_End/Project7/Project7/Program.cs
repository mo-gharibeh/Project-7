using Microsoft.EntityFrameworkCore;
using Project7.Models;
using PayPalCheckoutSdk.Core; // Add the necessary namespaces for PayPal
using PayPalCheckoutSdk.Orders; // Add this for PayPal SDK

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("YourConnectionString")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// PayPal Service Injection
builder.Services.AddSingleton<PayPalService>(sp =>
{
    // You can get these values from your appsettings.json or environment variables
    var clientId = builder.Configuration["PayPal:ClientId"];
    var clientSecret = builder.Configuration["PayPal:ClientSecret"];

    // Ensure you toggle this flag for live/sandbox environments
    var isLive = builder.Configuration.GetValue<bool>("PayPal:IsLive");

    return new PayPalService(clientId, clientSecret, isLive);
});

var app = builder.Build();

app.UseCors("Development");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
