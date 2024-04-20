using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[ApiController]
[Route("api/v1/microservices/[controller]")]
public class BaseController : ControllerBase
{
}