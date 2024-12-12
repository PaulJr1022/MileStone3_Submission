import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminRentalReportComponent } from './admin-rental-report.component';

describe('AdminRentalReportComponent', () => {
  let component: AdminRentalReportComponent;
  let fixture: ComponentFixture<AdminRentalReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminRentalReportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminRentalReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
