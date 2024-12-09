using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using UsersApi.Dtos;
using UsersApi.Models;
using UsersApi.Services;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UserController : ControllerBase
    {

        private IMapper _iMapper;
        private UserService _userService;

        public UserController(IMapper iMapper, UserService userService)
        {
            _iMapper = iMapper;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] CreateUserDto userDto)
        {
            User user = _iMapper.Map<User>(userDto);

            IdentityResult result = await _userService.Create(user, userDto.Password);
            if (result.Errors.Any())
            {
                return BadRequest(new { Message = result.Errors.Select(e => e.Description) });
            }

            return Ok(new { Message = "Usuário cadastrado" });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Get()
        { 
            return Ok(new { Message = "Usuário autenticado" });
        }

    }
}
