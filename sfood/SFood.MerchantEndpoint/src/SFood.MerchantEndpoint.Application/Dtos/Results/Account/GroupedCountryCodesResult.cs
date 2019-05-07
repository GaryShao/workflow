using Newtonsoft.Json;
using SFood.MerchantEndpoint.Application.Dtos.Internal;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Account
{
    public class GroupedCountryCodesResult
    {
        public GroupedCountryCodesResult()
        {
            Countries = new List<CountryCodeDto>();
        }

        [JsonProperty("groupName")]
        public string Initial { get; set; }

        [JsonProperty("groupData")]
        public List<CountryCodeDto> Countries { get; set; }
    }      
}
