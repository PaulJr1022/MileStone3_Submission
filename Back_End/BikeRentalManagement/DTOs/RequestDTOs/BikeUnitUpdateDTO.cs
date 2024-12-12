using System;

namespace BikeRentalManagement.DTOs.RequestDTOs;

public class BikeUnitUpdateDTO
{
    public int UnitId { get; set; }

   
    public string RegistrationNumber { get; set; }

   
    public int Year { get; set; }

   
    public int RentPerDay { get; set; }

    public List<IFormFile> BikeImages { get; set; } = new List<IFormFile>();
}
