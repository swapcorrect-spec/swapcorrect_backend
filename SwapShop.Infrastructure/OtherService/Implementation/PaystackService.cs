using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using SwapShop.Domain.Dtos.Response.Payment;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Infrastructure.OtherService.Implementation
{


    public class PaystackService : IPaystackService
    {
        private readonly IConfiguration _configuration;

        public PaystackService(IConfiguration configuration)
        {

            _configuration = configuration;
        }

        public async Task<PaystackInitializeResponse> InitializeTransactionAsync(int amount, string email, string reference, string callbackurl)
        {
            var client = new RestClient(_configuration["Paystack:BaseUrl"]);
            var restRequest = new RestRequest(_configuration["Paystack:InitiateSubUrl"], Method.POST);

            restRequest.AddHeader("Authorization", $"Bearer {_configuration["Paystack:SecretKey"]}");
            restRequest.AddHeader("Content-Type", "application/json");

            restRequest.AddJsonBody(new
            {
                email = email,
                amount = amount * 100,
                reference = reference,
                callback_url = callbackurl
            });

            var response = await client.ExecuteAsync(restRequest);

            if (!response.IsSuccessful)
                throw new Exception("Paystack initialization failed");

            var result = JsonConvert.DeserializeObject<PaystackInitializeResponse>(response.Content);
            
            return result;
        }

        public async Task<bool> VerifyTransactionAsync(string reference)
        {
            var client = new RestClient(_configuration["Paystack:BaseUrl"]);
            var request = new RestRequest($"{_configuration["Paystack:VerifySubUrl"]}/{reference}",
                Method.GET);

            request.AddHeader("Authorization", $"Bearer {_configuration["Paystack:SecretKey"]}");

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                return false;

            dynamic result = JsonConvert.DeserializeObject(response.Content);
            return result.data.status == "success";
        }
        public async Task<PaystackBankResponse> GetAllBank()
        {
            var client = new RestClient(_configuration["Paystack:BaseUrl"]);
            var request = new RestRequest($"bank ",
                Method.GET);

            request.AddHeader("Authorization", $"Bearer {_configuration["Paystack:SecretKey"]}");

            var response = await client.ExecuteAsync(request);

            

            var result = JsonConvert.DeserializeObject<PaystackBankResponse>(response.Content);
            return result;
        }
    }

}
