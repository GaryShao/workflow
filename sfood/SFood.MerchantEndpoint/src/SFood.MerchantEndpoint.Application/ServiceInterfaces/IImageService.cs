using System.IO;

namespace SFood.MerchantEndpoint.Application
{
    public interface IImageService
    {
        string Upload(Stream stream);
    }
}
