import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUserRequestComponent } from './admin-user-request.component';

describe('AdminUserRequestComponent', () => {
  let component: AdminUserRequestComponent;
  let fixture: ComponentFixture<AdminUserRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminUserRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminUserRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
