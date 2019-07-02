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
		public APIResult<List<RunDTO>> Last24h()
		{
			DLs.InitializeConstsAsWebAPI();
			try
			{
				return new APIResult<List<RunDTO>>(DLs.JobLogging.SelectLast24hLogs());
			}
			catch (UnauthorizedAccessException uae)
			{
				return new APIResult<List<RunDTO>>(null, $"ERROR: Access denied (MachineName={Environment.MachineName}), full error: {uae}");
			}
			catch (Exception e)
			{
				return new APIResult<List<RunDTO>>(null, e.ToString());
			}
		}

		[Route("last-24-hours-errors")]
		[HttpGet]
		public APIResult<List<RunDTO>> Last24hErrors()
		{
			DLs.InitializeConstsAsWebAPI();
			try
			{
				return new APIResult<List<RunDTO>>(DLs.JobLogging.SelectLast24hErrorLogs());
			}
			catch (Exception e)
			{
				return new APIResult<List<RunDTO>>(null, e.ToString());
			}
		}
	}
}