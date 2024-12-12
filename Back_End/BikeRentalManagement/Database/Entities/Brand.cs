using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BikeRentalManagement.Database.Entities;

public class Brand
{
    [Key]
    public int BrandId { get; set; }

    [Required]
    public string BrandName { get; set; }

 
    public List<Model> Models { get; set; } = new List<Model>();
}
