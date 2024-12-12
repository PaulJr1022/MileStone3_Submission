using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalManagement.Database.Entities;

public class Entrylog
{

    [Key]
    public int EntryId{get;set;}


   [ForeignKey("User")]
    public int UserId {get;set;}

    public string RegistrationNumber{get;set;}

    public User?User{get;set;}

    

}
