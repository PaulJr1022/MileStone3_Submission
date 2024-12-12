using System;
using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.DTOs.RequestDTOs;
using BikeRentalManagement.DTOs.ResponseDTOs;

namespace BikeRentalManagement.IRepository;

public interface IRentRepository
{
    Task<List<BookedDatesResponseDTO>> GetBikeBookedDates(string registrationNumber);
    Task<bool> RequestRent(RentalRequest rentRequest);
   Task <bool> CheckRequest(string RegistrationNumber,DateTime FromDate,DateTime ToDate);
   Task<bool> CheckRequest(int id,string RegistrationNumber,DateTime FromDate,DateTime ToDate);
    Task <List<RentalRequest>> AllRequest();
    Task <RentalRequest> GetRequestById(int id);

     Task<bool> AcceptRejectRequest(int id,int status);
     Task<bool> CancelRequest(int id);

      Task <bool> UpdateRequest(int id,RentalRequest rentRequest);

      Task <List<RentalResponseDTO>> RequestByUser(int id);

      Task <ICollection<object>> CountHistory(int id);

      Task <bool> LateReturns();
      Task<List<RentalRequest>> Reminder();

      Task <string> GetPhoneNumber(int id);
      Task<string> GetEmail(int id);
}
