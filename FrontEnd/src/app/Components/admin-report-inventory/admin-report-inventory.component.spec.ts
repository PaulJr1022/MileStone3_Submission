import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminReportInventoryComponent } from './admin-report-inventory.component';

describe('AdminReportInventoryComponent', () => {
  let component: AdminReportInventoryComponent;
  let fixture: ComponentFixture<AdminReportInventoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminReportInventoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminReportInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
