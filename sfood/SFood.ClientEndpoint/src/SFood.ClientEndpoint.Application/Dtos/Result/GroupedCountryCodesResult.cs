using Newtonsoft.Json;
using SFood.ClientEndpoint.Application.Dtos.Internal;
using System.Collections.Generic;

namespace SFood.ClientEndpoint.Application.Dtos.Result
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
