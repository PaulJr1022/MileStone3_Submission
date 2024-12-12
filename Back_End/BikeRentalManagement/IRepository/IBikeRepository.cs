using System;
using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.DTOs;
using BikeRentalManagement.DTOs.RequestDTOs;
using BikeRentalManagement.DTOs.ResponseDTOs;

namespace BikeRentalManagement.IRepository;

public interface IBikeRepository
{
 Task<bool>AddBrand(Brand brandRequest);

 Task <List<Brand>>AllBrands();

 Task <bool> AddModel(Model modelRequest);

 Task <List<Brand>> GetAllModels();

 //Task <bool>AddBike(BikeRequestDTO bikeRequestDTO);
 Task <bool>CheckRegNo(string RegistrationNumbers);
 Task<int>FindModelId(string ModelName);

 //Task<int>getbikeId();

//Task <bool>AddBikeImages(List<BikeImages> bikeImages);
Task <bool>AddBikeUnit(BikeUnit unit);
Task <int> AddModelBike(int modelId);
Task<List<BikeResponseDTO>>AllBikes();
Task<bool>checkRental(string RegistrationNumber);
Task<bool>DeleteBike(string RegistrationNumber);
Task <Bike>GetById(int id);
Task <Bike>GetByRegNo(string RegNo);
//Task<bool>UpdateBike(string RegistrationNumber,BikeUnit unit);
 Task <bool>AddBikeImages(BikeImages imageRequest);


 Task<bool> UpdateBikeUnit(BikeUnit bikeUnit);
 Task<bool> UpdateBikeImages(int unitId,List<BikeImages> bikeImages);
 Task <List<Model>>GetModelByBrand(int id);
}
