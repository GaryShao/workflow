namespace SFood.MerchantEndpoint.Common.Options
{
    public class QiNiuOption
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Bucket { get; set; }
        public int TokenExpiredIn { get; set; }
        public string Domain { get; set; }
    }
}
