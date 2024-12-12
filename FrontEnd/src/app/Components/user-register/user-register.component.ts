import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent {
  registerForm: FormGroup;
  licenseImage: File | null = null;
  cameraCapture: File | null = null;
  private stream: MediaStream | null = null;
  isCameraOpen = false;
  num = Math.random();

  @ViewChild('videoElement') videoElement!: ElementRef<HTMLVideoElement>;
  @ViewChild('canvasElement') canvasElement!: ElementRef<HTMLCanvasElement>;
  @ViewChild('capturedImageElement') capturedImageElement!: ElementRef<HTMLImageElement>;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      FirstName: ['', [Validators.required]],
      LastName: ['', [Validators.required]],
      Email: ['', [Validators.required, Validators.email]],
      MobileNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      NIC: ['', [Validators.required, Validators.pattern(/^[0-9]{9}[VXvx]$|^[0-9]{12}$/)]],
      LicenseNumber: ['', [Validators.required, Validators.pattern(/^[A-Z]{1}[0-9]{7}$/)]],
      Role: ['2'],
      Status: [1],
      LicenseImage: [null, [Validators.required]], // Required license image
      CameraCapture: [null, [Validators.required]] // Required camera capture
    });
  }

  async openCamera(): Promise<void> {
    try {
      this.isCameraOpen = true;
      this.stream = await navigator.mediaDevices.getUserMedia({ video: true });
      const video = this.videoElement.nativeElement;
      video.srcObject = this.stream;
      video.play();
    } catch (error) {
      console.error('Error accessing the camera:', error);
      this.toastr.warning('Unable to access the camera!');
    }
  }

  closeCamera(): void {
    if (this.stream) {
      this.stream.getTracks().forEach(track => track.stop());
      this.stream = null;
      this.isCameraOpen = false;
      const video = this.videoElement.nativeElement;
      video.srcObject = null;
    }
  }

  takePhoto(): void {
    if (!this.stream) {
      this.toastr.error('Camera not open!');
      return;
    }

    const video = this.videoElement.nativeElement;
    const canvas = this.canvasElement.nativeElement;

    const ctx = canvas.getContext('2d');
    if (ctx) {
      canvas.width = video.videoWidth;
      canvas.height = video.videoHeight;
      ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

      canvas.toBlob(blob => {
        if (blob) {
          this.cameraCapture = new File([blob], `camera_capture${this.num}.png`, { type: blob.type });
          this.registerForm.patchValue({ CameraCapture: this.cameraCapture });

          const capturedImage = this.capturedImageElement.nativeElement;
          capturedImage.src = URL.createObjectURL(blob);
          capturedImage.style.display = 'block';
        }
      }, 'image/png');
    }
  }

  onFileChange(event: Event, field: string): void {
    const file = (event.target as HTMLInputElement).files?.[0] || null;
    if (field === 'LicenseImage') {
      this.licenseImage = file;
      this.registerForm.patchValue({ LicenseImage: this.licenseImage });
    } else if (field === 'CameraCapture') {
      this.cameraCapture = file;
      this.registerForm.patchValue({ CameraCapture: this.cameraCapture });
    }
  }

  RegisterMember(): void {
    if (this.registerForm.invalid) {
     // this.toastr.error('Please fill out all required fields.', 'Form Invalid');
      return;
    }

    const formData = new FormData();
    Object.keys(this.registerForm.controls).forEach(key => {
      if (key !== 'LicenseImage' && key !== 'CameraCapture') {
        formData.append(key, this.registerForm.get(key)?.value);
      }
    });

    if (this.licenseImage) {
      formData.append('LicenseImage', this.licenseImage, this.licenseImage.name);
    }
    if (this.cameraCapture) {
      formData.append('CameraCapture', this.cameraCapture, this.cameraCapture.name);
    }

    this.http.post('https://localhost:7000/api/User/CreateUser', formData).subscribe({
      next: response => {
        this.toastr.success('User registered successfully, please wait for admin approval. You will get an email.', 'Success');
        this.router.navigateByUrl('/home');
      },
      error: error => {
        console.error('Error registering user:', error);
        this.toastr.error('Error registering user. Please try again.', 'Error');
      }
    });
  }

  ngOnDestroy(): void {
    if (this.stream) {
      this.stream.getTracks().forEach(track => track.stop());
    }
  }
}
