import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RentalRequestService {

  private apiUrl= 'https://localhost:7000/api/Rent/AllRequest'; //endpoint url
  private rentUrl= 'https://localhost:7000/api/Rent'; //rent base url

 

  constructor(private http: HttpClient) {}

  getAllRequests(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);

  }

  declineRequest(requestId: number): Observable<any> {
    return this.http.post(`${this.rentUrl}/CancelRequest${requestId}`, {});
  }

  getRentalHistoryByUser(id: number): Observable<any[]> {
    return this.http.get<any[]>(`https://localhost:7000/api/Rent/RentalHistoryCountByUser${id}`);
  }

  updateRequestStatus(requestId: number, status: number): Observable<any> {
    const url = `${this.rentUrl}/AcceptRejectRequest${requestId}?status=${status}`;
    return this.http.put(url, requestId)
  }
  
}
