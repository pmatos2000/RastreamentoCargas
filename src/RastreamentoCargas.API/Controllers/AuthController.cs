using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RastreamentoCargas.Application.DTOs.Auth;
using RastreamentoCargas.Application.Interfaces; 
using RastreamentoCargas.Domain.Entities;


namespace RastreamentoCargas.API.Controllers
{
    [ApiController]
    [Route("api/autenticacao")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationService _authService;

        public AuthController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IAuthenticationService authService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {

            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                loginDto.Password,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            var tokenString =  await _authService.GenerateJwtToken(user);
            return Ok(new LoginResponseDto
            {
                Token = tokenString,
                Expiration = DateTime.UtcNow.AddHours(8) 
            });
        }
    }
}