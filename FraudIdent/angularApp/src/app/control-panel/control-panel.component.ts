import { Component, EventEmitter, Input, Output } from '@angular/core';
import { OnInit } from '@angular/core';
import { AppApiService } from '../app.api.service';
import { HttpClient } from '@angular/common/http';
import { AppTruckModel } from 'models/app-truck-model';
import { AppCoreModule } from 'models/app-core-model';

@Component({
  selector: 'app-control-panel',
  templateUrl: './control-panel.component.html',
  styleUrls: ['./control-panel.component.css']
})
export class ControlPanelComponent implements OnInit{

  constructor(private appApiService: AppApiService) {}

  listTruckTypes: AppTruckModel[] = [];

  model: AppCoreModule = new AppCoreModule();

  @Output()
  truckId = new EventEmitter<number>();

  @Input()
  validateFraud: boolean = false;

  @Input()
  enableRegister: boolean = false;

  ngOnInit(): void {
    this.initComponent();
  }

  initComponent(){
    this.getModelsAvailable();
  }

  onSelectTruckModel(){
    const truck = this.listTruckTypes.find(x => x.Id == this.model.TruckId);

    if(truck != null){

      this.model.Truck = truck;
      if(this.model.Truck.TruckParam == null)
        this.model.Truck.TruckParam = {
          Id: 0,
          TruckId: 0,
          Height: 0,
          Length: 0,
          Width: 0
        };

        if(this.model.TruckId > 0 && this.validateFraud){  
          this.appApiService.changeTruckSelected(this.model.TruckId).subscribe(() => {});
        }

        this.truckId.emit(this.model.TruckId);
    }
  }

  getModelsAvailable(){
    this.appApiService.getTruckModels().subscribe((data) => {
      
      this.listTruckTypes = data;

      this.getBalanceInfo();
      
    },
    (error) => {
      console.log(`Error: ${error.error}`);
    });
  }

  getBalanceInfo(){
    this.appApiService.getBalanceInfos().subscribe((data) => {
      
      this.model.BalanceInfo = data
      
    },
    (error) => {
      console.log(`Error: ${error.error}`);
    });
  }

  saveInfos(){
    this.appApiService.saveInfos(JSON.stringify(this.model)).subscribe((data) => {
           
      this.initComponent();
    },
    (error) => {
      console.log(`Error: ${error.error}`);
    });
  }
}
