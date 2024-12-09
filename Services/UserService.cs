using ApiUser.Dtos;
using ApiUser.Exceptions;
using Microsoft.AspNetCore.Identity;
using UsersApi.Models;

namespace UsersApi.Services
{
    public class UserService
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
        }

        public async Task<IdentityResult> Create(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task Login(LoginDto loginDto)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, false, false);
            if (!result.Succeeded)
            {
                throw new LoginException("Credenciais inválidas");
            }
        }

    }
}
