using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<Dictionary<string, string>> Get()
        {
            return new Dictionary<string, string>() { { "FiveThousand", "5000" }, { "Random", new Random().Next(10000).ToString() } };
        }
    }
}
