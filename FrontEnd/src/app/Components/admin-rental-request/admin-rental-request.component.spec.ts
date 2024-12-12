import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminRentalRequestComponent } from './admin-rental-request.component';

describe('AdminRentalRequestComponent', () => {
  let component: AdminRentalRequestComponent;
  let fixture: ComponentFixture<AdminRentalRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminRentalRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminRentalRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
