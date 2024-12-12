import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserRequestService } from '../Services/user-request.service';
import { Route, Router } from '@angular/router';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { AuthGuard } from '../Services/auth.guard';
import { HttpClient } from '@angular/common/http';
import { Token } from '@angular/compiler';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrl: './log-in.component.css'
})
export class LogInComponent {

  Token!: '';
  loginObj: any = {
    Email: '',
    Password: '',
   // Role: ['1', '2']
  }

  constructor(private http: HttpClient, private router: Router, private toastr: ToastrService) {

  }
  // ngOnDestroy(): void {
  //   ///this.loginObj.reset();
   
  // }
  ngOnInit(): void {
    //this.loginObj.reset();
  }

  

  onLogin() {

    // this.http
    //   .post('https://localhost:7000/api/User/Login', this.loginObj, { responseType: 'text' })
    //   .subscribe({
    //     next: (res: string) => {
    //       // Store the token and navigate
    //       alert('Successfully Logged in');
    //       localStorage.setItem('token', res); // Save the token directly
    //       this.router.navigateByUrl('/user');
    //     },
    //     error: (err) => {
    //       console.error('Login error:', err);
    //       alert('Login failed');
    //     },
    //   });

    this.http
      .post('https://localhost:7000/api/User/Login', this.loginObj, {
        responseType: 'text',
      })
      .subscribe({
        next: (res: string) => {
          // Store the token and decode it
          //alert('Successfully Logged in');
          localStorage.setItem('token', res);
          //this.loginObj.reset();

          try {
            const decodedToken: any = jwtDecode(res);
            //console.log(decodedToken);
             // Decode the JWT token
            const userRole = decodedToken.role; // Extract role from token
            // Navigate based on role
            if (userRole === 'Admin') {
              this.toastr.success('Welcome Admin', 'Success');
              this.router.navigateByUrl('/admin/dashHome');
            } else if (userRole === 'StandardUser') {
            this.toastr.success('Welcome User ', 'Success');
              this.router.navigateByUrl('/user');
            } else {
              alert('Unknown role');
            }

            // Optional: Store additional details in localStorage
            localStorage.setItem('details', JSON.stringify(decodedToken));
          } catch (error) {
            console.error('Token decoding error:', error);
           // alert('Invalid token');
          }

          // Reset login form
          this.loginObj = { Email: '', Password: '' };
        },
        error: (err) => {
          console.error('Login error:', err);
          //alert('Login failed');
          this.toastr.error('Login failed', 'Error')
        },
      });




      const token = localStorage.getItem('token');
      if(token){
        const decode = jwtDecode(token);
        localStorage.setItem('details', JSON.stringify(decode));
      }

  
  }

}
