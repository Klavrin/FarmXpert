using FarmXpert.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController: ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender<ApplicationUser> _emailSender; 
    
    public AccountController(UserManager<ApplicationUser> userManager, IEmailSender<ApplicationUser> emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            // return NotFound("User not found or email not confirmed.");
            return NotFound("User not found.");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = $"{request.ClientResetUrl}?email={user.Email}&token={Uri.EscapeDataString(token)}";

        await _emailSender.SendPasswordResetLinkAsync(user, user.Email, resetLink);

        return Ok();
    }
    
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return BadRequest("Invalid user");

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok();
    }
}

public record ForgotPasswordRequest(string Email, string ClientResetUrl);
public record ResetPasswordRequest(string Email, string Token, string NewPassword);