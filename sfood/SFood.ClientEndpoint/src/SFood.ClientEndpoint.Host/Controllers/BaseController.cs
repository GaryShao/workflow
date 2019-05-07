using Microsoft.AspNetCore.Mvc;
using SFood.ClientEndpoint.Common.Consts;
using System.Linq;

namespace SFood.ClientEndpoint.Host.Controllers
{
    public class BaseController : ControllerBase
    {
        internal string UserId
        {
            get
            {
                return HttpContext.User.Identity.IsAuthenticated
                    ? HttpContext.User.Claims.FirstOrDefault(x => x.Type == AppConsts.SubKey)?.Value
                    : null;
            }
        }
    }
}
