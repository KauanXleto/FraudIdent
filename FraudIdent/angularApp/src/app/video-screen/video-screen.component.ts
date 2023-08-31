import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { AppApiService } from '../app.api.service';
import { AppLobInfoModel } from 'models/app-lob-info-model';

@Component({
  selector: 'app-video-screen',
  templateUrl: './video-screen.component.html',
  styleUrls: ['./video-screen.component.css']
})
export class VideoScreenComponent implements OnInit{

  constructor(private appApiService: AppApiService) {}

  @Input()
  truckId: number = 0;

  viewDistance: boolean = false;

  lastlistLogInfos: AppLobInfoModel[] = []
  listLogInfos: AppLobInfoModel[] = []

  lastLobInfo: AppLobInfoModel = {
    Id: 0,
    TruckId: 0,
    HasError: false,
    HasSuccess: false,
    MessageError: '',
    BackImageTruck: '',
    SideImageTruck: ''
  }

  changeView(){
    this.viewDistance = !this.viewDistance;
  }

  ngOnInit() {
    this.verifyLobInfo();
  }

  verifyLobInfo(){
    setInterval(() => {         
      this.getLobInfo();
    }, 500);
  }

  contStartVideo = 3;

  getLobInfo(){
    this.appApiService.getLobInfo(this.truckId, this.viewDistance).subscribe((data) => {
      console.log(data[0])
      this.listLogInfos = data;
      
      this.runVideoSaved();

      if(JSON.stringify(this.listLogInfos) != JSON.stringify(this.lastlistLogInfos)){
        
        this.contStartVideo = 3;
        this.runVideo = false;
        this.lastlistLogInfos = data;

      }
      else{
        if(this.contStartVideo == 0)
          this.runVideo = true;
        else
          this.contStartVideo -= 1;
      }
    },
    (error) => {
      console.log(`Error: ${error.error}`);
    });
  }

  runVideo: boolean = false;
  lastIndexRun: number = 10;

  runVideoSaved(){

    if(!this.runVideo){
      this.lastIndexRun = 10;
      
      if(this.listLogInfos != null && this.listLogInfos.length > 0)
        this.lastLobInfo = this.listLogInfos[0];
    }
    else{

      if(this.listLogInfos != null && this.listLogInfos.length > 0){

        this.lastLobInfo = this.listLogInfos[this.lastIndexRun - 1];
        this.lastIndexRun -= 1;
  
        if(this.lastIndexRun == 0)
          this.lastIndexRun = 10;
      }
    }
  }
}
