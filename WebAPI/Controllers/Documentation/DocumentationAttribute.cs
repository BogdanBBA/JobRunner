using System;

namespace WebAPI.Controllers.Documentation
{
	[AttributeUsage(AttributeTargets.Method)]
	public class DocumentationAttribute : Attribute
	{
		public string Controller { get; private set; }
		public string Endpoint { get; private set; }
		public string Description { get; private set; }

		public DocumentationAttribute(string controller, string endpoint, string description)
		{
			Controller = controller;
			Endpoint = endpoint;
			Description = description;
		}
	}
}
