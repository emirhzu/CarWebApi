using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _services;

        public AuthenticationController(IServiceManager services)
        {
            _services = services;
        }

        [HttpPost]
        [ServiceFilter(typeof (ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody]UserForRegistrationDto userForRegistrationDto)
        {
            var result = await _services
                            .AuthenticationService
                            .RegisterUser(userForRegistrationDto);

            if (!result.Succeeded)
            {
                foreach ( var error in result.Errors )
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }
        [HttpPost("login")]
        [ServiceFilter(typeof (ValidationFilterAttribute))]
        public async Task<IActionResult> Authenicate([FromBody]UserForAuthenticationDto user)
        {
            if(!await _services.AuthenticationService.ValidateUser(user))
                return Unauthorized();
            var tokenDto = await _services.AuthenticationService.CreateToken(true);

            return Ok(tokenDto);
        }
        [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Refresh([FromBody]TokenDto tokenDto)
        {
            var tokenDtoToReturn = await _services.AuthenticationService.RefreshToken(tokenDto);
            return Ok(tokenDtoToReturn);
        }
    }
}
