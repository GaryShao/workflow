using System.Collections.Generic;

namespace SFood.ClientEndpoint.Application.Dtos.Result
{
    public class GetCustomizationsResult
    {
        public GetCustomizationsResult()
        {
            Categories = new List<CategoryResult>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public List<CategoryResult> Categories { get; set; }
    }
}
