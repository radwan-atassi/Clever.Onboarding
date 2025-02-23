using Azure.Messaging.ServiceBus;
using Clever.Onboarding.OrderPoster.CrmApiClient;
using Clever.Onboarding.OrderPoster.CRMApiClient;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ServiceBusClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    string connectionString = configuration["ServiceBusConnectionString"];
    return new ServiceBusClient(connectionString);
});

builder.Services.AddScoped<ICrmApiClient, CrmApiClient>();
builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
//builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
