using System;

namespace BikeRentalManagement.DTOs.ResponseDTOs;

public class AllUserResponseDTO
{

    public int UserId { get; set; }

  
    public string FirstName { get; set; }

    
    public string LastName { get; set; }

 
   
    public string Email { get; set; }

   
    public string MobileNumber { get; set; }


    public string NIC { get; set; }


    public string Password { get; set; }

  
    public string LicenseNumber { get; set; }

   
 

    public string? LicenseImage {get;set;}

   
    public string ? CameraCapture{get;set;}

    public string Status {get;set;}

}
