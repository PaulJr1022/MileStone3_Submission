import { Component } from '@angular/core';
import Chart from 'chart.js/auto';
import { UserReportService } from '../Services/user-report.service';

@Component({
  selector: 'app-admin-report-user-history',
  templateUrl: './admin-report-user-history.component.html',
  styleUrl: './admin-report-user-history.component.css'
})
export class AdminReportUserHistoryComponent {
  public pieChart: any;
  public barChart: any;
  userHistories: any[] = [];

  
  constructor(private reportService: UserReportService) {}

  ngOnInit(): void {
    this.loadUserHistoryData();
  }

  private loadUserHistoryData(): void {
    this.reportService.getUserHistory().subscribe(data => {
      this.userHistories = data;
    });
  }

}
