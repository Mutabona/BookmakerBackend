using System.Net;
using BookmakerBackend.Api.Base;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBackend.Api.Controllers;

/// <summary>
/// Команды.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class TeamController : BaseController
{
    
}