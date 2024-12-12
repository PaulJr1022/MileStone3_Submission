import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../Services/user.service';
import { jwtDecode } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-update',
  templateUrl: './user-update.component.html',
  styleUrls: ['./user-update.component.css']
})
export class UserUpdateComponent implements OnInit {

  updateUserForm: FormGroup;
  userId : any 

  constructor(private fb: FormBuilder, private userService: UserService, private toastr : ToastrService) {
    this.updateUserForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: [{ value: '', disabled: true }, [Validators.required, Validators.email]],
      nic: [{ value: '', disabled: true }, Validators.required],
      licenseNumber: [{ value: '', disabled: true }, Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      mobileNumber: ['', [Validators.required, Validators.pattern('^\\d{10}$')]],
    });
  }

  ngOnInit(): void {
   

    const token = localStorage.getItem('token');
    if (token) {
      const decodedToken: CustomJwtPayload = jwtDecode<CustomJwtPayload>(token);
      this.userId= decodedToken.userId;
      this.getUserDetails(this.userId);
      //console.log(this.userId);
    } else{
      this.toastr.show('User Id Not found');
    }
    
  }

  getUserDetails(userId: any) {
    this.userService.getUserById(this.userId).subscribe(
      (user) => {
        this.updateUserForm.patchValue({
          firstName: user.firstName,
          lastName: user.lastName,
          email: user.email,
          nic: user.nic,
          licenseNumber: user.licenseNumber,
          password: '', // Don't prefill password for security reasons
          mobileNumber: user.mobileNumber,
        });
      },
      (error) => {
        console.error('Error fetching user details:', error);
      }
    );
  }

  updateUser() {
    if (this.updateUserForm.invalid) {
      return;
    }

    const updatedUser = this.updateUserForm.getRawValue(); 
    this.userService.updateUser(this.userId, updatedUser).subscribe(
      () => {
        this.toastr.success('User updated successfully!')
        //alert('User updated successfully!');
      },
      (error) => {
        console.error('Error updating user:', error);
      }
    );
  }
}


interface CustomJwtPayload {
  userId: string;
}
