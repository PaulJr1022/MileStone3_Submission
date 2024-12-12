import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class UserRequestService {

  private userRequestUrl = 'https://localhost:7000/api/User/UserRequest';

  constructor(private http:HttpClient) { }

  getUser():Observable<any[]>{
    return this.http.get<any[]>('https://localhost:7000/api/User/AllUsers');
  }

  // updateUserRequestStatus(id: string): any {
  //   const url = `${this.userRequestUrl}${id}?status=${status}`;
  //  // return this.http.put(url, {}); 
  //  return url;
  // }

  updateUserRequestStatus(userId: string, status: number): Observable<any> {
    const url = `${this.userRequestUrl}${userId}?status=${status}`;
    console.log('API URL:', url); // Debug: Check the constructed URL
    return this.http.put<any>(url, {}); // Ensure the HTTP method and response type are correct
  }

  // login(userId : number){
  //   return this.http.post('https://localhost:7000/api/User/Login', userId)
  // }

  UserLogin(login:any): Observable<any>{
    return this.http.post('https://localhost:7000/api/User/Login', login,{
      responseType:'text'
    });
  }

  isLoggedIn(){
    // if (localStorage.getItem("token")) {
    //   const token = localStorage.getItem("token");
    //   if (token) {
    //     const decoded:any = jwtDecode(token);
    //     console.log(decoded);
        
    //     localStorage.setItem("name", decoded.FullName)
    //     localStorage.setItem("Role", decoded.Roles)
    //   }
    //   return true;
    // }else{
    //   return false;
    // }
    if(localStorage.getItem("token")){
      return true;
    } else {
      return false;
    }
  }

}
