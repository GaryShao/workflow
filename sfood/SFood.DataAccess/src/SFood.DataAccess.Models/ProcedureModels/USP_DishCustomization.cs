using System.ComponentModel.DataAnnotations;

namespace SFood.DataAccess.Models.ProcedureModels
{
    public class USP_DishCustomization
    {
        public string CustomizationName { get; set; }

        public decimal CustomizationUnitPrice { get; set; }

        public string CustomizationId { get; set; }
        
        public string DishId { get; set; }
    }
}
