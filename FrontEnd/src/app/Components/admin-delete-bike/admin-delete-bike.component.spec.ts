import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminDeleteBikeComponent } from './admin-delete-bike.component';

describe('AdminDeleteBikeComponent', () => {
  let component: AdminDeleteBikeComponent;
  let fixture: ComponentFixture<AdminDeleteBikeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminDeleteBikeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminDeleteBikeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
