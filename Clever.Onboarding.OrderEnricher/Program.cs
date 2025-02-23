using Azure.Messaging.ServiceBus;
using Clever.Onboarding.OrderEnricher.CampaignApiClient;
using Clever.Onboarding.OrderEnricher.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

var serviceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
if (string.IsNullOrWhiteSpace(serviceBusConnectionString))
{
    throw new InvalidOperationException("ServiceBusConnectionString is not configured.");
}

builder.Services.AddHttpClient<ICampaignApiClient, CampaignApiClient>();
builder.Services.AddSingleton(sp =>
{
    return new ServiceBusClient(serviceBusConnectionString);
});

builder.Services.AddScoped<IOrderEnrichmentService, OrderEnrichmentService>();
builder.ConfigureFunctionsWebApplication();

builder.Build().Run();