using Microsoft.Extensions.Options;
using Qiniu.Storage;
using Qiniu.Util;
using SFood.ClientEndpoint.Common.Options;
using System;

namespace SFood.ClientEndpoint.Common.Utilities.Implements
{
    public class QiNiuUtility : IQiNiuUtility
    {
        private readonly QiNiuOption _option;

        public QiNiuUtility(IOptions<QiNiuOption> option)
        {
            _option = option.Value;
        }

        public string UploadFile(byte[] data)
        {
            var key = Guid.NewGuid().ToString();

            var mac = new Mac(_option.AccessKey, _option.SecretKey);
            var putPolicy = new PutPolicy()
            {
                Scope = _option.Bucket
            };
            putPolicy.SetExpires(_option.TokenExpiredIn);

            var config = new Config()
            {
                Zone = Zone.ZONE_AS_Singapore,
                UseHttps = true
            };

            var token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            var uploader = new FormUploader(config);
            var result = uploader.UploadData(data, key, token, null);
            return _option.Domain + key;
        }
    }
}
