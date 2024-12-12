using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BikeRentalManagement.Database.Entities;

public class Model
{
    [Key]
    public int ModelId { get; set; }

    [Required]
    public string ModelName { get; set; }

     [ForeignKey("Brand")]
    public int BrandId { get; set; }

 
    public Brand Brand { get; set; }

 
    public List<Bike> Bikes { get; set; } = new List<Bike>();
}
