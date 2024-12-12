import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserUpdateRequestComponent } from './user-update-request.component';

describe('UserUpdateRequestComponent', () => {
  let component: UserUpdateRequestComponent;
  let fixture: ComponentFixture<UserUpdateRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserUpdateRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserUpdateRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
