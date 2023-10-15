using DogeFriendsApi.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DogeFriendsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoatsController : ControllerBase
    {
        private readonly DataContext _context;

        public CoatsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/<CoatsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _context.Coats.Select(x => x.Name).ToList();
        }

        // GET api/<CoatsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CoatsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CoatsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CoatsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
