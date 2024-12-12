using System;
using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.DTOs.RequestDTOs;
using BikeRentalManagement.DTOs.ResponseDTOs;

namespace BikeRentalManagement.IService;

public interface IUserService
{
Task <bool> CreateUser(UserRequestDTO userRequestDTO);
Task<List<AllUserResponseDTO>>AllUsers();
Task <UserResponseDTO>UserById(int Id);
Task <bool> DeleteById(int Id);

Task <bool> UpdateUser(int Id,UserUpdateRequestDTO userupdate);


Task <string> Login(LoginRequestDTO loginrequest);
Task <bool> AddEmail(Email mail);

Task<bool>UserRequest(int id,int status);
}
