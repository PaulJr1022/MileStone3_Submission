using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalManagement.Database.Entities;

public class RentalRequest
{


[Key]
    public int RequestId{get;set;}


   [ForeignKey("User")]
    public int UserId{get;set;}


[ForeignKey("Bike")]
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

    public User?User{get;set;}

    public Bike?Bike{get;set;}

}
