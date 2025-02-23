using System.Net;
using Clever.Onboarding.OrderProcessor.Models;
using Clever.Onboarding.OrderProcessor.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Clever.Onboarding.OrderProcessor;
public class ProcessOrderFunction
{
    private readonly ILogger<ProcessOrderFunction> _logger;
    private readonly IOrderPublisher _orderPublisher;
    public ProcessOrderFunction(ILogger<ProcessOrderFunction> logger, IOrderPublisher orderPublisher)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _orderPublisher = orderPublisher ?? throw new ArgumentNullException(nameof(orderPublisher));
    }

    [Function("process-order")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "orders")]
            HttpRequestData request)
    {
        _logger.LogInformation("Started processing order post request.");

        try
        {
            var order = await request.ReadFromJsonAsync<Order>();

            if (order == null)
            {
                var badResponse = request.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Invalid or missing order data.");
                return badResponse;
            }
            //TODO: Validate order before publishing.

            await _orderPublisher.PublishRawOrderAsync(order).ConfigureAwait(false);
            var okResponse = request.CreateResponse(HttpStatusCode.OK);
            await okResponse.WriteAsJsonAsync(new { Message = "Order submitted successfully." });

            return okResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting order");
            var errorResponse = request.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("An error occurred while submitting the order.");
            return errorResponse;
        }
    }
}
