using Azure.Messaging.ServiceBus;
using Clever.Onboarding.OrderProcessor.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

string serviceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString") ?? throw new ArgumentNullException(nameof(serviceBusConnectionString));
string rawQueueName = Environment.GetEnvironmentVariable("RawOrdersQueueName") ?? throw new ArgumentNullException(nameof(serviceBusConnectionString));

builder.Services.AddSingleton<ServiceBusClient>(_ =>
{
    if (string.IsNullOrWhiteSpace(serviceBusConnectionString))
        throw new InvalidOperationException("ServiceBusConnectionString is not configured.");
    return new ServiceBusClient(serviceBusConnectionString);
});

builder.Services.AddSingleton<IOrderPublisher>(sp =>
{
    var client = sp.GetRequiredService<ServiceBusClient>();
    return new OrderPublisher(client, rawQueueName);
});

builder.ConfigureFunctionsWebApplication();

builder.Build().Run();