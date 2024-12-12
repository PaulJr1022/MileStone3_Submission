using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BikeRentalManagement.Database;
using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.DTOs.RequestDTOs;
using BikeRentalManagement.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using BikeRentalManagement.DTOs.ResponseDTOs;

namespace BikeRentalManagement.Repository;

public class UserRepository:IUserRepository
{
    private readonly BikeDbContext _bikeDbContext;
     private readonly IConfiguration _configuration;

    public UserRepository(BikeDbContext bikeDbContext,IConfiguration configuration)
    {
        _bikeDbContext=bikeDbContext;
        _configuration = configuration;

    }

public async Task<bool> CreateUser(User user)
{
 
    var checkUser = await _bikeDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email || u.NIC == user.NIC || u.LicenseNumber == user.LicenseNumber);
    if (checkUser != null && checkUser.Status==Status.Accepted)
    {
        throw new Exception("User already exists!");
    }

    var data = await _bikeDbContext.Users.AddAsync(user);
    var rows=await _bikeDbContext.SaveChangesAsync();

 
        return rows > 0;

   
   
}

public async Task<bool> UserRequest(int id, int status)
{
    var data = await _bikeDbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
    if (data == null)
    {
        throw new Exception("No Such User!");
    }

    if (status == 2)
    {
        data.Status = Status.Rejected;
    }
    else if (status == 6)
    {
        data.Status = Status.Accepted;
    }

   
    //_bikeDbContext.Entry(data).Property(d => d.Status).IsModified = true;

  
    try
    {
        await _bikeDbContext.SaveChangesAsync();
        await _bikeDbContext.Entry(data).ReloadAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        throw;
    }

    var response = await SendEmail(status, data);
    return response;
}


public async Task<bool> SendEmail(int status, User user)
{
    string txtTemplate = " ";
    if (status == 2)
    {
        txtTemplate = $"{user.FirstName}\n Your Request has been rejected!";
    }
    else if (status == 6)
    {
        txtTemplate = $"Welcome {user.FirstName}\n Thank you for registering on our site.\nYour Username: {user.Email}\nYour Password: {12345678}\nPlease update your password!";
    }

    var emailMessage = new MimeMessage();
    emailMessage.From.Add(new MailboxAddress("No-Reply", "Me2@gmail.com"));
    emailMessage.To.Add(new MailboxAddress("", user.Email));
    emailMessage.Subject = "Activate Account!";
    emailMessage.Body = new TextPart("plain")
    {
        Text = txtTemplate
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

   

  
    var result = await _bikeDbContext.SaveChangesAsync();
    

    return true;
}



    public async Task<List<User>>AllUsers()
    {
      
        var data=await _bikeDbContext.Users.ToListAsync();
        return data;
    }

    public async Task <User>UserById(int Id)
    {
        var data=await _bikeDbContext.Users.FirstOrDefaultAsync(u=>u.UserId==Id);

        if(data == null)
        {
            throw new Exception("No User Found!");
        }
        return data;
    }

    public async Task <bool> DeleteById(int Id)
    {
         var data=await _bikeDbContext.Users.FirstOrDefaultAsync(u=>u.UserId==Id);

         if(data == null)
         {
            throw new Exception("No Such User!");
         }

         var deleteuser= _bikeDbContext.Users.Remove(data);
         await _bikeDbContext.SaveChangesAsync();

         if(deleteuser != null)
         {
            return true;
         }else{
            return false;
         }

    }

public async Task<bool> UpdateUser(int Id, User user)
{
    var existingUser = await _bikeDbContext.Users.FirstOrDefaultAsync(u => u.UserId == Id );

    if (existingUser == null)
    {
        throw new Exception("No such User!");
    }

 

 var checkUser=await _bikeDbContext.Users.FirstOrDefaultAsync(u=>u.Email==user.Email && u.UserId!=Id);
        if(checkUser!=null)
        {
            throw new Exception("Email already Exists!");
        }
    existingUser.FirstName = user.FirstName;
    existingUser.LastName = user.LastName;
    existingUser.Email = user.Email;
    existingUser.MobileNumber = user.MobileNumber;
    existingUser.NIC = user.NIC;
    existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
    existingUser.LicenseNumber = user.LicenseNumber;
    //
    existingUser.Role = user.Role;
    existingUser.LicenseImage = user.LicenseImage;
    existingUser.CameraCapture = user.CameraCapture;
    existingUser.Status=Status.Accepted;

_bikeDbContext.Entry(existingUser).State=EntityState.Modified;



   

    try
    {
        await _bikeDbContext.SaveChangesAsync();
        _bikeDbContext.Entry(existingUser).Reload();

       
    }
    catch (DbUpdateConcurrencyException)
    {

        throw ;
    }

    return true;
}

public async Task <string> Login(LoginRequestDTO loginrequest)
{
    var dataUser=await _bikeDbContext.Users.FirstOrDefaultAsync(u=>u.Email==loginrequest.Email && u.Status==Status.Accepted);

    if(dataUser == null)
    {
        if(loginrequest.Email=="admin123@gmail.com" && loginrequest.Password=="admin1234")
        {
             var admin = new User
            {
                UserId = new Random().Next(1, 1000000), // Unique ID for admin
                FirstName = "Super",
                LastName = "Admin",
                Email = "admin123@gmail.com",
                Role = Role.Admin,
                MobileNumber = "0765678679",
                LicenseNumber = "Acd123",
                LicenseImage = "rre",
                CameraCapture = "re",
                Status = Status.Accepted
            };

         
            var adminToken = CreateToken(admin);
        //     var data = await _bikeDbContext.RentalRequests
        // .Where(r => r.Status == Status.Pending && r.Due <= 0)
        // .ToListAsync();

    // foreach (var req in data)
    // {
        
    //     var usermail = await _bikeDbContext.Users.FirstOrDefaultAsync(u => u.UserId == req.UserId);
    //     int overdueDays = Math.Abs(req.Due);

    // double fine = req.Amount * 0.1 * overdueDays;

   
    //    req.Amount += (int)fine;
    //     if (usermail == null)
    //     {
    //         Console.WriteLine($"User not found for UserId: {req.UserId}");
    //         continue;
    //     }


    //     var emailMessage = new MimeMessage();
    //     emailMessage.From.Add(new MailboxAddress("No-Reply", "Me2@gmail.com"));
    //     emailMessage.To.Add(new MailboxAddress("", usermail.Email));
    //     emailMessage.Subject = "Late Return Notice";
    //     emailMessage.Body = new TextPart("plain")
    //     {
    //         Text = $"Dear {usermail.FirstName},\n\nYour rental request is overdue for Request ID:{req.RequestId}. Please return the bike as soon as possible. Your Final Charge :{req.Amount}"
    //     };


    //     try
    //     {
    //         using (var client = new SmtpClient())
    //         {
    //             await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
    //             await client.AuthenticateAsync("sivapakthangopikrishna69@gmail.com", "plev rbuw jsgh iipc");
    //             await client.SendAsync(emailMessage);
    //             await client.DisconnectAsync(true);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Email sending failed for UserId: {req.UserId}. Error: {ex.Message}");
    //         continue;
    //     }
   

      
    // }

   
    await _bikeDbContext.SaveChangesAsync();
            return adminToken;

        }
        throw new Exception("Invalid Email ID!");
    }

    if(!BCrypt.Net.BCrypt.Verify(loginrequest.Password,dataUser.Password))
    {
        throw new Exception("Wrong Password");
    }



var token=CreateToken(dataUser);
Console.WriteLine(token);
    return token;
}


 private string CreateToken(User user)
{
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

   
    var claims = new[]
    {
        
        new Claim("userId", user.UserId.ToString()),
        new Claim("email", user.Email),
        new Claim("role", user.Role.ToString()),
        new Claim("firstname",user.FirstName.ToString()),
        new Claim("lastname",user.LastName.ToString())
    };


    var token = new JwtSecurityToken(
        issuer: "Me2",
        audience: "Users",
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(30),
        signingCredentials: credentials
    );

  
    return new JwtSecurityTokenHandler().WriteToken(token);
}

public async Task <bool> AddEmail(Email mail)
{
    var data=await _bikeDbContext.Emails.AddAsync(mail);
    await _bikeDbContext.SaveChangesAsync();
    if(data != null)
    {
        return true;
    }else{
        return false;
    }
    
}

}
