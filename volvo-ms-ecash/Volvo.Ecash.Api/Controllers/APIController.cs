using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volvo.Ecash.Api.Controllers
{
    [Authorize]
    public abstract class APIController : ControllerBase
    {

        protected string? GetUserLogin()
        {
            return User?.Claims?.SingleOrDefault(p => p.Type == "user")?.Value;
        }
    }
}