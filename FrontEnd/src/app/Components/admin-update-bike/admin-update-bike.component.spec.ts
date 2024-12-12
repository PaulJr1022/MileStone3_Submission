import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUpdateBikeComponent } from './admin-update-bike.component';

describe('AdminUpdateBikeComponent', () => {
  let component: AdminUpdateBikeComponent;
  let fixture: ComponentFixture<AdminUpdateBikeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminUpdateBikeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminUpdateBikeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
