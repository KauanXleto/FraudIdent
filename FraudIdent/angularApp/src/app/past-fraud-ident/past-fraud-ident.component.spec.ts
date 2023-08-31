import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PastFraudIdentComponent } from './past-fraud-ident.component';

describe('PastFraudIdentComponent', () => {
  let component: PastFraudIdentComponent;
  let fixture: ComponentFixture<PastFraudIdentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PastFraudIdentComponent]
    });
    fixture = TestBed.createComponent(PastFraudIdentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
