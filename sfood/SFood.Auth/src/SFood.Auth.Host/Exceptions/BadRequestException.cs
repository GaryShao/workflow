using System;

namespace SFood.Auth.Host.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string msg) : base(msg)
        {

        }
    }
}
