using BikeRentalManagement.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService=reportService;
        }

        [HttpGet("TotalUsers")]
        public async Task<IActionResult>TotalUsers()
        {
            try{
                var data=await _reportService.TotalUsers();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("TotalBikes")]
        public async Task <IActionResult>TotalBikes()
        {
            try{
                var data=await _reportService.TotalBikes();
                return Ok (data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BookedBikes")]
        public async Task <IActionResult>TotalBooked()
        {
            try{
                var data=await _reportService.TotalBooked();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Revenue")]
        public async Task <IActionResult>Revenue()
        {
            try{
                var data=await _reportService.Revenue();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

         [HttpGet("GetRevenueByMonth")]
         public async Task<IActionResult>GetRevenueByMonth()
         {
            try{
                var data=await _reportService.GetRevenueByMonth();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
         }

         [HttpGet("GetRevenueByBike")]
         public async Task <IActionResult>GetRevenueByBike()
         {
            try{
                var data=await _reportService.GetRevenueByBike();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
         }

         [HttpGet("InventoryManagement")]
         public async Task<IActionResult>InventoryManagement()
         {
            try{
                var data=await _reportService.InventoryManagement();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
         }

         [HttpGet("UserHistory")]
         public async Task <IActionResult>UserHistory()
         {
            try{
                var data=await _reportService.UserHistory();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
         }

    }
   
}
