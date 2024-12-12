using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BikeRentalManagement.Database.Entities;

public class User
{
    [Key]
    public int UserId { get; set; }

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
    public string Password { get; set; }

    [Required]
    public string LicenseNumber { get; set; }

    [Required]
    public Role Role { get; set; }

    // [Required]
    // public byte[] LicenseImage { get; set; }
    public string? LicenseImage {get;set;}

    // [Required]
    // public byte[] CameraCapture { get; set; }
    public string ? CameraCapture{get;set;}

    public Status Status {get;set;}


    public List<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();
    public List<Notification> Notifications { get; set; } = new List<Notification>();
    public List<Entrylog>Entrylogs{get;set;}=new List<Entrylog>();
}
