namespace SFood.BackendService.Common.Options
{
    public class OrderExpirationOptions
    {
        /// <summary>
        /// 订单任务的执行时间间隔， 单位为分钟
        /// </summary>        
        public byte Interval { get; set; }
    }
}