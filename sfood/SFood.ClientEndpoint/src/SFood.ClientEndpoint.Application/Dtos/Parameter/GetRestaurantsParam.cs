using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.ClientEndpoint.Application.Dtos.Parameter
{    
    public class GetRestaurantsParam
    {
        [Required]
        public string CenterId { get; set; }

        public SortingType SortType { get; set; } = SortingType.Default;

        public List<string> CategoryIds { get; set; }

        public string SearchWord { get; set; }

        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 10;

        public enum SortingType : byte
        {
            Turnover,
            Default
        }
    }
}
