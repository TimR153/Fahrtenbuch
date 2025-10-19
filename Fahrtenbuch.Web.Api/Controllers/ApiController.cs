using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Fahrtenbuch.Web.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet("private")]
        [Authorize(Policy = "KannFahrtenLesen")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public IActionResult Private()
        {
            return Ok("Hello from a private endpoint! You need to be authenticated to see this.");
        }
    }
}
