using System;
using BikeRentalManagement.Database.Entities;

namespace BikeRentalManagement.DTOs.ResponseDTOs;

public class BikeResponseDTO
{

public string ? BrandName{get;set;}
 public string? ModelName { get; set; }
 public List<BikeUnit> BikeUnits { get; set; } = new List<BikeUnit>();
}
