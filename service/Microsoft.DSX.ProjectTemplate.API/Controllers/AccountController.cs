using Microsoft.AspNetCore.Mvc;
using Microsoft.DSX.ProjectTemplate.Data.DTOs;
using Microsoft.DSX.ProjectTemplate.Data.Services;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthService _authService;

    public AccountController(UserService userService,AuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserRegistrationDto registrationDto)
    {
        if (ModelState.IsValid)
        {
            _userService.RegisterUser(registrationDto);
            return Ok(new { message = "User registered successfully" });
        }

        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        var token = _authService.Login(loginDto);
        if (!string.IsNullOrEmpty(token))
        {
            return Ok(new { token });
        }

        return Unauthorized();
    }
}