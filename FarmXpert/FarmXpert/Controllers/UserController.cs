using FarmXpert.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    [HttpPost("complete-profile-setup")]
    public async Task<IActionResult> CompleteProfileSetup()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();
        
        user.ProfileSetupCompleted = true;
        var result = await _userManager.UpdateAsync(user);
        
        if (result.Succeeded)
        {
            return Ok(new { message = "Profile setup completed successfully" });
        }
        
        return BadRequest(result.Errors);
    }
}
