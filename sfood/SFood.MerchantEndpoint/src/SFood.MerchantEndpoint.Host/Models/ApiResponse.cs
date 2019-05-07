using Newtonsoft.Json;
using SFood.MerchantEndpoint.Common.Enums;

namespace SFood.MerchantEndpoint.Host.Models
{
    /// <summary>
    /// 响应模型
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 业务处理结果的状态码
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public BusinessStatusCode StatusCode { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 错误的具体信息
        /// </summary>
        public string Message { get; set; }
    }
}
