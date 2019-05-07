using System;

namespace SFood.MerchantEndpoint.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string msg): base(msg)
        {

        }
    }
}
