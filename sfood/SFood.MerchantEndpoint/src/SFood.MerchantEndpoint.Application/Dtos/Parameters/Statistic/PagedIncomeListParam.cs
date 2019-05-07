using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Statistic
{
    public class PagedIncomeListParam
    {
        [Required]
        [FromQuery(Name = "startDate")]
        public DateTime BeginTime { get; set; }

        [Required]
        [FromQuery(Name = "endDate")]
        public DateTime EndTime { get; set; }

        public string RestaurantId { get; set; }

        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 10;
    }
}
