using Microsoft.AspNetCore.Mvc;
using SFood.MerchantEndpoint.Common.Consts;
using System.Linq;

namespace SFood.MerchantEndpoint.Host.Controllers
{
    public class BaseController : ControllerBase
    {
        internal string RestaurantId
        {
            get
            {
                return HttpContext.User.Identity.IsAuthenticated
                    ? HttpContext.User.Claims.FirstOrDefault(x => x.Type == AppConsts.RestaurantIdKey)?.Value 
                    : null;
            }
        }

        internal string UserId
        {
            get
            {
                return HttpContext.User.Identity.IsAuthenticated
                    ? HttpContext.User.Claims.FirstOrDefault(x => x.Type == AppConsts.SubKey)?.Value
                    : null;
            }
        }

        internal string Role
        {
            get
            {
                return HttpContext.User.Identity.IsAuthenticated
                    ? HttpContext.User.Claims.FirstOrDefault(x => x.Type == AppConsts.RoleKey)?.Value
                    : null;
            }
        }

        internal string Lang
        {
            get
            {
                var languageHeader = HttpContext.Request.Headers.FirstOrDefault(h => h.Key == "Language");
                if (languageHeader.Key == null)
                {
                    return "zh-cn";
                }
                return languageHeader.Value;
            }
        }
    }
}
