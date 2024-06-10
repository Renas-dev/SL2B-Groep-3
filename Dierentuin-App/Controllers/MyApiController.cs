using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dierentuin_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyApiController : ControllerBase
    {
        // GET: api/<MyApiController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MyApiController>/5/tempValue
        [HttpGet("{id}/{temp}")]
        public string Get(int id, string temp)
        {
            return id.ToString() + ", " + temp;
        }

        // POST api/<MyApiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MyApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MyApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
