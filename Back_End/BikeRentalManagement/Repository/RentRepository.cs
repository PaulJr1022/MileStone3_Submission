using System;
using System.Net.WebSockets;
using BikeRentalManagement.Database;
using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.DTOs.ResponseDTOs;
using BikeRentalManagement.IRepository;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MimeKit;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using BikeRentalManagement.DTOs.RequestDTOs;

namespace BikeRentalManagement.Repository;

public class RentRepository:IRentRepository
{
    private readonly BikeDbContext _bikeDbContext;

    public RentRepository(BikeDbContext bikeDbContext)
    {
        _bikeDbContext=bikeDbContext;
    }
    public async Task<bool> RequestRent(RentalRequest rentRequest)
    {
        var checkBike=await _bikeDbContext.BikeUnits.FirstOrDefaultAsync(u=>u.RegistrationNumber == rentRequest.RegistrationNumber);
        if(checkBike == null)
        {
            throw new Exception("No Such Bike!");
        }
        var data=await _bikeDbContext.RentalRequests.AddAsync(rentRequest);
        await _bikeDbContext.SaveChangesAsync();
        if(data != null)
        {
          return true;
        }else{
            return false;
        }
    

    }

    public async Task <bool>CheckRequest(string RegistrationNumber,DateTime FromDate,DateTime ToDate)
    {

        var data=await _bikeDbContext.RentalRequests
                .Where(r=>r.RegistrationNumber==RegistrationNumber && r.Status == Status.Pending ).ToListAsync();
        foreach(var d in data)
        {
            if(d.FromDate < ToDate && d.ToDate > FromDate)
            {
                return true;

            }
        }
      return false;
    }

    public async Task <bool>CheckRequest(int id,string RegistrationNumber,DateTime FromDate,DateTime ToDate)
    {
        var data=await _bikeDbContext.RentalRequests.
                 Where(r=>r.RegistrationNumber == RegistrationNumber && r.Status == Status.Pending && r.RequestId!=id).
                 ToListAsync();
         foreach(var d in data)
        {
            if(d.FromDate < ToDate && d.ToDate > FromDate)
            {
                return true;

            }
        }
      return false;
    }

    public async Task<List<BookedDatesResponseDTO>> GetBikeBookedDates(string registrationNumber)
    {
        var data=await _bikeDbContext.RentalRequests
        .Where(r=>r.RegistrationNumber==registrationNumber && r.Status == Status.Pending).ToListAsync();

        var date=new List<BookedDatesResponseDTO>();
        foreach(var d in data)
        {
           var bookedDatesDto = new BookedDatesResponseDTO
        {
            Dates = new List<DateTime> { d.FromDate, d.ToDate }
        };

        date.Add(bookedDatesDto);
        }
        
        return date;
    }

    public async  Task <List<RentalRequest>> AllRequest()
    {
        var data=await _bikeDbContext.RentalRequests.ToListAsync();
        if(data != null)
        {
            return data;
        }else{
            return new List<RentalRequest>();
        }
    }

    public async Task <RentalRequest>GetRequestById(int id)
    {
        var data=await _bikeDbContext.RentalRequests.FirstOrDefaultAsync(r=>r.RequestId==id);
        return data;
    }

    public async  Task<bool>AcceptRejectRequest(int id,int status)
    {
        var request=await _bikeDbContext.RentalRequests.FirstOrDefaultAsync(r=>r.RequestId== id);
        if (request == null)
{
    throw new Exception("Invalid Request!");
}
        bool requeststatus=true;

        var getuser=await _bikeDbContext.Users.FirstOrDefaultAsync(u=>u.UserId== request.UserId);
        if(getuser == null)
        {
            throw new Exception("No Such User!");
        }


        if(request == null)
        {
            throw new Exception("Invalid Request!");
        }

        if(status == 2)
        {
            request.Status=Status.Rejected;

    var emailMessage = new MimeMessage();
    emailMessage.From.Add(new MailboxAddress("No-Reply", "Me2@gmail.com"));
    emailMessage.To.Add(new MailboxAddress("", getuser.Email));
    emailMessage.Subject = "Request Rejected!";
    emailMessage.Body = new TextPart("plain")
    {
        Text = $" {getuser.FirstName}\n Your Request is Rejected for Bike {request.RegistrationNumber}.\n"
    };

    using (var client = new SmtpClient())
    {
        try
        {
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("sivapakthangopikrishna69@gmail.com", "plev rbuw jsgh iipc");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;  
        }
    }

   
           
            requeststatus= false;
        }
        
        
        
        else if(status == 3)
        {
            request.Status=Status.Pending;
            request.Due=(request.ToDate-DateTime.UtcNow).Days;
         
var pdfPath = GeneratePdf(request, getuser);
    var emailMessage = new MimeMessage();
    emailMessage.From.Add(new MailboxAddress("No-Reply", "Me2@gmail.com"));
    emailMessage.To.Add(new MailboxAddress("", getuser.Email));
    emailMessage.Subject = "Request Accepted!";
    emailMessage.Body = new TextPart("plain")
    {
        Text = $" {getuser.FirstName}\n Your Request is Accepted.\n"
    };
     var attachment = new MimePart("application", "pdf")
        {
            Content = new MimeContent(File.OpenRead(pdfPath)),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = "RequestDetails.pdf"
        };
        var multipart = new Multipart("mixed");
        multipart.Add(emailMessage.Body);
        multipart.Add(attachment);
        emailMessage.Body = multipart;

    using (var client = new SmtpClient())
    {
        try
        {
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("sivapakthangopikrishna69@gmail.com", "plev rbuw jsgh iipc");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;  
        }
    }

   
            requeststatus= true;
        }

        else if(status == 4)
        {

            request.Status=Status.Returned;

    var emailMessage = new MimeMessage();
    emailMessage.From.Add(new MailboxAddress("No-Reply", "Me2@gmail.com"));
    emailMessage.To.Add(new MailboxAddress("", getuser.Email));
    emailMessage.Subject = "Return Confirmation!";
    emailMessage.Body = new TextPart("plain")
    {
        Text = $" {getuser.FirstName}\n Your Payment cleared for RequestID:{request.RequestId} .\n"
    };

    using (var client = new SmtpClient())
    {
        try
        {
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("sivapakthangopikrishna69@gmail.com", "plev rbuw jsgh iipc");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;  
        }
    }

 

        }
         await _bikeDbContext.SaveChangesAsync();
        return requeststatus;
    }
private string GeneratePdf(RentalRequest request, User user)
{
    var pdf = new PdfDocument();
    var page = pdf.AddPage();
    var graphics = XGraphics.FromPdfPage(page);
    var font = new XFont("Arial", 12, XFontStyle.Regular);

    request.Due=(request.ToDate.Date- DateTime.UtcNow).Days;
    graphics.DrawString("Bike Rental Request Details", font, XBrushes.Black, new XPoint(20, 40));
    graphics.DrawString($"Request ID:{request.RequestId}", font, XBrushes.Black, new XPoint(20, 60));
    graphics.DrawString($"User Name: {user.FirstName} {user.LastName}", font, XBrushes.Black, new XPoint(20, 80));
    graphics.DrawString($"Email: {user.Email}", font, XBrushes.Black, new XPoint(20, 120));
    graphics.DrawString($"Bike Registration Number: {request.RegistrationNumber}", font, XBrushes.Black, new XPoint(20, 160));
    graphics.DrawString($"From Date: {request.FromDate}", font, XBrushes.Black, new XPoint(20, 200));
    graphics.DrawString($"To Date: {request.ToDate}", font, XBrushes.Black, new XPoint(20, 240));
    graphics.DrawString($"From Location: {request.FromLocation}", font, XBrushes.Black, new XPoint(20, 280));
    graphics.DrawString($"To Location: {request.ToLocation}", font, XBrushes.Black, new XPoint(20, 320));
    graphics.DrawString($"Distance: {request.Distance} km", font, XBrushes.Black, new XPoint(20, 360));
    graphics.DrawString($"Amount: ${request.Amount}", font, XBrushes.Black, new XPoint(20, 400));
   // graphics.DrawString($"Due: {request.Due}", font, XBrushes.Black, new XPoint(20, 440));

Random rn=new Random();
var num=rn.Next();
    var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), $"RequestDetails{request.RequestId+"_"+num}.pdf");
    pdf.Save(pdfPath);

    return pdfPath;
}




public async Task<bool>CancelRequest(int id)
{
    var data=await _bikeDbContext.RentalRequests.FirstOrDefaultAsync(r=>r.RequestId==id);
    
    if(data == null)
    {
        throw new Exception("No Such Request!");
    }
     var getuser=await _bikeDbContext.Users.FirstOrDefaultAsync(u=>u.UserId== data.UserId);
        if(getuser == null)
        {
            throw new Exception("No Such User!");
        }


    data.Status=Status.Cancelled;
    data.Due=0;
      var emailMessage = new MimeMessage();
    emailMessage.From.Add(new MailboxAddress("No-Reply", "Me2@gmail.com"));
    emailMessage.To.Add(new MailboxAddress("", getuser.Email));
    emailMessage.Subject = "Request Cancelled!";
    emailMessage.Body = new TextPart("plain")
    {
        Text = $" {getuser.FirstName}\n You Cancelled Rental for Bike {data.RegistrationNumber}.\n"
    };

    using (var client = new SmtpClient())
    {
        try
        {
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("sivapakthangopikrishna69@gmail.com", "plev rbuw jsgh iipc");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;  
        }
    }


    await _bikeDbContext.SaveChangesAsync();
    return true;
}


public async  Task <bool> UpdateRequest(int id,RentalRequest rentRequest)
{
    var request=await _bikeDbContext.RentalRequests.FirstOrDefaultAsync(r=>r.RequestId == id);

    var checkBike=await _bikeDbContext.BikeUnits.FirstOrDefaultAsync(u=>u.RegistrationNumber == request.RegistrationNumber);

    if(request == null && checkBike==null)
    {
        throw new Exception("No Such Request!");
    }

    request.UserId=request.UserId;
    request.BikeId=rentRequest.BikeId;
    request.RegistrationNumber=rentRequest.RegistrationNumber;
    request.FromDate=rentRequest.FromDate;
    request.ToDate=rentRequest.ToDate;
    request.FromLocation=rentRequest.FromLocation;
    request.ToLocation=rentRequest.ToLocation;
    request.Distance=rentRequest.Distance;
    request.Due=0;
    request.Amount=rentRequest.Amount;
    request.Status=Status.Waiting;
       await _bikeDbContext.SaveChangesAsync();
      _bikeDbContext.Entry(request).State=EntityState.Modified;



   

    try
    {
        await _bikeDbContext.SaveChangesAsync();
        _bikeDbContext.Entry(request).Reload();

       
    }
    catch (DbUpdateConcurrencyException)
    {

        throw ;
    }

    return true;
  
}

public async Task <List<RentalResponseDTO>>RequestByUser(int id)
{
    var user=await _bikeDbContext.Users.FirstOrDefaultAsync(r=>r.UserId== id);
    if(user== null)
    {
        throw new Exception("No Such User!");
    }

    var request=await _bikeDbContext.RentalRequests.Where(r=>r.UserId == id) .Select(r => new RentalResponseDTO
        {
            RequestId=r.RequestId,
            BikeId = r.BikeId,
            RegistrationNumber = r.RegistrationNumber,
            FromDate = r.FromDate,
            ToDate = r.ToDate,
            FromLocation = r.FromLocation,
            ToLocation = r.ToLocation,
            Distance = r.Distance,
            Amount = r.Amount,
            Due = r.Due,
            Status = r.Status.ToString()
        })
        .ToListAsync();

    return request;
 

}


public async Task <ICollection<object>>CountHistory(int id)
{
    var request=await _bikeDbContext.RentalRequests
               .Where(r=>r.UserId == id ).ToListAsync();

    var totalReq=request.Count();
    var totalReturn=request.Where(r=>r.Status == Status.Returned).Count();
    var totalPending=request.Where(r=>r.Status == Status.Pending).Count();
    var totalLateReturn=request.Where(r=>r.Due < 0).Count();

    var result = new List<object>
    {
        new { Label = "Total Requests", Value = totalReq },
        new { Label = "Total Returned", Value = totalReturn },
        new { Label = "Total Pending", Value = totalPending },
        new { Label = "Total Late Returns", Value = totalLateReturn }
    };

    return result;

}

public async Task<bool> LateReturns()
{
    var data = await _bikeDbContext.RentalRequests
        .Where(r => r.Status == Status.Pending && r.Due < 0)
        .ToListAsync();

    foreach (var req in data)
    {
        
        var usermail = await _bikeDbContext.Users.FirstOrDefaultAsync(u => u.UserId == req.UserId);
        int overdueDays = Math.Abs(req.Due);

    double fine = req.Amount * 0.1 * overdueDays;

   
       req.Amount += (int)fine;
        if (usermail == null)
        {
            Console.WriteLine($"User not found for UserId: {req.UserId}");
            continue;
        }


        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("No-Reply", "Me2@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", usermail.Email));
        emailMessage.Subject = "Late Return Notice";
        emailMessage.Body = new TextPart("plain")
        {
            Text = $"Dear {usermail.FirstName},\n\nYour rental request is overdue for Request ID:{req.RequestId}. Please return the bike as soon as possible. Your Final Charge :{req.Amount}"
        };


        try
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("sivapakthangopikrishna69@gmail.com", "plev rbuw jsgh iipc");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email sending failed for UserId: {req.UserId}. Error: {ex.Message}");
            continue;
        }


       
    }

   
    await _bikeDbContext.SaveChangesAsync();

    return true;
}

public async Task<List<RentalRequest>> Reminder()
{
   var today = DateTime.UtcNow.AddHours(5.5).Date;
   var data = await _bikeDbContext.RentalRequests .Where(r => r.FromDate.Date == today && r.Status == Status.Pending) 
             .ToListAsync();

   

    return data;
}

public async Task <string> GetPhoneNumber(int id)
{
    var data=await _bikeDbContext.Users.Where(u=>u.UserId == id).Select(u=>u.MobileNumber).FirstOrDefaultAsync();
    return data;
}

public async Task <string> GetEmail(int id)
{
    var data=await _bikeDbContext.Users.Where(u=>u.UserId == id).Select(u=>u.Email).FirstOrDefaultAsync();
    return data;
}

}
