using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Account
{
    public class VerificationCodeValidationParam
    {
        [Required]
        public string Phone { get; set; }

        [FromQuery(Name = "countryId")]
        public string CountryCodeId { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
