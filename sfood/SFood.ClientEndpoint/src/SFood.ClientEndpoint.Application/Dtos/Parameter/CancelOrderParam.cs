using System.ComponentModel.DataAnnotations;

namespace SFood.ClientEndpoint.Application.Dtos.Parameter
{
    public class CancelOrderParam
    {
        [Required]
        public string OrderId { get; set; }

        [Required]
        public string Reason { get; set; }
    }
}
