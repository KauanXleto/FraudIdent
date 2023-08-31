import { Component, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  truckSelect: number = 0;

  updateTruckId (event: number){
    this.truckSelect = event;
  }
}
