using ApiUser.Dtos;
using ApiUser.Exceptions;
using ApiUser.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UsersApi.Services;

namespace ApiUser.Controllers
{

    [ApiController]
    [Route("[Controller]")]
    public class AuthController : ControllerBase
    {
        private UserService _userService;
        private JwtService _jwtService;

        public AuthController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<ActionResult> login([FromBody] LoginDto loginDto)
        {
            try 
            { 
                await _userService.Login(loginDto);
                string token = _jwtService.GetToken(loginDto.Username);
                return Ok( new { Token = token });
            } 
            catch (LoginException ex)
            {
                return BadRequest( new { ex.Message });
            }
        }

    }
}
