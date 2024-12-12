import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserRentalRequestComponent } from './user-rental-request.component';

describe('UserRentalRequestComponent', () => {
  let component: UserRentalRequestComponent;
  let fixture: ComponentFixture<UserRentalRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserRentalRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserRentalRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
