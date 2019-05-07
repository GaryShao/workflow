namespace SFood.BackendService.Application.Dtos
{
    public class RestaurantDto
    {
        /// <summary>
        /// 餐厅id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 餐厅设置的订单响应时间
        /// </summary>
        public byte OrderResponseTime { get; set; }
    }
}
