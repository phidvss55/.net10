using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Contracts;
using webapi.Dtos.Auth;
using webapi.Models;
using RegisterRequest = webapi.Dtos.Auth.RegisterRequest;

namespace webapi.Controllers;

[Route("/auth")]
[ApiController]
public class AuthController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;
    
    public AuthController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginRequest.Username.ToLower());
        if (user == null) return Unauthorized("Invalid username");
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
        
        if (!result.Succeeded) return Unauthorized("Invalid password");

        return Ok(new CreateNewUserRequest
        {
            Username =  user.UserName,
            Email = user.Email,
            Token = _tokenService.CreateToken(user)
        });
    }
    
    [HttpPost("regsiter")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var appUser = new AppUser
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email
            };
            
            var result = await _userManager.CreateAsync(appUser, registerRequest.Password);
            if (result.Succeeded) {
                var roleresult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleresult.Succeeded) {
                    return Ok(new CreateNewUserRequest
                    {
                        Username =  appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    });
                } else {
                    return StatusCode(StatusCodes.Status500InternalServerError, roleresult.Errors);
                }
            } else {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
}