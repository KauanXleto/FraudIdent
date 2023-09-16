import { Inject, Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpContext, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppTruckModel } from 'models/app-truck-model';
import { AppBanceInfoModel } from 'models/app-balance-info-model';
import { AppLobInfoModel } from 'models/app-lob-info-model';

@Injectable({
  providedIn: 'root'
})
export class AppApiService {

  controllerUrl: string = "";

  constructor(private http: HttpClient) {
    this.controllerUrl = "https://fraudidentapi.azurewebsites.net/FraudIdent";
  }
  
  public getTruckModels(){
    return this.http.get<AppTruckModel[]>(this.controllerUrl + "/getTrucks");
  }

  public getBalanceInfos(){
    return this.http.get<AppBanceInfoModel>(this.controllerUrl + "/getBalanceInfos");
  }

  public getLobInfo(truckId: number, viewDistance: Boolean){
    var params: String = "";

    params += `IsDistanceImage=${viewDistance.toString()}`;

    if(truckId > 0){
      params += `&truckId=${truckId.toString()}`;
    }
    

    return this.http.get<AppLobInfoModel[]>(this.controllerUrl + "/getLastLobInfo" + (params != "" ? "?" + params : ""));
  }

  public saveInfos(data: string){   

    const httpOptions = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }

    return this.http.post(this.controllerUrl + "/saveConfigurations", data, httpOptions);
  }

  public changeTruckSelected(truckId: number){   
    var data: String = `${truckId.toString()}`;

    const httpOptions = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }

    return this.http.post(this.controllerUrl + "/changeTruckSelected", JSON.stringify(data), httpOptions);
  }
}
