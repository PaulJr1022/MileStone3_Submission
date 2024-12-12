using System;
using BikeRentalManagement.Database;
using BikeRentalManagement.Database.Entities;
using BikeRentalManagement.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalManagement.Repository;

public class ReportRepository:IReportRepository
{
    private readonly BikeDbContext _bikeDbContext;

    public ReportRepository(BikeDbContext bikeDbContext)
    {
        _bikeDbContext=bikeDbContext;
    }
    public async  Task<int>TotalUsers()
     {
        var data=await _bikeDbContext.Users.CountAsync();
        return data;

     }

     public async Task <int>TotalBikes()
     {
        var data=await _bikeDbContext.BikeUnits.CountAsync();
        return data;
     }

     public async Task<int> TotalBooked()
     {
        var data=await _bikeDbContext.RentalRequests.Where(r=>r.Status == Status.Pending).CountAsync();
        return data;
     }

     public async  Task <int>Revenue()
     {
        var data=await _bikeDbContext.RentalRequests.Where(r=>r.Status == Status.Returned).Select(r=>r.Amount).SumAsync();
        return data;
     }

     public async Task<object>GetRevenueByMonth()
     {
      var data=await _bikeDbContext.RentalRequests.Where(r=>r.Status == Status.Returned).ToListAsync();
      
        var list =  from i in data
    group i by i.ToDate.ToString("MMM") into grp
    select new {Month = grp.Key, Revenue = grp.Sum(i => i.Amount)};

    return list;

     }
  public async Task<object> GetRevenueByBike()
{
    var data = await (
        from rental in _bikeDbContext.RentalRequests.Where(r=>r.Status == Status.Returned)
        join bike in _bikeDbContext.Bikes on rental.BikeId equals bike.BikeId
        join model in _bikeDbContext.Models on bike.ModelId equals model.ModelId
        group rental by model.ModelName into grp
        select new
        {
            ModelName = grp.Key,
            Revenue = grp.Sum(r => r.Amount)
        }
    ).ToListAsync();

    return data;
}

public async Task<object> InventoryManagement()
{
    var data = await (
        from bikeunit in _bikeDbContext.BikeUnits
        join rental in _bikeDbContext.RentalRequests
            on bikeunit.RegistrationNumber equals rental.RegistrationNumber into rentalGroup
        from rental in rentalGroup.DefaultIfEmpty()  
        select new
        {
            RegistrationNumber = bikeunit.RegistrationNumber,
            Status =  rental.Status == Status.Pending ? "Pending" : "Available"  
        }
    ).ToListAsync();

    return data;
}
 
public async Task<ICollection<object>> UserHistory()
{
    var users = await _bikeDbContext.Users.ToListAsync();
    
    var result = new List<object>();

    foreach (var user in users)
    {
        
        var requests = await _bikeDbContext.RentalRequests
            .Where(r => r.UserId == user.UserId)
            .ToListAsync();

        var totalReq = requests.Count();
        var totalReturn = requests.Where(r => r.Status == Status.Returned).Count();
        var totalPending = requests.Where(r => r.Status == Status.Pending).Count();
        var totalLateReturn = requests.Where(r => r.Due < 0).Count();

        result.Add(new 
        {
            UserId = user.UserId,
            UserName = user.FirstName,  
            TotalRequests = totalReq,
            TotalReturned = totalReturn,
            TotalPending = totalPending,
            TotalLateReturns = totalLateReturn
        });
    }

    return result;
}

   
}
