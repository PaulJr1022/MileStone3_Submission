import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { jwtDecode } from 'jwt-decode';


declare const google: any;

@Component({
  selector: 'app-user-rental-request',
  templateUrl: './user-rental-request.component.html',
  styleUrls: ['./user-rental-request.component.css'],
})
export class UserRentalRequestComponent implements OnInit, AfterViewInit {
  requests: any[] = [];
  selectedRequest: any = null;

  updateForm!: FormGroup;

  bikeId!: number;
  userId!: string;
  bikeImage: string = '';
  registrationNumber: string = '';
  rentPerDay: number = 0;
  distance: number | null = null;
  daysDifference: number | null = null;
  amount: number | null = null;

  map: any;
  directionsService = new google.maps.DirectionsService();
  directionsRenderer = new google.maps.DirectionsRenderer();
  showmap = false;

  constructor(
    private http: HttpClient,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    const decodedToken: CustomJwtPayload = jwtDecode<CustomJwtPayload>(token!);
    this.userId = decodedToken.userId;

    const apiUrl = `https://localhost:7000/api/Rent/RequestByUser${this.userId}`;
    this.http.get<any[]>(apiUrl).subscribe({
      next: (data) => (this.requests = data),
      error: (error) => console.error('Error fetching requests:', error),
    });

    this.updateForm = new FormGroup({
      userId: new FormControl({ value: '0', disabled: true }, [Validators.required]),
      bikeId: new FormControl({ value: '', disabled: true }, [Validators.required]),
      registrationNumber: new FormControl({ value: '', disabled: true }, [Validators.required]),
      fromDate: new FormControl('', [Validators.required]),
      toDate: new FormControl('', [Validators.required]),
      fromLocation: new FormControl('', [Validators.required]),
      toLocation: new FormControl('', [Validators.required]),
      distance: new FormControl('', [Validators.required]),
      due: new FormControl({ value: '', disabled: true }),
      amount: new FormControl({ value: '', disabled: true }, [Validators.required]),
    });
  }

  ngAfterViewInit(): void {
    this.initMap();
  }

  updateEndDateMin() {
    const fromDate = this.updateForm.get('fromDate')?.value;
    if (fromDate) {
      this.updateForm.get('toDate')?.setValue('');
    }
  }

  initMap() {
    this.map = new google.maps.Map(document.getElementById('map') as HTMLElement, {
      center: { lat: 0, lng: 0 },
      zoom: 2,
    });
    this.directionsRenderer.setMap(this.map);
  }

  calculateDistance() {
    const fromLocation = this.updateForm.get('fromLocation')?.value;
    const toLocation = this.updateForm.get('toLocation')?.value;
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
        this.updateForm.patchValue({ distance: this.distance });
      } else {
        this.toastr.error('Could not calculate the distance. Please check your inputs.');
      }
    });
  }

  onCancel(request: any): void {
    const cancelApiUrl = `https://localhost:7000/api/Rent/CancelRequest${request.requestId}`;
    this.http.put(cancelApiUrl, {}).subscribe({
      next: () => {
        this.toastr.info('Cancel request successful');
        request.status = 'Cancelled';
      },
      error: (error) => console.error('Error cancelling request:', error),
    });
  }

  calculateAmount() {
    const fromDate = new Date(this.updateForm.get('fromDate')?.value);
    const toDate = new Date(this.updateForm.get('toDate')?.value);

    if (!fromDate || isNaN(fromDate.getTime()) || !toDate || isNaN(toDate.getTime())) {
      this.toastr.error('Please select valid From Date and To Date.');
      return;
    }

    const diffTime = Math.abs(toDate.getTime() - fromDate.getTime());
    this.daysDifference = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    if (this.rentPerDay <= 0) {
      this.toastr.error('Rent per day is not set or invalid.');
      return;
    }

    this.amount = this.daysDifference * this.rentPerDay;
    this.updateForm.patchValue({ amount: this.amount });
    //alert(`Calculated amount: ${this.amount}`);
  }

  onUpdate(request: any): void {
    this.selectedRequest = request;
    this.updateForm.patchValue(request);
    this.getRentPerDay(request.registrationNumber);
  }

  getRentPerDay(registrationNumber: string): void {
    const bikeApiUrl = `https://localhost:7000/api/Bike/GetByRegistrationNumber?RegNo=${registrationNumber}`;
    this.http.get<any>(bikeApiUrl).subscribe({
      next: (data) => {
        if (data && data.bikeUnits && data.bikeUnits.length > 0) {
          this.rentPerDay = data.bikeUnits[0].rentPerDay;
          console.log(`Rent per day for ${registrationNumber}: ${this.rentPerDay}`);
        } else {
          this.toastr.error('Bike data not found.');
        }
      },
      error: (error) => {
        console.error('Error fetching rent per day:', error);
      },
    });
  }

  onSubmitUpdateForm(): void {
    if (this.updateForm.valid) {
      const updatedData = this.updateForm.getRawValue();
      const updateApiUrl = `https://localhost:7000/api/Rent/UpdateRequest${this.selectedRequest.requestId}`;

      this.http.put(updateApiUrl, updatedData).subscribe({
        next: () => {
          this.toastr.success('Request updated successfully');
          const index = this.requests.findIndex((r) => r.requestId === this.selectedRequest.requestId);
          if (index !== -1) {
            this.requests[index] = { ...updatedData, requestId: this.selectedRequest.requestId };
          }
          this.selectedRequest = null;
        },
        error: (error) => console.error('Error updating request:', error),
      });
    }
  }

  getToday(): any{
    return new Date().toISOString().split('T')[0];
}
}

interface CustomJwtPayload {
  userId: string;
}
