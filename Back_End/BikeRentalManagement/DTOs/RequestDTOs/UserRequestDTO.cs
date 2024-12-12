using System;
using System.ComponentModel.DataAnnotations;
using BikeRentalManagement.Database.Entities;

namespace BikeRentalManagement.DTOs.RequestDTOs;

public class UserRequestDTO
{

     [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Phone]
    public string MobileNumber { get; set; }

    [Required]
    public string NIC { get; set; }

   

    [Required]
    public string LicenseNumber { get; set; }

    [Required]
    public Role Role { get; set; }

   public IFormFile LicenseImage { get; set; }
    public IFormFile CameraCapture { get; set; }

    public Status Status{get;set;} 

}
