using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BikeRentalManagement.Database.Entities;

public class Bike
{
    [Key]
    public int BikeId { get; set; }



    [ForeignKey("Model")]
    public int ModelId { get; set; }

   
 


    public Model Model { get; set; }

  


    public List<BikeUnit> BikeUnits { get; set; } = new List<BikeUnit>();
    public List <RentalRequest> RentalRequests{get;set;}=new List<RentalRequest>();
}
