using System;
using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.DTOs.RequestDTOs;
using BikeRentalManagement.DTOs.ResponseDTOs;

namespace BikeRentalManagement.IService;

public interface IRentService
{
    Task<bool> RequestRent(RentRequestDTO rentRequestDTO);
    Task<List<BookedDatesResponseDTO>> GetBikeBookedDates(string RegistrationNumber);
   Task <List<AllRentalResponseDTO>> AllRequest();
   Task <RentalRequest> GetRequestById(int id);

   Task<bool> AcceptRejectRequest(int id,int status);
   Task<bool>CancelRequest(int id);

  Task <bool> UpdateRequest(int id,RentRequestDTO rentRequestDTO);
  Task <List<RentalResponseDTO>> RequestByUser(int id);


  Task <ICollection<object>> CountHistory(int id);

  Task <bool> LateReturns();
 Task<List<RentalRequest>>  Reminder();
    

}
