import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserReportService } from '../Services/user-report.service';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent {
  reports: any[] = [];
  //id: number = 9;
  //userId! :'';

  
    fname : any
    lname : any
  


  constructor(private userReportService: UserReportService, private router: Router) {
    //this.loadUser();
  }

  ngOnInit(): void {
    this.loadReports();
 
    

  }



  loadReports(): void {
    // const UserId : any =localStorage.getItem('token');
    // // console.log(UserId);
    // const decodeId = jwtDecode(UserId);
    //  console.log(decodeId);
    // const id = decodeId.userId;

    const token = localStorage.getItem('token');
    const decodedToken: CustomJwtPayload = jwtDecode<CustomJwtPayload>(token!);
    this.fname = decodedToken.firstname;
    this.lname = decodedToken.lastname;
    const id = decodedToken.userId;
    console.log(decodedToken.firstname, this.lname);
    
     
    console.log(id);




    // const token = localStorage.getItem('token');
    // if (token) {
    //   const decodedToken: CustomJwtPayload = jwtDecode<CustomJwtPayload>(token);
    //  // console.log(decodedToken);

    //   const userId = decodedToken.userId; // Access the 'userId' property
    //   console.log('User ID:', userId);
    // } else {
    //   console.error('No token found in localStorage');
    // }




    this.userReportService.userReport(id).subscribe({
      next: (data) => {
        console.log(id);
        this.reports = data;
      },
      error: (err) => {
        console.error('Error fetching user report data:', err);
      }
    });
  }



  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/home'])
  }

}

interface CustomJwtPayload {
  aud: string;
  email: string;
  exp: number;
  iss: string;
  role: string;
  userId: string;
  lastname :any;
  firstname : any;
}
