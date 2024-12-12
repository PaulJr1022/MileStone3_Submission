using System;
using Quartz;
using BikeRentalManagement.Service;

namespace BikeRentalManagement;

public class ApiJob:IJob
{
     
        private readonly ApiService _apiService;

        public ApiJob(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            await _apiService.CallApis();
        }
    

}
