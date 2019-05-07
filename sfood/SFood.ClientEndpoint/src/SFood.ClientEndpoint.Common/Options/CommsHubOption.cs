using System;
using System.Collections.Generic;
using System.Text;

namespace SFood.ClientEndpoint.Common.Options
{
    public class CommsHubOption
    {
        public string BaseUrl { get; set; }

        public string TokenPath { get; set; }

        public string SendPath { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string SignPhrase { get; set; }

        public byte Internal { get; set; }
    }
}
