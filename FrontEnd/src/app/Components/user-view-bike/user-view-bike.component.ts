import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { BikesService } from '../Services/bikes.service';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

declare const google: any;

@Component({
  selector: 'app-user-view-bike',
  templateUrl: './user-view-bike.component.html',
  styleUrls: ['./user-view-bike.component.css']
})
export class UserViewBikeComponent implements OnInit, AfterViewInit {
  rentalForm!: FormGroup;
  bikeId!: number;
  userId!: string;
  bikeImage: string = '';
  registrationNumber: string = '';
  rentPerDay: number = 0;
  distance: number | null = null;
  daysDifference: number | null = null;
  map: any;
  directionsService = new google.maps.DirectionsService();
  directionsRenderer = new google.maps.DirectionsRenderer();
  showmap = false;

  bookedDates: { fromDate: string, toDate: string }[] = [];



  constructor(private fb: FormBuilder, private toastr: ToastrService, private rentService: BikesService, private router: Router, private http: HttpClient) { }

  ngOnInit(): void {
 
    this.rentalForm = this.fb.group({
      fromDate: ['', Validators.required],
      toDate: ['', Validators.required],
      fromLocation: ['',],
      toLocation: ['',],
      ridingOption: ['alone']
    });

    // Fetch user and bike details
    const token = localStorage.getItem('token');
    if (token) {
      const decodedToken: CustomJwtPayload = jwtDecode<CustomJwtPayload>(token);
      this.userId = decodedToken.userId;
    }

    const state = history.state;
    if (state && state.bike) {
      const bike = state.bike;
      this.bikeId = bike.bikeId;
      this.registrationNumber = bike.registrationNumber;
      this.rentPerDay = bike.rentPerDay;
      if (bike.bikeImages && bike.bikeImages.length > 0) {
        this.bikeImage = 'https://localhost:7000/' + bike.bikeImages[0].image;
      }
    }
  }

  updateEndDateMin() {
    const fromDate = this.rentalForm.get('fromDate')?.value;
    if (fromDate) {
      this.rentalForm.get('toDate')?.setValue('');
    }
  }
  
  isFormValidForCalculation(): boolean {
    const fromDate = this.rentalForm.get('fromDate')?.value;
    const toDate = this.rentalForm.get('toDate')?.value;
    return (
      !!fromDate &&
      !!toDate &&
      new Date(toDate) > new Date(fromDate) &&
      !!this.rentalForm.get('fromLocation')?.value &&
      !!this.rentalForm.get('toLocation')?.value
    );
  }

  getBookedDates() {
    if (!this.registrationNumber) {
      //alert(2)
      return;
    }


   this.http
  .get<any>(`https://localhost:7000/api/Rent/BikeBookedDates?RegistrationNumber=${this.registrationNumber}`)
  .subscribe({
    next: (response) => {
 
      this.bookedDates = [];


      if (response && Array.isArray(response) && response.length > 0) {
        response.forEach((entry) => {
          if (entry.dates && entry.dates.length === 2) {
            try {
 
              const fromDate = new Date(entry.dates[0]);
              const toDate = new Date(entry.dates[1]);


              const localFromDate = new Date(fromDate.getTime() - fromDate.getTimezoneOffset() * 60000);
              const localToDate = new Date(toDate.getTime() - toDate.getTimezoneOffset() * 60000);


              if (localToDate > localFromDate) {
 
                this.bookedDates.push({
                  fromDate: localFromDate.toISOString().split('T')[0],
                  toDate: localToDate.toISOString().split('T')[0],
                });

                console.log('Processed Dates:', {
                  originalFromDate: entry.dates[0],
                  originalToDate: entry.dates[1],
                  localFromDate: localFromDate.toISOString().split('T')[0],
                  localToDate: localToDate.toISOString().split('T')[0],
                });
              } else {
                console.log('Invalid date range: toDate must be greater than fromDate', entry.dates);
              }
            } catch (err) {
              console.log(`Error processing dates for entry: ${JSON.stringify(entry)}`, err);
            }
          } else {
            console.log(`Invalid date format or missing dates for entry: ${JSON.stringify(entry)}`);
          }
        });
      } else {
        console.log('No data found in response.');
      }
    },
    error: (error) => {
 
      console.error('Error fetching bike booked dates:', error);


      this.bookedDates = [];
    },
    complete: () => {
      console.log('HTTP request completed.');
    },
  });


  }

  ngAfterViewInit(): void {
    this.initMap();
  }

  initMap() {
    this.map = new google.maps.Map(document.getElementById('map') as HTMLElement, {
      center: { lat: 0, lng: 0 },
      zoom: 2,
    });
    this.directionsRenderer.setMap(this.map);
  }

  calculateDistance() {
    const fromLocation = this.rentalForm.get('fromLocation')?.value;
    const toLocation = this.rentalForm.get('toLocation')?.value;
    this.showmap = true;

    if (!fromLocation || !toLocation) {
      this.toastr.error('Please enter both locations');
      return;
    }

    const request = {
      origin: fromLocation,
      destination: toLocation,
      travelMode: google.maps.TravelMode.DRIVING,
    };

    this.directionsService.route(request, (result: any, status: any) => {
      if (status === google.maps.DirectionsStatus.OK) {
        this.directionsRenderer.setDirections(result);
        const route = result.routes[0];
        const distanceInKm = route.legs[0].distance.value / 1000; // Distance in kilometers
        this.distance = Math.round(distanceInKm);
        // const distance = route.legs[0].distance.value / 1000; // Convert to kilometers
        // this.distance = Math.round(distance * 100) / 100; // Round to 2 decimal places

        // Calculate number of rental days
        const fromDate = new Date(this.rentalForm.get('fromDate')?.value);
        const toDate = new Date(this.rentalForm.get('toDate')?.value);
        if (fromDate && toDate) {
          const diffTime = Math.abs(toDate.getTime() - fromDate.getTime());
          this.daysDifference = Math.ceil(diffTime / (1000 * 60 * 60 * 24)); 
        }
      } else {
        this.toastr.error('Could not calculate the distance. Please check your inputs.');
      }
    });
  }

  calculateAmount(): number {
    return (this.daysDifference || 0) * this.rentPerDay;
  }

  submitRequest() {
    if (this.rentalForm.valid && this.distance !== null) {


      const rentalData = {
        userId: this.userId.toString(), 
        bikeId: this.bikeId, 
        registrationNumber: this.registrationNumber,
        fromDate: new Date(this.rentalForm.get('fromDate')?.value).toISOString().split('T')[0], 
        toDate: new Date(this.rentalForm.get('toDate')?.value).toISOString().split('T')[0], 
        fromLocation: this.rentalForm.get('fromLocation')?.value,
        toLocation: this.rentalForm.get('toLocation')?.value,
        distance: this.distance, 
        amount: this.calculateAmount(), 
        due: 0, 
        status: 1 
      };
      console.log(rentalData);

      this.rentService.submitRentalRequest(rentalData).subscribe({
        next: (response) => {
          this.toastr.success('Rental request submitted successfully!');
          this.router.navigateByUrl('/user/userRental');
          console.log(response);
        },
        error: (err) => {
          console.error('Error:', err);
          this.toastr.error(
            'Failed to submit rental request. ' +
            (err.error?.title || 'This Bike Already Booked in this date.')
          );
        }
      });
    } else {
      this.toastr.error('Please fill all required fields and calculate distance.');
    }
  }

  getToday(): any{
      return new Date().toISOString().split('T')[0];
  }
}

interface CustomJwtPayload {
  userId: string;
}
