import { AfterViewInit, Component, OnInit } from '@angular/core';
import { UserReportService } from '../Services/user-report.service';
import Chart from 'chart.js/auto';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-admin-dash-home',
  templateUrl: './admin-dash-home.component.html',
  styleUrl: './admin-dash-home.component.css'
})
export class AdminDashHomeComponent implements OnInit, AfterViewInit{

  totalUsers: number = 0;
  totalBikes: number = 0;
  BookedBikes: number = 0;
  revenue: number = 0;
  bikes: any[] = [];
  userHistories: any[] = [];
  public pieChart: any;
  public barChart: any;

  constructor(private reportService: UserReportService) {}

  ngOnInit(): void {
    this.loadReport();
    this.loadInventoryData();
    this.loadUserHistoryData();
  }

  ngAfterViewInit(): void {
    this.createPieChart();
    this.createBarChart();
  }

  private loadReport(): void {
    this.reportService.getTotalUsers().subscribe(data => this.totalUsers = data);
    this.reportService.getTotalBikes().subscribe(data => this.totalBikes = data);
    this.reportService.getBookedBikes().subscribe(data => this.BookedBikes = data);
    this.reportService.getRevenue().subscribe(data => this.revenue = data);
  }

  private loadInventoryData(): void {
    this.reportService.getInventoryManagement().subscribe((data: any) => {
      this.bikes = data;
    });
  }

  private loadUserHistoryData(): void {
    this.reportService.getUserHistory().subscribe(data => {
      this.userHistories = data;
    });
  }

  private createPieChart(): void {
    this.reportService.getRevenueByBike().subscribe((data) => {
      const labels = data.map((item: any) => item.modelName);
      const revenues = data.map((item: any) => item.revenue);

      this.pieChart = new Chart('PieChart', {
        type: 'pie',
        data: {
          labels: labels,
          datasets: [{
            label: 'Revenue by Bike',
            data: revenues,
            backgroundColor: [
              'red', 'pink', 'green', 'yellow', 'orange', 'blue', 'purple', 'cyan'
            ],
            hoverOffset: 4
          }]
        },
        options: {
          responsive: true,
          aspectRatio: 2.5
        }
      });
    });
  }

  private createBarChart(): void {
    this.reportService.getRevenueByMonth().subscribe((data) => {
      const labels = data.map((item: any) => item.month); 
      const revenues = data.map((item: any) => item.revenue); 
      
      
      this.barChart = new Chart('BarChart', {
        type: 'bar', 
        data: {
          labels: labels, 
          datasets: [{
            label: 'Revenue by Month', 
            data: revenues, 
            backgroundColor: [
              'red', 'pink', 'green', 'yellow', 'orange', 'blue', 'purple', 'cyan'
            ],
            borderColor: [
              'darkred', 'darkpink', 'darkgreen', 'darkyellow', 'darkorange', 'darkblue', 'darkpurple', 'darkcyan'
            ],
            borderWidth: 1,
            barThickness: 20, 
            maxBarThickness: 30, 
          }]
        },
        options: {
          responsive: true,
          aspectRatio: 2.5,
          scales: {
            x: {
              title: {
                display: true,
                text: 'Month'
              },
              grid: {
                display: false 
              },
             // categoryPercentage: 0.8, 
             // barPercentage: 0.9 
            },
            y: {
              beginAtZero: true,
              title: {
                display: true,
                text: 'Revenue'
              },
              grid: {
                display: true, 
                color: 'rgba(0,0,0,0.1)'
              }
            }
          }
        }
      });
    });
  }

  
  public printPdf(): void {
    const DATA: any = document.getElementById('contentToDownload'); // Reference the main container
    const doc = new jsPDF('p', 'mm', 'a4'); // Create jsPDF object

    html2canvas(DATA).then((canvas) => {
      const imgWidth = 190; // Fit the content within A4 width
      const imgHeight = (canvas.height * imgWidth) / canvas.width; // Maintain aspect ratio
      const imgData = canvas.toDataURL('image/png');

      doc.addImage(imgData, 'PNG', 10, 10, imgWidth, imgHeight);
      doc.save('Admin_Dashboard.pdf'); // Save the PDF file
    });
  }
}
