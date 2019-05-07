using System;

namespace SFood.ClientEndpoint.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string msg): base(msg)
        {

        }
    }
}
