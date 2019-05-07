using Newtonsoft.Json;
using SFood.Auth.Common.Enums;

namespace SFood.Auth.Host.Models
{
    public class ApiResponse
    {
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
