import { Component } from '@angular/core';
import { BikesService } from '../Services/bikes.service';
import { Router } from '@angular/router';
import { UserViewBikeComponent } from '../user-view-bike/user-view-bike.component';

@Component({
  selector: 'app-user-rental-bike',
  templateUrl: './user-rental-bike.component.html',
  styleUrl: './user-rental-bike.component.css'
})
export class UserRentalBikeComponent {
  bikes: Bike[] = [];
  
 searchBrand:string='';
 searchRent: number| null=null;
 activeTab:string='brand';


  constructor(private bikeService: BikesService, private router:Router, ) {}

  ngOnInit(): void {
      this.getBikes();
  }

  getBikes(){
    this.bikeService.getAllBikes().subscribe({
      next: (data: Bike[]) => {
        this.bikes = data;
       // console.log(this.bikes);
      },
      error: (err) => {
        console.error('Error fetching bikes:', err);
      }
    });
  }


  viewBike(registrationNumber: string): void {
    const selectedBike = this.bikes
      .flatMap((bike) => bike.bikeUnits)
      .find((unit) => unit.registrationNumber === registrationNumber);

    if (selectedBike) {
      this.router.navigate(['/user/viewBike'], {
        state: { bike: selectedBike }
      });
    }
  }

}

export interface BikeImage {
  imageId: number;
  unitId: number;
  image: string;
}

export interface BikeUnit {
  bikeId: number;
  unitId: number;
  registrationNumber: string;
  rentPerDay: number;
  year: number;
  bikeImages: BikeImage[];
}

export interface Bike {
  brandName: string;
  modelName: string;
  bikeUnits: BikeUnit[];
}
