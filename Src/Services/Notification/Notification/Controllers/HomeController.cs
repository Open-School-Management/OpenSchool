using Microsoft.AspNetCore.Mvc;

namespace Notification.Controllers;

public class HomeController : BaseController
{
    [HttpGet]
    public IActionResult IndexAsync()
    {
        return Redirect("~/swagger");
    }
    
}