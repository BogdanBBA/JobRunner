using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController
    {
        // GET api/values
        [HttpGet]
        public APIResult<object> Get()
        {
			DLs.Setup();
            return new APIResult<object>($"Simple test endpoint. You might be looking for .../api/job-runs/last-24-hours. Also, Environment.CurrentDirectory='{Environment.CurrentDirectory}'");
		}
    }
}
