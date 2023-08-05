import { Component, OnInit } from '@angular/core';
import { AppApiService } from '../app.api.service';
import { AppLobInfoModel } from 'models/app-lob-info-model';

@Component({
  selector: 'app-video-screen',
  templateUrl: './video-screen.component.html',
  styleUrls: ['./video-screen.component.css']
})
export class VideoScreenComponent implements OnInit{

  constructor(private appApiService: AppApiService) {}

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

  ngOnInit() {
    this.verifyLobInfo();
  }

  verifyLobInfo(){
    setInterval(() => {         
      this.getLobInfo();
    }, 500);
  }

  getLobInfo(){
    this.appApiService.getLobInfo().subscribe((data) => {
      
      this.listLogInfos = data;      
      this.lastLobInfo = data[0];
    },
    (error) => {
      console.log(`Error: ${error.error}`);
    });
  }
}
