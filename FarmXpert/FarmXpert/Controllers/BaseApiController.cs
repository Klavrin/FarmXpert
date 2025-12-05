using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected string GetCurrentUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User not authenticated");
        }
    }
}
