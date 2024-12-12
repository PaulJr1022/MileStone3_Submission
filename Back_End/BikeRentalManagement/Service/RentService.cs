using System;
using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.DTOs.RequestDTOs;
using BikeRentalManagement.DTOs.ResponseDTOs;
using BikeRentalManagement.IRepository;
using BikeRentalManagement.IService;
using Microsoft.Extensions.Configuration;
//using Twilio; 
//using Twilio.Rest.Api.V2010.Account;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net.WebSockets;


namespace BikeRentalManagement.Service;

public class RentService:IRentService
{
    private readonly IRentRepository _rentRepository;
    private readonly IConfiguration _configuration;

    public RentService(IRentRepository rentRepository,IConfiguration configuration)
    {
        _rentRepository=rentRepository;
        _configuration=configuration;
    }
    public async Task<bool>RequestRent(RentRequestDTO rentRequestDTO)
     {
        var rentRequest=new RentalRequest
        {
            UserId=rentRequestDTO.UserId,
            BikeId=rentRequestDTO.BikeId,
            RegistrationNumber=rentRequestDTO.RegistrationNumber,
            FromDate=rentRequestDTO.FromDate,
            ToDate=rentRequestDTO.ToDate,
            FromLocation=rentRequestDTO.FromLocation,
            ToLocation=rentRequestDTO.ToLocation,
            Distance=rentRequestDTO.Distance,
            Amount=rentRequestDTO.Amount,
            Status=Status.Waiting

        };

        var checkRequest=await _rentRepository.CheckRequest(rentRequestDTO.RegistrationNumber,rentRequestDTO.FromDate,rentRequestDTO.ToDate);

        if(checkRequest)
        {
            throw new Exception("Already Booked!");
        }

        var data=await _rentRepository.RequestRent(rentRequest);

        if(data)
        {
            return true;
        }else{
            return false;
        }

     }

     public async Task<List<BookedDatesResponseDTO>>GetBikeBookedDates(string RegistrationNumber)
     {

        var data=await _rentRepository.GetBikeBookedDates(RegistrationNumber);

        if(data == null)
        {
            return new List<BookedDatesResponseDTO>();
        }

        return data;

     }
 public async Task <List<AllRentalResponseDTO>> AllRequest()
 {
    var data=await _rentRepository.AllRequest();
    if(data == null)
    {
        throw new Exception("Nothing Found!");
    }

    var response=new List<AllRentalResponseDTO>();

    foreach(var d in data)
    {
        var rentalresponse=new AllRentalResponseDTO
        {
            RequestId=d.RequestId,
            UserId=d.UserId,
            BikeId=d.BikeId,
            RegistrationNumber=d.RegistrationNumber,
            FromDate=d.FromDate,
            ToDate=d.ToDate,
            FromLocation=d.FromLocation,
            ToLocation=d.ToLocation,
            Distance=d.Distance,
            Amount=d.Amount,
            Due=d.Due,
            Status=d.Status.ToString()


        };

        response.Add(rentalresponse);
    }
    return response;

 }

 public async  Task <RentalRequest>GetRequestById(int id)
 {
    var data=await _rentRepository.GetRequestById(id);

    if(data == null)
    {
        throw new Exception("No Data Found!");
    }
    return data;
 }
 public async  Task<bool>AcceptRejectRequest(int id,int status)
 {
    var data=await _rentRepository.AcceptRejectRequest(id,status);
    if(data)
    {
        return true;
    }else{
        return false;
    }
 }

 public async Task<bool>CancelRequest(int id)
 {
    var data=await _rentRepository.CancelRequest(id);
    if(data)
    {
        return true;
    }else{
        return false;
    }
 }

 public async  Task <bool> UpdateRequest(int id,RentRequestDTO rentRequestDTO)
 {

      var rentRequest=new RentalRequest
        {
            UserId=0,
            BikeId=rentRequestDTO.BikeId,
            RegistrationNumber=rentRequestDTO.RegistrationNumber,
            FromDate=rentRequestDTO.FromDate,
            ToDate=rentRequestDTO.ToDate,
            FromLocation=rentRequestDTO.FromLocation,
            ToLocation=rentRequestDTO.ToLocation,
            Distance=rentRequestDTO.Distance,
            Amount=rentRequestDTO.Amount,
            Status=Status.Waiting

        };

        var checkRequest=await _rentRepository.CheckRequest(id,rentRequestDTO.RegistrationNumber,rentRequestDTO.FromDate,rentRequestDTO.ToDate);

        if(checkRequest )
        {
            throw new Exception("Already Booked!");
        }

    var data=await _rentRepository.UpdateRequest(id,rentRequest);

    if(data)
    {
        return true;
    }else{
        return false;
    }
 }


 public async Task <List<RentalResponseDTO>>RequestByUser(int id)
 {
    var data=await _rentRepository.RequestByUser(id);
    if(data == null)
    {
        throw new Exception("No Data Found !");
    }
    return data;
 }

public async Task <ICollection<object>>CountHistory(int id)
{
    var data=await _rentRepository.CountHistory(id);

    if(data == null)
    {
        
    }
    return data;
}

public async Task <bool> LateReturns()
{
    var data=await _rentRepository.LateReturns();
    return data;
}





 public async Task<List<RentalRequest>> Reminder()
{
    var data = await _rentRepository.Reminder();

    if (data == null)
    {
        return new List<RentalRequest>();
    }

    //var twilioSettings = _configuration.GetSection("Twilio").Get<TwilioSettings>();

    // if (twilioSettings == null )
    // {
        
    //     return new List<RentalRequest>();
    // }

    //TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);

    foreach (var d in data)
    {
        var phoneNumber = await _rentRepository.GetPhoneNumber(d.UserId);
        var Email=await _rentRepository.GetEmail(d.UserId);

        if (string.IsNullOrEmpty(phoneNumber))
        {
           
            continue;
        }

        // try
        // {
        //     phoneNumber="766946959";
        //     phoneNumber=$"+94{phoneNumber}";
        //     var to = new Twilio.Types.PhoneNumber(phoneNumber);
        //     var message = MessageResource.Create(
        //         to: to,
        //         from: new Twilio.Types.PhoneNumber("13203355850"), 
        //         body: $"You have booked a ride from {d.FromLocation} to {d.ToLocation}"
        //     );

        //     Console.WriteLine($"Message sent to {phoneNumber}: {message.Sid}");
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine($"Failed to send message to {phoneNumber}: {ex.Message}");
        // }

       await  SendEmail(d.FromLocation,d.ToLocation,Email);
    }

    return data;
}

public async Task <bool> SendEmail(string From,string To,string Email)
{
    var emailMessage = new MimeMessage();
    emailMessage.From.Add(new MailboxAddress("No-Reply", "Me2@gmail.com"));
    emailMessage.To.Add(new MailboxAddress("", Email));
    emailMessage.Subject = "Ready For a Ride!";
    emailMessage.Body = new TextPart("plain")
    {
        Text = $"  Your have booked a ride today from :{From} To {To} .\n"
    };

    using (var client = new SmtpClient())
    {
        try
        {
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("sivapakthangopikrishna69@gmail.com", "plev rbuw jsgh iipc");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;  
        }
}

}
}


