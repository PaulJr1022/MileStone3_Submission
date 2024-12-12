using System;

namespace BikeRentalManagement.DTOs.ResponseDTOs;

public class RentalResponseDTO
{
    public int RequestId{get;set;}
    
    public int BikeId{get;set;}

    public string RegistrationNumber{get;set;}

    public DateTime FromDate{get;set;}

    public DateTime ToDate {get;set;}

    public string FromLocation {get;set;}

    public string ToLocation {get;set;}

    public int Distance {get;set;}

    public int Amount {get;set;}

    public int Due {get;set;}

    public String Status {get;set;}

}
