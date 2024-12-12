// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-admin-reports',
//   templateUrl: './admin-reports.component.html',
//   styleUrl: './admin-reports.component.css'
// })
// export class AdminReportsComponent {



// }

import { Component, OnInit } from '@angular/core';
import { Chart, ChartConfiguration } from 'chart.js';
import { UserReportService } from '../Services/user-report.service';
// import Chart from 'chart.js/auto';

@Component({
  selector: 'app-admin-reports',
  templateUrl: './admin-reports.component.html',
  styleUrl: './admin-reports.component.css'
})
export class AdminReportsComponent implements OnInit {
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
    this.createChart();
    this.loadInventoryData();
  }

  private loadInventoryData(): void {
    this.reportService.getInventoryManagement().subscribe((data: any) => {
      this.bikes = data;
    });
  }

  createChart(): void {
    const canvas = document.getElementById('rentalChart') as HTMLCanvasElement | null;

    if (canvas) {
      const ctx = canvas.getContext('2d');

      if (ctx) {
        const config: ChartConfiguration = {
          type: 'line',
          data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
              label: 'Monthly Rentals',
              data: [200, 300, 250, 400, 450, 600, 700, 750, 500, 400, 350, 300],
              borderColor: 'rgba(75, 192, 192, 1)',
              backgroundColor: 'rgba(75, 192, 192, 0.2)',
              fill: true,
              tension: 0.4
            }]
          },
          options: {
            responsive: true,
            plugins: {
              legend: {
                position: 'top',
              },
              title: {
                display: true,
                text: 'Monthly Motorbike Rentals'
              }
            },
            scales: {
              y: {
                beginAtZero: true
              }
            }
          }
        };

        new Chart(ctx, config);
      }
    }
  }
}

