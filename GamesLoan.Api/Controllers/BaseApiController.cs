using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace GamesLoan.Api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected int GetUserId()
        {
            return int.Parse(this.User.Claims.First(i => i.Type == "UserId").Value);
        }

        protected string GetUserRole()
        {
            return User.Claims.First(i => i.Type == ClaimTypes.Role).Value;
        }
    }
}
