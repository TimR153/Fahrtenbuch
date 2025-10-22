using Fahrtenbuch.Web.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Fahrtenbuch.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserInfoController : ControllerBase
    {
        [HttpGet("Get")]
        [ProducesResponseType(typeof(UserInfo), (int)HttpStatusCode.OK)]
        public ActionResult<UserInfo> Get()
        {
            var user = HttpContext.User;

            var userInfo = new UserInfo
            {
                Name = user.FindFirst("name")?.Value,
                Email = user.FindFirst("email")?.Value,
                Nickname = user.FindFirst("nickname")?.Value,
                Locale = user.FindFirst("locale")?.Value,
                Picture = user.FindFirst("picture")?.Value,
                Roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList()
            };

            return this.Ok(userInfo);
        }
    }
}
