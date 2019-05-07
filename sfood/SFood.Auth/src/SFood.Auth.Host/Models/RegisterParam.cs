using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.Auth.Host.Models
{
    public class RegisterParam
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"(?=.*[0-9])(?=.*[a-zA-Z]).{6,12}",
            ErrorMessage = "Password must contain number, capital or lower-case letter, symbol; more than 6 and less than 12 letters")]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
            ErrorMessage = "Invalid Email Format.")]
        public string Email { get; set; }

        [Required]
        public string VerificationCode { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [JsonProperty("countryId")]
        public string CountryCodeId { get; set; }

        [Required]
        public bool AgreePolicy { get; set; }
    }
}
