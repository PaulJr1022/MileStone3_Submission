using System;

namespace BikeRentalManagement.DTOs;

public class BikeImageRequestDTO
{
    
    public int UnitId{get;set;}



    //public byte[] Image{get;set;}
    public required IFormFile Image{get;set;}

}
