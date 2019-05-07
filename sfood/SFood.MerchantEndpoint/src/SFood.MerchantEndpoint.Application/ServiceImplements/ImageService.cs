using SFood.MerchantEndpoint.Common.Extensions;
using SFood.MerchantEndpoint.Common.Utilities;
using System.IO;

namespace SFood.MerchantEndpoint.Application.ServiceImplements
{
    public class ImageService : IImageService
    {
        private readonly IQiNiuUtility _qiNiuUtility;

        public ImageService(IQiNiuUtility qiNiuUtility)
        {
            _qiNiuUtility = qiNiuUtility;
        }

        public string Upload(Stream stream)
        {
           return _qiNiuUtility.UploadFile(stream.ReadFully());
        }
    }
}
