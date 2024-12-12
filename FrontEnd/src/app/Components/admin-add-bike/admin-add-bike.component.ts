import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { BikesService } from '../Services/bikes.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-add-bike',
  templateUrl: './admin-add-bike.component.html',
  styleUrls: ['./admin-add-bike.component.css'],
})
export class AdminAddBikeComponent implements OnInit {
  bikeForm: FormGroup;
  bikes: Bike[] = [];
  brands: any[] = [];
  models: any[] = [];
  selectedBrand: number | null = null;
  selectedModel: string | null = null;
  newBrandName: string = '';
  newModelName: string = '';

  showAddBrandPopup = false;
  showAddModelPopup = false;

 searchBrand:string='';
 searchRent: number| null=null;
 activeTab:string='brand';


  constructor(
    private bikeService: BikesService,
    private fb: FormBuilder,
    private http: HttpClient,
    private toastr: ToastrService,
    private cdRef: ChangeDetectorRef // Change detection
  ) {
    this.bikeForm = this.fb.group({
      bikeUnits: this.fb.array([this.createBikeUnit()]),
      image: [null, Validators.required],
    });
  }

  ngOnInit(): void {
    this.fetchBrands();
    this.bikeService.getAllBikes().subscribe({
      next: (data: Bike[]) => {
        this.bikes = data;
      },
      error: (err) => {
        console.error('Error fetching bikes:', err);
      },
    });
  }

  get bikeUnits(): FormArray {
    return this.bikeForm.get('bikeUnits') as FormArray;
  }

  createBikeUnit(): FormGroup {
    return this.fb.group({
      registrationNumber: ['', Validators.required],
      year: [null, [Validators.required, Validators.min(1900)]],
      rentPerDay: [null, [Validators.required, Validators.min(0)]],
    });
  }

  addBikeUnit() {
    this.bikeUnits.push(this.createBikeUnit());
  }

  removeBikeUnit(index: number) {
    this.bikeUnits.removeAt(index);
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    this.bikeForm.patchValue({ image: file });
  }

  fetchBrands() {
    this.http.get('https://localhost:7000/api/Bike/AllBrands').subscribe((data: any) => {
      this.brands = data;
    });
  }

  fetchModels() {
    if (this.selectedBrand) {
      this.http
        .get(`https://localhost:7000/api/Bike/GetModelByBrand${this.selectedBrand}`)
        .subscribe((data: any) => {
          this.models = data;
        });
    }
  }

  openAddBrandPopup() {
    this.showAddBrandPopup = true;
    this.cdRef.detectChanges();
  }

  closeAddBrandPopup() {
    this.showAddBrandPopup = false;
    this.newBrandName = '';
    this.cdRef.detectChanges();
  }

  openAddModelPopup() {
    if (!this.selectedBrand) {
      this.toastr.error('Please select a brand first.');
      return;
    }
    this.showAddModelPopup = true;
    this.cdRef.detectChanges();
  }

  closeAddModelPopup() {
    this.showAddModelPopup = false;
    this.newModelName = '';
    this.cdRef.detectChanges();
  }

  addBrand() {
    if (!this.newBrandName.trim()) {
      this.toastr.warning('Brand name is required.');
      return;
    }
    const payload = { brandName: this.newBrandName };
    this.http.post('https://localhost:7000/api/Bike/AddBrand', payload).subscribe(
      () => {
        this.closeAddBrandPopup();
        this.fetchBrands();
        this.toastr.success('Brand added successfully.');
      },
      (error) => {
        console.error('Error adding brand:', error);
        this.toastr.error('Error adding brand.');
      }
    );
  }

  addModel() {
    if (!this.newModelName.trim()) {
      this.toastr.warning('Model name is required.');
      return;
    }
    const payload = { modelName: this.newModelName, brandId: this.selectedBrand };
    this.http.post('https://localhost:7000/api/Bike/AddModel', payload).subscribe(
      () => {
        this.closeAddModelPopup();
        this.fetchModels();
        this.toastr.success('Model added successfully.');
      },
      (error) => {
        console.error('Error adding model:', error);
        this.toastr.error('Error adding model.');
      }
    );
  }

  submitForm() {
    if (this.bikeForm.invalid || !this.selectedModel) {
      this.toastr.error('Form is invalid or no model selected.');
      return;
    }
    const formData = this.bikeForm.value;
    const payload = {
      modelName: this.selectedModel,
      bikeUnits: formData.bikeUnits,
    };
    this.bikeService.addBike(payload).subscribe(
      (response) => {
        this.toastr.success('Bike added successfully.');
        this.ngOnInit();
        const unitId = response[0]?.unitId;
        if (unitId) {
          this.uploadImage(unitId, formData.image);
        }
      },
      (error) => {
        console.error('Error adding bike:', error);
        this.toastr.error('Duplicate Registation Number Found');
      }
    );
  }

  uploadImage(unitId: number, imageFile: File) {
    this.bikeService.uploadImage(unitId, imageFile).subscribe(
      (response) => {
        console.log('Image uploaded successfully:', response);
      },
      (error) => {
        console.error('Error uploading image:', error);
      }
    );
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
