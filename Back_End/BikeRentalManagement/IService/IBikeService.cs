using System;
using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.DTOs;
using BikeRentalManagement.DTOs.RequestDTOs;
using BikeRentalManagement.DTOs.ResponseDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalManagement.IService;

public interface IBikeService
{
    Task <bool> AddBrand(BrandRequestDTO brandRequestDTO);
    Task <List<Brand>>AllBrands();

    Task <bool> AddModel(ModelRequestDTO modelRequestDTO);

    Task <List<Brand>> GetAllModels();

    Task <List<BikeUnit>>AddBike(BikeRequestDTO bikeRequestDTO);
    Task <bool>AddImages(BikeImageRequestDTO imageRequestDTO);

    Task<List<BikeResponseDTO>>AllBikes();

    Task <bool>DeleteBike(string RegistrationNumber);

    Task <Bike>GetById(int id);

    Task <Bike>GetByRegNo(string RegNo);

    Task <bool>UpdateBikeUnit(BikeUnitUpdateDTO unit);

    Task <List<Model>>GetModelByBrand(int id);

}
