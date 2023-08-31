import { Component } from '@angular/core';

@Component({
  selector: 'app-past-fraud-ident',
  templateUrl: './past-fraud-ident.component.html',
  styleUrls: ['./past-fraud-ident.component.css']
})
export class PastFraudIdentComponent {

  truckSelect: number = 0;

  updateTruckId (event: number){
    this.truckSelect = event;
  }
}
