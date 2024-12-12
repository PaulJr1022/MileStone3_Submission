import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Bike } from '../home/home.component';

@Injectable({
  providedIn: 'root'
})
export class BikesService {

  private apiUrl = 'https://localhost:7000/api/Bike/AllBikes';

  private bikeUrl = 'https://localhost:7000/api/Bike';

  private rentBike = 'https://localhost:7000/api/Rent'
  private addBikeUrl = 'https://localhost:7000/api/Bike/AddBike';
  private uploadImageUrl = 'https://localhost:7000/api/Bike/UploadImages';
  

  constructor(private http: HttpClient) {}

  

  getAllBikes(): Observable<Bike[]> {
    return this.http.get<Bike[]>(this.apiUrl);
  }

  addBike(bikeData: any): Observable<any> {
    return this.http.post(this.addBikeUrl, bikeData);
  }

  uploadImage(unitId: number, imageFile: File): Observable<any> {
    const formData = new FormData();
    formData.append('UnitId', unitId.toString());
    formData.append('Image', imageFile, imageFile.name);
    return this.http.post(this.uploadImageUrl, formData);
  }

  getBikeByRegistrationNumber(regNo: string): Observable<Bike> {
    const url = `https://localhost:7000/api/Bike/GetByRegistrationNumber?RegNo=${regNo}`;
    return this.http.get<Bike>(url);
    
  }

  getBikeByRegistrationNumber1(registrationNumber: string): Observable<any> {
    return this.http.get(`${this.bikeUrl}/GetByRegistrationNumber?RegNo=${registrationNumber}`);
  }


   updateBike( bikeData: any): Observable<any> {
    return this.http.put(`${this.bikeUrl}/UpdateBike`, bikeData);
  }


  deleteBike(registrationNumber: string): Observable<any> {
    return this.http.delete(`${this.bikeUrl}/DeleteBike${registrationNumber}`);
  }


  // submitRentalRequest(data: any): Observable<any> {
  //   return this.http.post(`https://localhost:7000/api/Rent/RequestRent`, data);
  // }

  submitRentalRequest(rentalData: any): Observable<any> {
    const headers = { 'Content-Type': 'application/json' };
    return this.http.post(`${this.rentBike}/RequestRent`, rentalData, { headers });
  }

}
