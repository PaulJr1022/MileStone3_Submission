using System;
using BikeRentalManagement.Database.Entities;

namespace BikeRentalManagement.DTOs.RequestDTOs;

public class RentRequestDTO
{
     public int UserId{get;set;}



    public int BikeId{get;set;}

    public string RegistrationNumber{get;set;}

    public DateTime FromDate{get;set;}

    public DateTime ToDate {get;set;}

    public string FromLocation {get;set;}

    public string ToLocation {get;set;}

    public int Distance {get;set;}

    public int Amount {get;set;}

    public int Due {get;set;}

    public Status Status {get;set;}

}
