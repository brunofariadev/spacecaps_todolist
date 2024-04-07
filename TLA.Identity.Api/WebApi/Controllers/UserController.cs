using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TLA.Identity.Api.Services;
using TLA.WebApi.Core.Controllers;
using TLA.WebApi.Core.Enums;

namespace TLA.Identity.Api.WebApi.Controllers
{
    [Route("api/user")]
    [Authorize]
    public class UserController : MainController
    {
        private readonly AuthenticationService _authenticationService;

        public UserController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [Authorize(Roles = nameof(RoleEnum.Admin))]
        public async Task<IActionResult> GetAll()
        {
            var users = _authenticationService.ObterUsuarios();
            return users == null ? NotFound() : CustomResponse(users);
        }
    }
}
