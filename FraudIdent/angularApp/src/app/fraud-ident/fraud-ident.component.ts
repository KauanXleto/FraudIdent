import { Component } from '@angular/core';

@Component({
  selector: 'app-fraud-ident',
  templateUrl: './fraud-ident.component.html',
  styleUrls: ['./fraud-ident.component.css']
})
export class FraudIdentComponent {

  truckSelect: number = 0;

  updateTruckId (event: number){
    this.truckSelect = event;
  }
}
