using System;

namespace BikeRentalManagement.Database.Entities;

public enum Status
{
    Waiting=1,
    Rejected,
    Pending,
    Returned,

    Cancelled,

    Accepted

}
