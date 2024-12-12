import { Component } from '@angular/core';
import { UserRequestService } from '../Services/user-request.service';
import { ToastrService } from 'ngx-toastr';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-admin-user-request',
  templateUrl: './admin-user-request.component.html',
  styleUrls: ['./admin-user-request.component.css'],
})
export class AdminUserRequestComponent {
  users: any[] = [];

  constructor(private userRequestService: UserRequestService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  // Fetch user data from the service
  loadUsers(): void {
    this.userRequestService.getUser().subscribe({
      next: (data) => {
        this.users = data.map((user: any) => ({
          ...user,
        }));
      },
      error: (err) => {
        console.error('Error fetching users:', err);
      },
    });
  }

  accept(user: any): void {
    this.userRequestService.updateUserRequestStatus(user.userId, 6).subscribe({
      next: () => {
       // alert('User request accepted successfully!');
        user.status = 'Accepted';
        this.toastr.success('User request accepted successfully!', 'Success');
      },
      error: (err: any) => {
        this.toastr.error('Failed to accept the user request.', 'Error');
        console.error(err);
      },
    });
  }

  decline(user: any): void {
    this.userRequestService.updateUserRequestStatus(user.userId, 2).subscribe({
      next: () => {
        user.status = 'Rejected';
        this.toastr.success('User request declined successfully!', 'Success');
      },
      error: (err: any) => {
        this.toastr.error('Failed to decline the user request.', 'Error');
        console.error(err);
      },
    });
  }

  printTableAsPDF(): void {
    const tableElement = document.querySelector('.table-custom') as HTMLElement;

    if (!tableElement) {
      this.toastr.error('Table element not found.', 'Error');
      return;
    }

    html2canvas(tableElement, {
      scale: 2, // Increases the resolution of the captured image
    }).then((canvas) => {
      const imgData = canvas.toDataURL('image/png');
      const pdf = new jsPDF('p', 'mm', 'a4');
      const pdfWidth = pdf.internal.pageSize.getWidth();
      const pdfHeight = (canvas.height * pdfWidth) / canvas.width;

      pdf.addImage(imgData, 'PNG', 0, 0, pdfWidth, pdfHeight);
      pdf.save('User_Requests.pdf');
    });
  }
}
