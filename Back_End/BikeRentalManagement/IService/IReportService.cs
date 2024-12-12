using System;

namespace BikeRentalManagement.IService;

public interface IReportService
{
    Task<int>TotalUsers();
    Task <int>TotalBikes();

    Task <int>TotalBooked();

    Task <int>Revenue();

    Task<object>GetRevenueByMonth();
    Task<object>GetRevenueByBike();

    Task<object>InventoryManagement();
    Task <ICollection<object>>UserHistory();

}
