namespace SFood.DataAccess.Common.Enums
{
    public enum RestaurantStatus : byte
    {
        /// <summary>
        /// 正常经营
        /// </summary>
        Running,

        /// <summary>
        /// 已下线
        /// </summary>
        Offline,

        /// <summary>
        /// 注销
        /// </summary>
        Logout
    }
}
