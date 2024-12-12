using System;

namespace BikeRentalManagement.DTOs.ResponseDTOs;

public class BookedDatesResponseDTO
{
    //public string? RegistrationNumber{get;set;}
    public ICollection<DateTime>? Dates {get;set;}

}
