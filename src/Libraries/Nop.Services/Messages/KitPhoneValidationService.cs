using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Messages;
using Nop.Core.Http;
using Nop.Services.Localization;

namespace Nop.Services.Messages
{
    public class KitPhoneValidationService : IPhoneValidationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PhoneValidationSettings _phoneValidationSettings;
        private readonly ILocalizationService _localizationService;

        public KitPhoneValidationService(IHttpClientFactory httpClientFactory,
            PhoneValidationSettings phoneValidationSettings, ILocalizationService localizationService)
        {
            _httpClientFactory = httpClientFactory;
            _phoneValidationSettings = phoneValidationSettings;
            _localizationService = localizationService;
        }

        public async Task<PhoneValidationRequestResult> SendValidationRequestAsync(string phone)
        {
            var client = GetHttpClient();

            var content = GenerateJsonContent(JsonConvert.SerializeObject(new
            {
                phone,
                type = _phoneValidationSettings.DefaultValidationType,
                from = _phoneValidationSettings.DefaultFromAccount,
                msg = _phoneValidationSettings.DefaultMessageBody
            }));

            var httpResponseMessage = await client.PostAsync("request", content);

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new NopException(string.Format(
                    await _localizationService.GetResourceAsync("Account.PhoneValidation.Error")));
            }

            var result = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PhoneValidationRequestResult>(result);
        }

        public async Task<PhoneCodeVerificationRequestResult> SendCodeVerificationRequestAsync(string requestId,
            string code)
        {
            var client = GetHttpClient();

            var content = GenerateJsonContent(JsonConvert.SerializeObject(new {id = requestId, pin = code}));

            var httpResponseMessage = await client.PostAsync("verify", content);

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new NopException(
                    string.Format(await _localizationService.GetResourceAsync("Account.PhoneValidation.Error")));
            }

            var result = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PhoneCodeVerificationRequestResult>(result);
        }

        private HttpClient GetHttpClient()
        {
            var client = _httpClientFactory.CreateClient(NopHttpDefaults.DefaultHttpClient);
            SetDefaultHttpClientSettings(client);

            return client;
        }

        private HttpContent GenerateJsonContent(string jsonContent)
        {
            return new StringContent(
                jsonContent,
                Encoding.UTF8,
                "application/json");
        }

        private void SetDefaultHttpClientSettings(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("http://isms.center/v1/validation/");
            httpClient.DefaultRequestHeaders.Add("Token", _phoneValidationSettings.Token);
        }
    }
}