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
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IAuthenticationService authService,
            ILogger<AuthController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            _logger.LogInformation("Tentativa de login para o usuário: {UserName}", loginDto.UserName);

            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                _logger.LogWarning("Falha no login: Usuário {UserName} não encontrado.", loginDto.UserName);
                return Unauthorized("Usuário ou senha inválidos.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                loginDto.Password,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Falha no login para o usuário {UserName}: Senha inválida.", loginDto.UserName);
                return Unauthorized("Usuário ou senha inválidos.");
            }

            var tokenString =  await _authService.GenerateJwtToken(user);


            _logger.LogInformation("Login bem-sucedido para o usuário {UserName}. Gerando token JWT.", loginDto.UserName);
            return Ok(new LoginResponseDto
            {
                Token = tokenString,
                Expiration = DateTime.UtcNow.AddHours(8) 
            });
        }
    }
}