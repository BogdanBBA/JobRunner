using Jobs.JobLogging.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/job-runs")]
    [ApiController]
    public class JobRunsController : ControllerBase
    {
        [Route("last-24-hours")]
        [HttpGet]
        public APIResult<List<RunDTO>> ListRestaurants()
        {
            try
            {
                return new APIResult<List<RunDTO>>(DLs.JobLogging.SelectLast24HLogs());
            }
            catch (Exception e)
            {
                return new APIResult<List<RunDTO>>(null, e.ToString());
            }
        }
    }
}