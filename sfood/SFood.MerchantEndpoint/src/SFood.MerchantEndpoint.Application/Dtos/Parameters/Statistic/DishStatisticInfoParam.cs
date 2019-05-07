using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFood.MerchantEndpoint.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Statistic
{
    public class DishStatisticInfoParam
    {
        [Required]        
        [FromQuery(Name = "startDate")]
        public DateTime BeginTime { get; set; }

        [Required]
        [FromQuery(Name = "endDate")]
        public DateTime EndTime { get; set; }

        public string RestaurantId { get; set; }

        [Required]
        public StatisticDimension OrderBy { get; set; }

        [Required]
        public bool IsAsc { get; set; } = false;
    }
}
