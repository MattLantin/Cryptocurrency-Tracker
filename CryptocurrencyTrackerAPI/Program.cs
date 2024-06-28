using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CryptocurrencyTrackerAPI.Services; // Add this
using CryptocurrencyTrackerAPI; // Add this

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptocurrencyTrackerAPI", Version = "v1" });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Your frontend URL
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddSignalR();
builder.Services.AddSingleton<CryptocurrencyService>();
builder.Services.AddSingleton<PricePredictionService>();
builder.Services.AddSingleton<AnomalyDetectionService>();
builder.Services.AddSingleton<SentimentAnalysisService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptocurrencyTrackerAPI v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseCors(); // Enable CORS

app.MapControllers();
app.MapHub<CryptoHub>("/cryptohub");

app.Run();
