import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'https://localhost:7000/api/User'; // Base API URL

  constructor(private http: HttpClient) {}

  
   
  // getUserById(userId: number): Observable<any> {
  //   return this.http.get<any>(`${this.apiUrl}/UserById`, {
  //     params: { Id: userId.toString() },
  //   });
  // }

 
  // updateUser(userId: number, updatedData: any): Observable<any> {
  //   return this.http.put(`${this.apiUrl}/UpdateUser`, updatedData, { responseType: 'text' } );
  // }

  
  getUserById(userId: any): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/UserById?Id=${userId}`);
  }


  updateUser(userId: number, userData: any): Observable<any> {
    return this.http.put<any>(
      `${this.apiUrl}/UpdateUser?Id=${userId}`,
      userData
    );
  }
}
