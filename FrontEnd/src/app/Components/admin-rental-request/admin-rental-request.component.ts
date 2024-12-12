import { Component } from '@angular/core';
import { BikesService } from '../Services/bikes.service';
import * as bootstrap from 'bootstrap';
import { RentalRequestService } from '../Services/rental-request.service';
import { ToastrService } from 'ngx-toastr';
import jsPDF from 'jspdf';
import 'jspdf-autotable';

@Component({
  selector: 'app-admin-rental-request',
  templateUrl: './admin-rental-request.component.html',
  styleUrls: ['./admin-rental-request.component.css']
})
export class AdminRentalRequestComponent {
  rentalRequests: any[] = [];
  rentalDetails: any[] | null = null;
  req : any

  constructor(private rentalService: RentalRequestService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.fetchRentalRequests();
  }

  fetchRentalRequests(): void {
    this.rentalService.getAllRequests().subscribe(
      (data) => {
        console.log('Fetched rental requests:', data);
        this.rentalRequests = data.map((request: any) => ({
          ...request,
          isAccepted: false,
          isReturned: false
        }));
      },
      (error) => {
        console.error('Error fetching rental requests:', error);
      }
    );
  }

  viewDetails(id: number): void {
    this.rentalDetails = null;

    this.rentalService.getRentalHistoryByUser(id).subscribe(
      (data) => {
        this.rentalDetails = data;
        const modalElement = document.getElementById('rentalHistoryModal');
        if (modalElement) {
          const modal = new bootstrap.Modal(modalElement);
          modal.show();
        } else {
          console.error('Modal element not found.');
        }
      },
      (error) => {
        console.error('Error fetching rental history:', error);
        alert('Failed to fetch rental history. Please try again.');
      }
    );
    console.log(this.rentalDetails);
  }

  updateRequestStatus(requestId: number, newStatus: number, actionMessage: string): void {
    const request = this.getRequestById(requestId);
    if (request) {
      if (newStatus === 3) request.isAccepted = true;
      if (newStatus === 4) request.isReturned = true;
    }
    this.rentalService.updateRequestStatus(requestId, newStatus).subscribe({
      next: () => {
        this.toastr.info(`Rental request (ID: ${requestId}) ${actionMessage}.`);
        this.fetchRentalRequests();
      },
      error: (err) => {
        console.error(`Error updating rental request status to ${newStatus}:`, err);
        this.toastr.error('Failed to update the rental request status. Please try again.');
      }
    });
  }

  acceptRequest(requestId: number, request: any): void {
    const requests = this.getRequestById(requestId);
    if (!requests || requests.isAccepted || requests.isReturned) {
      this.toastr.error('Invalid action.');
      return;
    }
    const confirmAccept = confirm('Are you sure you want to accept this rental request?');
    if (confirmAccept) {
      this.updateRequestStatus(requestId, 3, 'accepted');
      //request.status = 'Pending';
    }
  }

  declineRequest(requestId: number): void {
    const confirmDecline = confirm('Are you sure you want to decline this rental request?');
    if (confirmDecline) {
      
      this.updateRequestStatus(requestId, 2, 'declined');
    }
  }

  returnRequest(requestId: number): void {
    const request = this.getRequestById(requestId);
    if (!request || request.isReturned) {
      this.toastr.warning('Invalid action.');
      return;
    }
    const confirmReturn = confirm('Are you sure you want to mark this rental request as "Returned"?');
    if (confirmReturn) {
      this.updateRequestStatus(requestId, 4, 'returned');
    }
  }

  private getRequestById(requestId: number): any {
    return this.rentalRequests.find(request => request.requestId === requestId);
  }

  printTableAsPDF(): void {
    const doc = new jsPDF();
    const columns = ['Bike ID', 'User ID', 'Bike Reg', 'Start Date', 'End Date', 'Total Price'];
    const rows = this.rentalRequests.map(request => [
      request.bikeId,
      request.userId,
      request.registrationNumber,
      new Date(request.fromDate).toLocaleDateString(),
      new Date(request.toDate).toLocaleDateString(),
      request.amount
    ]);

    doc.text('Rental Requests', 14, 10);
    (doc as any).autoTable({
      head: [columns],
      body: rows,
      startY: 20
    });

    doc.save('RentalRequests.pdf');
  }
}
