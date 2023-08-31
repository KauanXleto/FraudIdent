import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditRegisterTruckComponent } from './edit-register-truck.component';

describe('EditRegisterTruckComponent', () => {
  let component: EditRegisterTruckComponent;
  let fixture: ComponentFixture<EditRegisterTruckComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditRegisterTruckComponent]
    });
    fixture = TestBed.createComponent(EditRegisterTruckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
