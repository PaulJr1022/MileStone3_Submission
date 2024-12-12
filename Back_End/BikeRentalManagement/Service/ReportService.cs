using System;
using BikeRentalManagement.IRepository;
using BikeRentalManagement.IService;

namespace BikeRentalManagement.Service;

public class ReportService:IReportService
{
    private readonly IReportRepository _reportRepository;

    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository=reportRepository;
    }

  public async  Task<int>TotalUsers()
  {
    var data=await _reportRepository.TotalUsers();
    if(data == null)
     {
         return 0;
     }
    return data;
  }

  public async  Task <int>TotalBikes()
  {
     var data=await _reportRepository.TotalBikes();

     if(data == null)
     {
         return 0;
     }
     return data;
  }

  public async  Task <int>TotalBooked()
  {
    var data=await _reportRepository.TotalBooked();

    if(data == null)
     {
         return 0;
     }
     return data;
  }

public async   Task <int>Revenue()
{
    var data=await _reportRepository.Revenue();

      if(data == null)
     {
         return 0;
     }
     return data;
}

public async  Task<object>GetRevenueByMonth()
{
  var data=await _reportRepository.GetRevenueByMonth();
   if(data == null)
   {
    return null;
   }
   return data;
}

public async  Task<object>GetRevenueByBike()
{
  var data=await _reportRepository.GetRevenueByBike();
  if(data == null)
  {
    return null;
  }
  return data;
}
public async Task<object>InventoryManagement()
{
  var data=await _reportRepository.InventoryManagement();
    if(data == null)
  {
    return null;
  }
  return data;
}

public async  Task <ICollection<object>>UserHistory()
{
   var data=await _reportRepository.UserHistory();
    if(data == null)
  {
    return null;
  }
  return data;

}

}
