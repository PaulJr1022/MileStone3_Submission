using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.DTOs;
using BikeRentalManagement.DTOs.RequestDTOs;
using BikeRentalManagement.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        private readonly IBikeService _bikeservice;

        public BikeController(IBikeService bikeService)
        {
            _bikeservice=bikeService;
        }

        [HttpPost("AddBrand")]
        public async Task <IActionResult>AddBrand(BrandRequestDTO brandRequestDTO)
        {
            try{
                var data=await _bikeservice.AddBrand(brandRequestDTO);
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AllBrands")]
        public async Task <IActionResult>AllBrands()
        {
           try{
            var data=await _bikeservice.AllBrands();
            return Ok(data);

           } catch(Exception ex)
           {
            return BadRequest(ex.Message);
           }
        }

        [HttpPost("AddModel")]
        public async Task <IActionResult> AddModel(ModelRequestDTO modelRequestDTO)
        {
            try{
                var data=await _bikeservice.AddModel(modelRequestDTO);
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetAllModels")]
        public async Task <IActionResult> GetAllModels()
        {
            try{
                var data=await _bikeservice.GetAllModels();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddBike")]
        public async Task <IActionResult>AddBike([FromBody]BikeRequestDTO bikeRequestDTO)
        {
            try{
                var data=await _bikeservice.AddBike(bikeRequestDTO);
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("UploadImages")]
        public async Task <IActionResult>AddImages([FromForm]BikeImageRequestDTO imageRequestDTO)
        {
            try{
                var data=await _bikeservice.AddImages(imageRequestDTO);
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AllBikes")]
        public async Task<IActionResult>AllBikes()
        {
            try{
                var data=await _bikeservice.AllBikes();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("DeleteBike{RegistrationNumber}")]
        public async Task <IActionResult>DeleteBike(string RegistrationNumber)
        {
            try{
                var data=await _bikeservice.DeleteBike(RegistrationNumber);
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetById{id}")]
        public async Task <IActionResult>GetById(int id)
        {
            try{
                var data=await _bikeservice.GetById(id);
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetByRegistrationNumber")]
       public async Task <IActionResult>GetByRegNo(string RegNo)
       {
        try{
            var data=await _bikeservice.GetByRegNo(RegNo);
            return Ok(data);

        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
       }

       [HttpPut("UpdateBike")]
       public async Task <IActionResult>UpdateBikeUnit([FromForm]BikeUnitUpdateDTO unit)
       {
        try{
            var data=await _bikeservice.UpdateBikeUnit(unit);
            return Ok(data);

        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
       }

       [HttpGet("GetModelByBrand{id}")]
       public async Task <IActionResult>GetModelByBrand(int id)
       {
        try{
            var data=await _bikeservice.GetModelByBrand(id);
            return Ok(data);

        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
       }
    }
}
