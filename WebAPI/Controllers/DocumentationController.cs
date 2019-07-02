using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI.Controllers.Documentation;

namespace WebAPI.Controllers
{
	[Route("api/documentation")]
	[ApiController]
	public class DocumentationController
	{
		[Documentation("Documentation", null, "Displays all the controllers available in the API")]
		[HttpGet]
		public string Get()
		{
			DLs.InitializeConstsAsWebAPI();
			DocumentationCenter.XXX();
			return "Docu here\n\nok\ttabbed";
		}
	}
}
