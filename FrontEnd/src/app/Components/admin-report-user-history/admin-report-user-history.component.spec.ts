import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminReportUserHistoryComponent } from './admin-report-user-history.component';

describe('AdminReportUserHistoryComponent', () => {
  let component: AdminReportUserHistoryComponent;
  let fixture: ComponentFixture<AdminReportUserHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminReportUserHistoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminReportUserHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
