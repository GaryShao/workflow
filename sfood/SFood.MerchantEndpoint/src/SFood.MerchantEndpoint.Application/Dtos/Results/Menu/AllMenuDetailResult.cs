using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Menu
{
    public class AllMenuDetailResult
    {
        public AllMenuDetailResult()
        {
            Menus = new List<MenuDetailResult>();
        }

        /// <summary>
        /// 该餐厅是否有菜品
        /// </summary>
        public bool IsThereAnyDishes { get; set; }

        /// <summary>
        /// 菜单详情列表
        /// </summary>
        public List<MenuDetailResult> Menus { get; set; }
    }
}
