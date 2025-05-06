using System.Configuration;
using Auth.Core.Interfaces;
using Auth.Core.Services;
using Auth.Core.Settings;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.ApplicationInsights(builder.Configuration["ApplicationInsights:ConnectionString"], 
        TelemetryConverter.Traces)
    .CreateLogger();

builder.Host.UseSerilog();
//production to use key vault
// builder.Configuration.AddAzureKeyVault(
//     new Uri($"https://{builder.Configuration["KeyVault:Name"]}.vault.azure.net/"),
//     new DefaultAzureCredential(),
//     new DefaultKeyVaultSecretManager());

// Add services to the container.
builder.Services.Configure<OtpSettings>(builder.Configuration.GetSection("OtpSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<SmsSettings>(builder.Configuration.GetSection("SmsSettings"));


// Register services
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISmsService, SmsService>();

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.Run();
