import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserReportService {

  constructor(private http : HttpClient) { }
   
  baseUrl1 = 'https://localhost:7000/api/Rent/RentalHistoryCountByUser';
  private baseUrl = 'https://localhost:7000/api/Report';
  private apiUrl = 'https://localhost:7000/api/Report/InventoryManagement';
   apiUrlReport = 'https://localhost:7000/api/Report/UserHistory';

  private userUrl='https://localhost:7000/api/Report/TotalUsers';
  private bikeUrl='https://localhost:7000/api/Report/TotalBikes';


  userReport(id: any): Observable<any> {
    const url = `${this.baseUrl1}${id}`;
    return this.http.get<any>(url);  // Get request for fetching data
  }
  getTotalUsers(): Observable<number> {
    return this.http.get<number>(`${this.userUrl}`);
  }

  getTotalBikes(): Observable<number> {
    return this.http.get<number>(`${this.bikeUrl}`);
  }

  getBookedBikes(): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}/BookedBikes`);
  }

  getRevenue(): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}/Revenue`);
  }

  getRevenueByMonth(): Observable<{ month: string; count: number }[]> {
    return this.http.get<{ month: string; count: number }[]>(`${this.baseUrl}/GetRevenueByMonth`);
  }
  
  getRevenueByBike(): Observable<{ modelName: string; revenue: number }[]> {
    return this.http.get<{ modelName: string; revenue: number }[]>(`${this.baseUrl}/GetRevenueByBike`);
  }

  getInventoryManagement(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }

  getUserHistory(): Observable<any> {
    return this.http.get<any>(this.apiUrlReport);
  }

  
 


}
