import { Component, OnInit } from '@angular/core';
import { BikesService } from '../Services/bikes.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as bootstrap from 'bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-update-bike',
  templateUrl: './admin-update-bike.component.html',
  styleUrls: ['./admin-update-bike.component.css'],
})
export class AdminUpdateBikeComponent implements OnInit {
  bikes: Bike[] = [];
  selectedUnit: BikeUnit | null = null;
  updateForm: FormGroup;

  constructor(private bikeService: BikesService, private fb: FormBuilder, private toastr : ToastrService) {
    this.updateForm = this.fb.group({
      unitId: [], // Hidden field for unitId
      registrationNumber: [''], // Hidden field for registration number
      brandName: [{ value: '', disabled: true }, Validators.required],
      modelName: [{ value: '', disabled: true }, Validators.required],
      year: ['', [Validators.required]],
      rentPerDay: ['', [Validators.required]],
      bikeImage: [null], // For file upload
    });
  }

  ngOnInit(): void {
    this.loadBikes();
  }

  loadBikes(): void {
    this.bikeService.getAllBikes().subscribe({
      next: (data: Bike[]) => {
        this.bikes = data;
      },
      error: (err) => {
        console.error('Error fetching bikes:', err);
      },
    });
  }

  openUpdateModal(unit: BikeUnit, bike: Bike): void {
    this.selectedUnit = unit;

    // Bind existing values to the form
    this.updateForm.patchValue({
      unitId: unit.unitId,
      registrationNumber: unit.registrationNumber,
      brandName: bike.brandName,
      modelName: bike.modelName,
      year: unit.year,
      rentPerDay: unit.rentPerDay,
    });

    // Open the modal (Bootstrap)
    const modal = document.getElementById('exampleModalToggle');
    if (modal) {
      const modalInstance = new bootstrap.Modal(modal);
      modalInstance.show();
    }
  }

  onFileChange(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0] || null;
    this.updateForm.patchValue({ bikeImage: file });
  }

  updateBike(): void {
    if (this.updateForm.invalid || !this.selectedUnit) {
      console.log('Form is invalid:', this.updateForm);
      return;
    }

    const formData = new FormData();
    formData.append('unitId', this.updateForm.get('unitId')?.value);
    formData.append(
      'registrationNumber',
      this.updateForm.get('registrationNumber')?.value
    );
    formData.append('brandName', this.updateForm.get('brandName')?.value);
    formData.append('modelName', this.updateForm.get('modelName')?.value);
    formData.append('year', this.updateForm.get('year')?.value);
    formData.append('rentPerDay', this.updateForm.get('rentPerDay')?.value);

    const file = this.updateForm.get('bikeImage')?.value;
    
    
    if (file) {
      formData.append('bikeImages', file); // Append the file only if it exists
      console.log(file);
      
      
      
    }

    this.bikeService.updateBike(formData).subscribe({
      next: () => {
        this.toastr.success('Bike details updated successfully!');
        this.loadBikes(); // Refresh the data
        this.closeModal();
      
        
        
        
      },
      error: (err) => {
        this.toastr.error('Error updating bike details!');
        console.error('Error:', err);
      },
    });
  }

  closeModal(): void {
    const modal = document.getElementById('exampleModalToggle');
    if (modal) {
      const modalInstance = bootstrap.Modal.getInstance(modal);
      modalInstance?.hide();
    }
    this.selectedUnit = null;
    this.updateForm.reset();
  }

  deleteBikeByRegistration(registrationNumber: string): void {
    if (
      confirm(
        `Are you sure you want to delete the bike with registration number ${registrationNumber}?`
      )
    ) {
      this.bikeService.deleteBike(registrationNumber).subscribe({
        next: () => {
          this.toastr.success(
            `Bike with registration number ${registrationNumber} has been successfully deleted.`
          );
          this.loadBikes(); // Reload bikes after deletion
        },
        error: (err) => {
          console.error(
            `Error deleting bike with registration number ${registrationNumber}:`,
            err
          );
          this.toastr.error(`Failed to delete bike: ${err.message}`);
        },
      });
    }
  }
}

// Interfaces for Bike, BikeUnit, and BikeImage
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
