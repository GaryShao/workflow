namespace SFood.DataAccess.Models.ProcedureModels
{
    public class USP_DishStatistic
    {
        public string OrderDishId { get; set; }

        public string DishId { get; set; }

        public string DishName { get; set; }

        public decimal DishUnitPrice { get; set; }

        public byte Amount { get; set; }

        public decimal? CustomizationUnitPrice { get; set; }
    }
}
