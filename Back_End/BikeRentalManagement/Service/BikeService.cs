using System;
using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.DTOs.RequestDTOs;
using BikeRentalManagement.IRepository;
using BikeRentalManagement.IService;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Org.BouncyCastle.Bcpg.Attr;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BikeRentalManagement.Migrations;
using BikeRentalManagement.DTOs;
using System.IO;
using BikeRentalManagement.DTOs.ResponseDTOs;


namespace BikeRentalManagement.Service;

public class BikeService:IBikeService
{
    private readonly IBikeRepository _bikerepository;

    public BikeService(IBikeRepository bikeRepository)
    {
        _bikerepository=bikeRepository;
    }
    public async Task<bool>AddBrand(BrandRequestDTO brandRequestDTO)
    {
        var brand=new Brand{
            BrandName=brandRequestDTO.BrandName

        };

        var data=await _bikerepository.AddBrand(brand);
        if(data)
        {
            return true;
        }else{
            return false;
        }

    }

    public async Task <List<Brand>>AllBrands()
    {
        var data=await _bikerepository.AllBrands();

        if(data == null)
        {
            throw new Exception("No Bikes Found!");
        }else{
            return data;
        }
    }

 public async Task <bool> AddModel(ModelRequestDTO modelRequestDTO)
 {
    var model=new Model
    {
        ModelName=modelRequestDTO.ModelName,
        BrandId=modelRequestDTO.BrandId

    };

    var data=await _bikerepository.AddModel(model);

    if(data)
    {
        return true;
    }else{
        return false;
    }
 }

 public async  Task <List<Brand>> GetAllModels()
 {
    var data=await _bikerepository.GetAllModels();

    if(data != null)
    {
        return data;
    }else{
        throw new Exception("No Data Found!");
    }
 }

public async Task<List<BikeUnit>> AddBike([FromBody] BikeRequestDTO bikeRequestDTO)
{
    var imageDirectory = Path.Combine("wwwroot", "images_bike");
    if (!Directory.Exists(imageDirectory))
    {
        Directory.CreateDirectory(imageDirectory);
    }

var distinctList = bikeRequestDTO.BikeUnits .Select(bkUnit => bkUnit.RegistrationNumber) .Distinct() .ToList();
if(distinctList.Count()!=bikeRequestDTO.BikeUnits.Count())
{
    throw new Exception("Duplicate Registration Number!");
}
   
    var bUnit=new  List<BikeUnit>();
    var modelId = await _bikerepository.FindModelId(bikeRequestDTO.ModelName);
    var getbikeid = await _bikerepository.AddModelBike(modelId);

    if (getbikeid <= 0)
    {
        throw new Exception("No ID!");
    }

    foreach (var bikeUnit in bikeRequestDTO.BikeUnits)
    {
        var chkReg = await _bikerepository.CheckRegNo(bikeUnit.RegistrationNumber);
        
        if (!chkReg)
        {
            throw new Exception("Registration Number Already Exists!");
        }

        var unit = new BikeUnit
        {
            BikeId = getbikeid,
            RegistrationNumber = bikeUnit.RegistrationNumber,
            Year = bikeUnit.Year,
            RentPerDay = bikeUnit.RentPerDay
        };

        var unitId = await _bikerepository.AddBikeUnit(unit);
     if(!unitId)
     {
        throw new Exception("Failed To Add Unit!");
     }
       bUnit.Add(unit);

    }

   

    // foreach (var bikeUnt in bikeRequestDTO.BikeUnits)
    // {
    //     var unit = new BikeUnit
    //     {
    //         BikeId = getbikeid,
    //         RegistrationNumber = bikeUnt.RegistrationNumber,
    //         Year = bikeUnt.Year,
    //         RentPerDay = bikeUnt.RentPerDay
    //     };

    //     var unitId = await _bikerepository.AddBikeUnit(unit);
    //  if(!unitId)
    //  {
    //     throw new Exception("Failed To Add Unit!");
    //  }
    //    bUnit.Add(unit);
    //     //id.Add(unitId);
    // }

   
return bUnit;
   
}

public async  Task <bool>AddImages([FromForm]BikeImageRequestDTO imageRequestDTO)
{
            if (imageRequestDTO.Image == null || imageRequestDTO.Image.Length == 0)
        {
            return false;
        }
         
        var bikeImages = new List<BikeImages>();

     
        var imageDirectory = Path.Combine("wwwroot", "bike_images");
        if (!Directory.Exists(imageDirectory))
        {
            Directory.CreateDirectory(imageDirectory);
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{imageRequestDTO.Image.FileName}";
        var filePath = Path.Combine(imageDirectory, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageRequestDTO.Image.CopyToAsync(stream);
        }

    
        var image=new BikeImages
        {
            UnitId=imageRequestDTO.UnitId,
            Image=filePath

        };
        await _bikerepository.AddBikeImages(image);
        return true;

}


public async Task<List<BikeResponseDTO>>AllBikes()
{
    var data=await _bikerepository.AllBikes();

    if(data == null)
    {
        throw new Exception("Data Not Found!");
        
    }
    foreach(var d in data)
    {
        foreach(var u in d.BikeUnits)
        {
            foreach(var i in u.bikeImages)
            {
                i.Image=i.Image?.Replace("wwwroot","");
                i.Image=i.Image?.Replace("\\","");
                i.Image=i.Image?.Replace("bike_images","bike_images/");
            }
        }
    }
    return data;
}

public async Task <bool>DeleteBike(string RegistrationNumber)
{
    var checkRental=await _bikerepository.checkRental(RegistrationNumber);

    if(!checkRental)
    {
        throw new Exception("Bike is Booked !");
    }

    var data=await _bikerepository.DeleteBike(RegistrationNumber);
    if(data)
    {
        return true;
    }else{
        return false;
    }
}

public async  Task <Bike>GetById(int id)
{
    var data=await _bikerepository.GetById(id);
    if(data == null)
    {
        throw new Exception("No Data!");
    }
    return data;
    
}

public async Task <Bike>GetByRegNo(string RegNo)
{
    var data=await _bikerepository.GetByRegNo(RegNo);
    foreach(var d in data.BikeUnits)
    {
        foreach(var img in d.bikeImages)
        {
            img.Image=img.Image.Replace("wwwroot\\","").Replace("\\","/");
        }
    }

    if(data == null)
    {
        throw new Exception("No Such Bike!");
    }
    return data;
}

public async Task<bool> UpdateBikeUnit(BikeUnitUpdateDTO bikeUnitUpdateDTO)
{
    var bike = await _bikerepository.GetByRegNo(bikeUnitUpdateDTO.RegistrationNumber);

    if (bike == null )
    {
        throw new Exception("Bike unit not found.");
    }

  
    var bikeUnit = bike.BikeUnits.FirstOrDefault(bu => bu.UnitId == bikeUnitUpdateDTO.UnitId);

    if (bikeUnit == null)
    {
        throw new Exception("Bike unit not found.");
    }

 
    bikeUnit.RegistrationNumber = bikeUnitUpdateDTO.RegistrationNumber;
    bikeUnit.Year = bikeUnitUpdateDTO.Year;
    bikeUnit.RentPerDay = bikeUnitUpdateDTO.RentPerDay;

    var unitUpdated = await _bikerepository.UpdateBikeUnit(bikeUnit);

    if (!unitUpdated)
    {
        throw new Exception("Failed to update bike unit.");
    }

 
    if (bikeUnitUpdateDTO.BikeImages != null && bikeUnitUpdateDTO.BikeImages.Any())
    {
        var imageDirectory = Path.Combine("wwwroot", "bike_images");
        if (!Directory.Exists(imageDirectory))
        {
            Directory.CreateDirectory(imageDirectory);
        }

        var bikeImages = new List<BikeImages>();
        foreach (var bikeImage in bikeUnitUpdateDTO.BikeImages)
        {
            if (bikeImage != null && bikeImage.Length > 0)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{bikeImage.FileName}";
                var filePath = Path.Combine(imageDirectory, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await bikeImage.CopyToAsync(stream);
                }

                bikeImages.Add(new BikeImages
                {
                    UnitId = bikeUnitUpdateDTO.UnitId,
                    Image = filePath
                });
            }
        }

        var imagesUpdated = await _bikerepository.UpdateBikeImages(bikeUnitUpdateDTO.UnitId,bikeImages);
        if (!imagesUpdated)
        {
            throw new Exception("Failed to update bike images.");
        }
    }

    return true;
}
 public async  Task <List<Model>>GetModelByBrand(int id)
 {
    var data=await _bikerepository.GetModelByBrand(id);
    if(data == null)
    {
        throw new Exception("No Data!");
    }
    return data;
 }
    
}

