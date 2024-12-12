using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalManagement.Database.Entities;

public class BikeUnit
{
    [Key]
    public int UnitId{get;set;}

 [ForeignKey("Bike")]
    public int BikeId{get;set;}

    [Required]
    public string RegistrationNumber{get;set;}

    [Required]
    public int Year{get;set;}

    [Required]
    public int RentPerDay{get;set;}

    public List<BikeImages>bikeImages{get;set;}

    public Bike?Bike{get;set;}

   


}
