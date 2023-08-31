import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FraudIdentComponent } from './fraud-ident.component';

describe('FraudIdentComponent', () => {
  let component: FraudIdentComponent;
  let fixture: ComponentFixture<FraudIdentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FraudIdentComponent]
    });
    fixture = TestBed.createComponent(FraudIdentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
