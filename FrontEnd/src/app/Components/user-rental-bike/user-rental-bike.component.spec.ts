import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserRentalBikeComponent } from './user-rental-bike.component';

describe('UserRentalBikeComponent', () => {
  let component: UserRentalBikeComponent;
  let fixture: ComponentFixture<UserRentalBikeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserRentalBikeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserRentalBikeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
