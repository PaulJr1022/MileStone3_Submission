using System;
using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.DTOs.RequestDTOs;
using BikeRentalManagement.DTOs.ResponseDTOs;
using BikeRentalManagement.IRepository;
using BikeRentalManagement.IService;
using Microsoft.AspNetCore.Http.HttpResults;


namespace BikeRentalManagement.Service;

public class UserService:IUserService
{

 private readonly  IUserRepository _userRepository;

public UserService(IUserRepository userRepository)
{
    _userRepository=userRepository;
}
  public async Task<bool> CreateUser(UserRequestDTO userRequestDTO)
{
    try
    {
       
        var imageDirectory = Path.Combine("wwwroot", $"images");
        if (!Directory.Exists(imageDirectory))
        {
            Directory.CreateDirectory(imageDirectory);
        }

      
        var imagePath = Path.Combine(imageDirectory, userRequestDTO.LicenseImage.FileName);
        if (userRequestDTO.LicenseImage != null && userRequestDTO.LicenseImage.Length > 0)
        {
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await userRequestDTO.LicenseImage.CopyToAsync(stream);
            }
        }

       
        var imagePathCam = Path.Combine(imageDirectory, userRequestDTO.CameraCapture.FileName);
        if (userRequestDTO.CameraCapture != null && userRequestDTO.CameraCapture.Length > 0)
        {
            using (var stream = new FileStream(imagePathCam, FileMode.Create))
            {
                await userRequestDTO.CameraCapture.CopyToAsync(stream);
            }
        }

     
        var user = new User
        {
            FirstName = userRequestDTO.FirstName,
            LastName = userRequestDTO.LastName,
            Email = userRequestDTO.Email,
            MobileNumber = userRequestDTO.MobileNumber,
            NIC = userRequestDTO.NIC,
            Password = "12345678",
            LicenseNumber = userRequestDTO.LicenseNumber,
            Role = userRequestDTO.Role,
            LicenseImage = imagePath,
            CameraCapture = imagePathCam,
            Status = userRequestDTO.Status
        };

        
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

     
        var data = await _userRepository.CreateUser(user);
        if(data == true )
        {
            return true;
        }else{
            return false;
        }
    }
    catch (Exception ex)
    {
   
        Console.WriteLine(ex.Message);
        throw;
    }
}


    public async Task<bool>UserRequest(int id,int status)
    {
        var data=await _userRepository.UserRequest(id,status);
        if(data)
        {
            return true;
        }else{
            return false;
        }
    }

    public async Task<List<AllUserResponseDTO>>AllUsers()
    {
        var data=await _userRepository.AllUsers();
        var response=new List<AllUserResponseDTO>();

        if(data == null)
        {
            throw new Exception("No Data Exists!");
        }else{
            foreach(var d in data)
            {
                d.LicenseImage=d.LicenseImage?.Replace("wwwroot","");
                d.LicenseImage=d.LicenseImage?.Replace("\\","/");
                d.CameraCapture=d.CameraCapture?.Replace("wwwroot","");
                d.CameraCapture=d.CameraCapture?.Replace("\\","/");
             
                
                  var  AllUserResponse=new AllUserResponseDTO
            {
                FirstName=d.FirstName,
               LastName=d.LastName,
                Email=d.Email,
                LicenseNumber=d.LicenseNumber,
               MobileNumber=d.MobileNumber,
               UserId=d.UserId,
               LicenseImage=d.LicenseImage,
                CameraCapture=d.CameraCapture,
               Status=d.Status.ToString()

            };
            response.Add(AllUserResponse);

            }

        
            return response;
        }
    }
public async Task<UserResponseDTO> UserById(int Id)
{
    try
    {
       
        var user = await _userRepository.UserById(Id);

        if (user == null)
        {
            return null; 
        }

        var response = new UserResponseDTO
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            MobileNumber = user.MobileNumber,
            NIC = user.NIC,
            LicenseNumber = user.LicenseNumber,
            Password=user.Password,
            LicenseImage = user.LicenseImage.Replace("wwwroot\\","/").Replace("\\","/"),
            CameraCapture = user.CameraCapture.Replace("wwwroot\\","/").Replace("\\","/")
        };

        return response;
    }
    catch (Exception ex)
    {
        
        Console.WriteLine(ex.Message);
        return null;
    }
}


    public async Task <bool> DeleteById(int Id)
    {
        var data=await _userRepository.DeleteById(Id);

       return data;
    }

   public async Task<bool> UpdateUser(int Id,UserUpdateRequestDTO userupdate)
{
    try
    {
        
        var existingUser = await _userRepository.UserById(Id);
        if (existingUser == null)
        {
            return false; 
        }

        // var imageDirectory = Path.Combine("wwwroot", "images");
        // if (!Directory.Exists(imageDirectory))
        // {
        //     Directory.CreateDirectory(imageDirectory);
        // }

        // string ?imagePath = existingUser.LicenseImage;
        // string? imagePathCam = existingUser.CameraCapture;

    
        // if (userRequestDTO.LicenseImage != null && userRequestDTO.LicenseImage.Length > 0)
        // {
        //     imagePath = Path.Combine(imageDirectory, userRequestDTO.LicenseImage.FileName);
        //     using (var stream = new FileStream(imagePath, FileMode.Create))
        //     {
        //         await userRequestDTO.LicenseImage.CopyToAsync(stream);
        //     }
        // }

      
        // if (userRequestDTO.CameraCapture != null && userRequestDTO.CameraCapture.Length > 0)
        // {
        //     imagePathCam = Path.Combine(imageDirectory, userRequestDTO.CameraCapture.FileName);
        //     using (var stream = new FileStream(imagePathCam, FileMode.Create))
        //     {
        //         await userRequestDTO.CameraCapture.CopyToAsync(stream);
        //     }
        // }

     
        var user = new User
        {
           // UserId = Id,
            FirstName = userupdate.FirstName ?? existingUser.FirstName,
            LastName = userupdate.LastName ?? existingUser.LastName,
            Email = userupdate.Email ?? existingUser.Email,
            MobileNumber = userupdate.MobileNumber ?? existingUser.MobileNumber,
            NIC = userupdate.NIC ?? existingUser.NIC,
            Password = userupdate.Password??existingUser.Password,
            LicenseNumber = userupdate.LicenseNumber ?? existingUser.LicenseNumber,
            Role = existingUser.Role,
            LicenseImage = existingUser.LicenseImage,
            CameraCapture = existingUser.CameraCapture,
            Status =  existingUser.Status
        };

      
        var data = await _userRepository.UpdateUser(Id, user);
        return data;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return false;
    }
}

    public async Task <string> Login(LoginRequestDTO loginrequest)
    {

        var data=await _userRepository.Login(loginrequest);

       return data;

    }

    public async Task <bool> AddEmail(Email mail)
    {
        var data=await _userRepository.AddEmail(mail);
        if(data)
        {
            return true;
        }else{
            return false;
        }
    }

}
