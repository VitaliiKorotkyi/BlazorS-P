using Core.Interface;
using Core.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterAsync(model);
                if (result.Succeeded)
                {
                    return Ok(new { Result = "Registration successful" });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest("Invalid registration details");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation("Login attempt for email: {Email}", model.Email);

            if (ModelState.IsValid)
            {
                var token = await _accountService.LoginAsync(model);
                if (token != null)
                {
                    _logger.LogInformation("Login successful for email: {Email}", model.Email);
                    return Ok(new { Token = token });
                }
                _logger.LogWarning("Unauthorized login attempt for email: {Email}", model.Email);
                return Unauthorized("Invalid email or password.");
            }

            _logger.LogWarning("Invalid login model state for email: {Email}", model.Email);
            return BadRequest("Invalid login");
        }
    }
}
