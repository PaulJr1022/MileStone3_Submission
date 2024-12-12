import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserRegisterService {

  constructor(private http:HttpClient) { }

  addUser(userRegister:IUserRegister){
    return this.http.post('https://localhost:7000/api/User/CreateUser', userRegister);
  }
}

export interface IUserRegister{
  firstName : string;
  lastName : string;
  email : string;
  phoneNo : number;
  nic : string;
  licenceNo : string;
  licencePhoto : File;
  cameraPic : File,
  role : any

}

export enum Role {
  Admin =1,
  StandardUser
}
