using System.IO;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Common.Utilities
{
    /// <summary>
    /// 七牛云的公用方法接口
    /// </summary>
    public interface IQiNiuUtility
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="data">文件的字节流</param>
        /// <returns></returns>
        string UploadFile(byte[] data);
    }
}
