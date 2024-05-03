using Microsoft.AspNetCore.Mvc;

namespace Notification.Controllers;

[ApiVersion("1.0")]
public class HomeController : BaseController
{
    [HttpGet]
    public IActionResult IndexAsync()
    {
        return Redirect("~/swagger");
    }
    
}