using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser>   _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(
        UserManager<IdentityUser>   userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager   = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register-customer")]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterDto model)
    {
        var user   = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Customer");
            return Ok("Customer registered successfully.");
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto model)
    {
        var user   = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
            return Ok("Admin registered successfully.");
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded) return Ok("Login successful.");
        return Unauthorized("Invalid login attempt.");
    }

    [HttpPost("logout")]
    
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("Logged out successfully.");
    }

// POST api/auth/forgot-password
[HttpPost("forgot-password")]
public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
{
    var user = await _userManager.FindByEmailAsync(model.Email);
    if (user == null)
        return Ok("If that email exists, a reset link has been sent."); // don't reveal if user exists

    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

    // TODO: Send email with token
    // For now, return token directly (only for testing!)
    return Ok(new { token, email = model.Email });
}

// POST api/auth/reset-password
[HttpPost("reset-password")]
public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
{
    var user = await _userManager.FindByEmailAsync(model.Email);
    if (user == null)
        return BadRequest("Invalid request.");

    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
    if (result.Succeeded)
        return Ok("Password reset successfully.");

    return BadRequest(result.Errors);
}
} 