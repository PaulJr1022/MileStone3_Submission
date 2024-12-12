import { Component } from '@angular/core';
import { BikesService } from '../Services/bikes.service';
import { SearchPipe } from "../Pipes/search.pipe";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',

})
export class HomeComponent {
  bikes: Bike[] = [];
  searchBrand:string='';
 searchRent: number| null=null;
 activeTab:string='brand';

  constructor(private bikeService: BikesService) {}

  ngOnInit(): void {
    this.bikeService.getAllBikes().subscribe({
      next: (data: Bike[]) => {
        this.bikes = data;
      },
      error: (err) => {
        console.error('Error fetching bikes:', err);
      }
    });
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

