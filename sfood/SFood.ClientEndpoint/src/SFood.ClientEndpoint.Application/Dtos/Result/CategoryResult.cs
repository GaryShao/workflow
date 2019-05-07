using System.Collections.Generic;

namespace SFood.ClientEndpoint.Application.Dtos.Result
{
    public class CategoryResult
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public byte MaxSelected { get; set; }

        public bool IsMultiple { get; set; }

        public bool IsSelected { get; set; }

        public List<OptionResult> Options { get; set; }
    }
}
