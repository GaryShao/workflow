using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFood.ClientEndpoint.Common.Dtos.CommsHub;
using SFood.ClientEndpoint.Common.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Common.Utilities.Implements
{
    public class SmsUtility : ISmsUtility
    {
        private readonly CommsHubOption _commsHubOption;
        private readonly HttpClient _httpClient;

        public SmsUtility(IOptionsSnapshot<CommsHubOption> commsHubOption
            , IHttpClientFactory clientFactory)
        {
            _commsHubOption = commsHubOption.Value;
            _httpClient = clientFactory.CreateClient("CommsHub");
            _httpClient.BaseAddress =new Uri(_commsHubOption.BaseUrl);
        }

        public async Task SendingAsync(SendParam param)
        {
            var token = await FetchTokenAsync();
            var phoneNumber = $"{param.CountryCode.TrimEnd()}{param.Phone}";

            var sendUrl =
                $"{_commsHubOption.SendPath}?messageType=VERIFY&phone={WebUtility.UrlEncode(phoneNumber)}" +
                $"&content={WebUtility.UrlEncode(param.Content)}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(sendUrl);

            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SendResult>(resultJson);
                if (result.HasError)
                {
                    throw new Exception($"{string.Join(",", result.ErrorList)}");
                }
                return;
            }
            throw new Exception($"Bad response from commshub");
        }

        private async Task<string> FetchTokenAsync()
        {
            var paramDic = new Dictionary<string, string>();
            paramDic.Add("grant_type", "pasword");
            paramDic.Add("username", _commsHubOption.UserName);
            paramDic.Add("password", _commsHubOption.Password);

            var formContent = new FormUrlEncodedContent(paramDic);
            var response = await _httpClient.PostAsync(_commsHubOption.TokenPath, formContent);
            var resultJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Bad response from commshub");
            }

            var smsToken = JsonConvert.DeserializeObject<TokenResult>(resultJson);
            if (string.IsNullOrEmpty(smsToken?.AccessToken))
            {
                throw new Exception($"Bad response from commshub");
            }
            return smsToken.AccessToken;
        }
    }
}
