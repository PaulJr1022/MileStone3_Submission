import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserViewBikeComponent } from './user-view-bike.component';

describe('UserViewBikeComponent', () => {
  let component: UserViewBikeComponent;
  let fixture: ComponentFixture<UserViewBikeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserViewBikeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserViewBikeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
