using System;

namespace SFood.BusinessInfo.Common.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string msg): base(msg)
        {

        }
    }
}
