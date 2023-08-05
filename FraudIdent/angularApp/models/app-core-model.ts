import { AppBanceInfoModel } from "./app-balance-info-model";
import { AppTruckModel } from "./app-truck-model";

export class AppCoreModule{
    ManualDataInsert: boolean = false;

    TruckId: number = 0;
    Truck: AppTruckModel = new AppTruckModel();

    BalanceInfo: AppBanceInfoModel = new AppBanceInfoModel();
}