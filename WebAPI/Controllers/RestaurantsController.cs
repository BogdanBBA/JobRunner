using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAPI.Controllers.Restaurants;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class RestaurantsController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public APIResult<List<Restaurant>> Get()
        {
            return new APIResult<List<Restaurant>>(null, "Not yet implemented");
        }

        /*// GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
