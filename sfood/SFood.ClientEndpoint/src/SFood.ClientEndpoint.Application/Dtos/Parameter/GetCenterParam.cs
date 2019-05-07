using System.ComponentModel.DataAnnotations;

namespace SFood.ClientEndpoint.Application.Dtos.Parameter
{
    public class GetCenterParam
    {
        [Required(AllowEmptyStrings = false)]
        public string CenterId { get; set; }

        public string SeatId { get; set; }
    }
}
