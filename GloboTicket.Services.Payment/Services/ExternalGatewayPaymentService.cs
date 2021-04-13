using GloboTicket.Services.Payment.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace GloboTicket.Services.Payment.Services
{
    public class ExternalGatewayPaymentService: IExternalGatewayPaymentService
    {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;
        private readonly ILogger<ExternalGatewayPaymentService> logger;

        public ExternalGatewayPaymentService(HttpClient client, IConfiguration configuration, ILogger<ExternalGatewayPaymentService> logger)
        {
            this.client = client;
            this.configuration = configuration;
            this.logger = logger;
        }
       
        public async Task<bool> PerformPayment(PaymentInfo paymentInfo)
        {
            var dataAsString = JsonSerializer.Serialize(paymentInfo);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await  client.PostAsync(configuration.GetValue<string>("ApiConfigs:ExternalPaymentGateway:Uri") + "/api/paymentapprover", content);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Unable to make payment for {CardNumber}", HideCardNumber(paymentInfo.CardNumber));
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
            }

            logger.LogDebug("Successfully made payment for {CardNumber}", HideCardNumber(paymentInfo.CardNumber));

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<bool>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private static string HideCardNumber(string cardNumber)
        {
            var cardNumberWithoutSpaces = cardNumber.Trim();
            var lastDigits = cardNumberWithoutSpaces.Substring(cardNumberWithoutSpaces.Length - 4, 4);
            var prefix = new string('*', cardNumberWithoutSpaces.Length - 4);
            return prefix + lastDigits;
        }
    }
}
