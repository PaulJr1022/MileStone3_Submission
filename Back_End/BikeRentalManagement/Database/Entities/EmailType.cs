using System;

namespace BikeRentalManagement.Database.Entities;

public enum EmailType
{
   UserPassword=1,
   BookingConfirmation,
   LateRentalAlert,
   ReturnConfirmation

}
