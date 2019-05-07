using System;

namespace SFood.BusinessInfo.Common.Exceptions
{
    public class BadRequestException: Exception
    {
        public BadRequestException(string msg): base(msg)
        {
            
        }
    }
}
