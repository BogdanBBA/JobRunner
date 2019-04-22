using Microsoft.AspNetCore.Mvc;

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
            return new APIResult<object>("Simple test endpoint. You might be looking for .../api/job-runs/last-24-hours.");
        }
    }
}
