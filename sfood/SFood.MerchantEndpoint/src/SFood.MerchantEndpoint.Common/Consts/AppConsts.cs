namespace SFood.MerchantEndpoint.Common.Consts
{
    public static class AppConsts
    {
        #region Identity claim keys

        public static readonly string RestaurantIdKey = "resid";

        public static readonly string SubKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public static readonly string RoleKey = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        #endregion

        public const string SFoodDatabaseConnectionString = "SFoodDatabase";

        public const string PasswordRegex = @"(?=.*[0-9])(?=.*[a-zA-Z]).{6,12}";

        public const string EmployeeRole = "restaurant_employee";

        public const string OwnerRole = "restaurant_owner";
    }
}
