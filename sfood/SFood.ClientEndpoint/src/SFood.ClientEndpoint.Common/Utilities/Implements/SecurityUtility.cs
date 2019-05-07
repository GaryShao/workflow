using Microsoft.Extensions.Configuration;
using SFood.ClientEndpoint.Common.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SFood.ClientEndpoint.Common.Utilities.Implements
{
    public class SecurityUtility : ISecurityUtility
    {
        private static readonly Random random = new Random();
        private readonly string _secret;

        public SecurityUtility(IConfiguration configuration)
        {
            _secret = configuration["SMS:SignPhrase"];
        }

        /// <summary>
        ///  生成签名
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public string GenerateSign(Dictionary<string, string> dictionary)
        {
            var strSign = new StringBuilder();
            var orderedDic = dictionary.OrderBy(d => d.Key, new OrdinalComparer());
            var strBuilder = new StringBuilder();
            foreach (var item in orderedDic)
            {
                strBuilder.Append($"{item.Key}={item.Value}&");
            }
            strBuilder.Append($"key={_secret}");
            using (var md5 = MD5.Create())
            {                
                var hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(strBuilder.ToString()));
                foreach (var b in hashValue)
                {
                    strSign.Append(b.ToString("x2"));
                }
            }
            return strSign.ToString().ToUpper();
        }

        /// <summary>
        /// 生成指定位数的随机数字符串
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public string Generate(int size)
        {
            var result = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var num = random.Next(0, 9);
                result.Append(num.ToString());
            }
            return result.ToString();
        }
    }
}
