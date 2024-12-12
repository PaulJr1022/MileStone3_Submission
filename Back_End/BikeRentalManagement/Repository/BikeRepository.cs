using System;
using BikeRentalManagement.Database;
using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.DTOs;
using BikeRentalManagement.DTOs.RequestDTOs;
using BikeRentalManagement.DTOs.ResponseDTOs;
using BikeRentalManagement.IRepository;
using BikeRentalManagement.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BikeRentalManagement.Repository;

public class BikeRepository:IBikeRepository
{
    private readonly BikeDbContext _bikeDbContext;

    public BikeRepository(BikeDbContext bikeDbContext)
    {
        _bikeDbContext=bikeDbContext;
    }

    public async Task <bool> AddBrand(Brand brand)
    {
        var findbrand=await _bikeDbContext.Brands.FirstOrDefaultAsync(b=>b.BrandName==brand.BrandName);
        if(findbrand != null)
        {
            throw new Exception("Brand Already Exists");
        }

        var data=await _bikeDbContext.Brands.AddAsync(brand);
        await _bikeDbContext.SaveChangesAsync();

        if(data != null)
        {
            return true;
        }else{
            return false;
        }
    }

    public async Task <List<Brand>>AllBrands()
    {
        var data=await _bikeDbContext.Brands.ToListAsync();
        return data;
    }

    public async Task <bool> AddModel(Model modelRequest)
    {
        var checkModel=await _bikeDbContext.Models.FirstOrDefaultAsync(m=>m.ModelName==modelRequest.ModelName);

        if(checkModel!=null)
        {
            throw new Exception ("Model Already Exists !");
        }
        var data=await _bikeDbContext.Models.AddAsync(modelRequest);
        await _bikeDbContext.SaveChangesAsync();

        if(data != null)
        {
            return true;
        }else{
            return false;
        }
    }

    public async Task <List<Brand>> GetAllModels()
    {
        var  result=await _bikeDbContext.Brands.Include(b=>b.Models).ToListAsync();
        return result;
    }

    // public async Task <bool>AddBike(BikeRequestDTO bikeRequestDTO)
    // {
       

    // }

    public async Task <bool>CheckRegNo(string RegistrationNumber)
    {
         var data=await _bikeDbContext.BikeUnits.FirstOrDefaultAsync(u=>u.RegistrationNumber==RegistrationNumber);

         if(data != null)
         {
             return false;
         }else{
            return true;
         }
    }
 public async  Task<int>FindModelId(string ModelName)
 {
    var findbike=await _bikeDbContext.Models.FirstOrDefaultAsync(m=>m.ModelName==ModelName);
    if(findbike == null)
    {
        throw new Exception("No Such Model!");
    }

    var bikeId=findbike.ModelId;
    return bikeId;
 }

public async Task<int> AddModelBike(int modelId)
{
    var newBike = new Bike
    {
        ModelId = modelId
    };

    await _bikeDbContext.Bikes.AddAsync(newBike);
    await _bikeDbContext.SaveChangesAsync();
   
    return newBike.BikeId;
}


public async Task<bool> AddBikeUnit(BikeUnit unit)
{
 

    try
    {
        await _bikeDbContext.BikeUnits.AddAsync(unit);
        await _bikeDbContext.SaveChangesAsync();

    }
    catch (Exception ex)
    {

        return false; 
    }

    return true;
}



public async Task<bool> AddBikeImages(BikeImages imageRequest)
{
    if (imageRequest != null )
    {
        await _bikeDbContext.BikeImages.AddRangeAsync(imageRequest);
        await _bikeDbContext.SaveChangesAsync();
     
        return true;
    }
    else
    {
        Console.WriteLine("No Images to Add");
        return false;
    }
}



public async Task<List<BikeResponseDTO>> AllBikes()
{
 

    var data = await _bikeDbContext.Bikes
        .Include(b => b.BikeUnits)
        .ThenInclude(bi => bi.bikeImages)
        .Include(m => m.Model)
        .ThenInclude(b => b.Brand)
   
        .ToListAsync();


    var response = data
        .Where(bike => bike.BikeUnits.Count > 0) 
        .Select(bike => new BikeResponseDTO
        {
            BrandName = bike.Model?.Brand.BrandName,
            ModelName = bike.Model?.ModelName,
            BikeUnits = bike.BikeUnits
                .Select(unit => new BikeUnit
                {
                    UnitId = unit.UnitId,
                    BikeId = unit.BikeId,
                    RegistrationNumber = unit.RegistrationNumber,
                    Year = unit.Year,
                    RentPerDay = unit.RentPerDay,
                    bikeImages = unit.bikeImages.ToList()
                }).ToList()
        }).ToList();

    return response;
}


public async Task<bool>DeleteBike(string RegistrationNumber)
{
    var data=await _bikeDbContext.BikeUnits.FirstOrDefaultAsync(b=>b.RegistrationNumber==RegistrationNumber);
    if(data == null)
    {
        return false;
    }
    var deletebike= _bikeDbContext.BikeUnits.Remove(data);
    await _bikeDbContext.SaveChangesAsync();
    return true;
}

public async Task <bool>checkRental(string RegistrationNumber)
{
        var data = await _bikeDbContext.RentalRequests.FirstOrDefaultAsync(r => r.RegistrationNumber == RegistrationNumber && r.Status==Status.Pending);
        if(data != null)
        {
            return false;
        }else{
            return true;
        }
}

public async Task<Bike> GetById(int id)
{
    
    var findbike = await _bikeDbContext.Bikes
        .Include(b => b.BikeUnits)  
        .ThenInclude(bu => bu.bikeImages)  
        .FirstOrDefaultAsync(b => b.BikeId == id);

    if (findbike == null)
    {
        throw new Exception("Error: Bike not found");
    }



    return findbike;
}

public async Task <Bike>GetByRegNo(string RegNo)
{
    var findbike=await _bikeDbContext.BikeUnits
                
                 .Include(bi=>bi.bikeImages)
                 .FirstOrDefaultAsync(b=>b.RegistrationNumber==RegNo);

      if (findbike == null)
    {
        throw new Exception("Error: Bike not found");
    }


 var getbike = await _bikeDbContext.Bikes
        .Include(b => b.BikeUnits.Where(bu=>bu.UnitId==findbike.UnitId))  
        .ThenInclude(bu => bu.bikeImages)  
        .FirstOrDefaultAsync(b => b.BikeId == findbike.BikeId);

    if(getbike!=null)
    {
        return getbike;
    }else{
        throw new Exception("Invalid!");
    }

}

// public async Task<bool>UpdateBike(string RegistrationNumber,BikeUnit unit)
// {
//     var existingBike=await _bikeDbContext.BikeUnits
//                     .Include(b=>b.bikeImages)
//                     .FirstOrDefaultAsync(b=>b.RegistrationNumber==RegistrationNumber);

//     if(existingBike == null)
//     {
//         throw new Exception("Invalid");
//     }

//     existingBike.RegistrationNumber=RegistrationNumber;
//     existingBike.Year=unit.Year;
//     existingBike.RentPerDay=unit.RentPerDay;
//     existingBike.bikeImages.Clear();

//     foreach (var bikeImg in unit.bikeImages)
//     {
//         existingBike.bikeImages.Add(new BikeImages
//         {
//             Image = bikeImg.Image
//         });
//     }

  
//     await _bikeDbContext.SaveChangesAsync();

//     return true;
// }
  public async Task<bool> UpdateBikeUnit(BikeUnit bikeUnit)
    {
        _bikeDbContext.BikeUnits.Update(bikeUnit);
        return await _bikeDbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateBikeImages(int unitId,List<BikeImages> bikeImages)
    {
        var findImage=await _bikeDbContext.BikeImages.Where(i=>i.UnitId == unitId).ToListAsync();

        if(findImage != null)
        {
             _bikeDbContext.BikeImages.RemoveRange(findImage);
             _bikeDbContext.BikeImages.UpdateRange(bikeImages);
        }
     
        return await _bikeDbContext.SaveChangesAsync() > 0;
    }

    public async Task <List<Model>>GetModelByBrand(int id)
    {
        var data=await _bikeDbContext.Models.Where(m=>m.BrandId==id).ToListAsync();
        return data;
    }
}
