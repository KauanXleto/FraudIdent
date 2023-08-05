import { AppTruckParamModel } from "./app-truck-param-model";

export class AppTruckModel{
    Id: number = 0;
    Name: string = "";
    TruckParam: AppTruckParamModel = new AppTruckParamModel();
}