using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async  Task<IActionResult>Test()
        {
            try{
                return Ok("API working");

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
