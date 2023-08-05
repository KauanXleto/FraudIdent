export class AppLobInfoModel{
    Id: number = 0;
    TruckId: number = 0;

    HasError: boolean | undefined;
    HasSuccess: boolean | undefined;
    MessageError: string | undefined;

    BackImageTruck: string | undefined;
    SideImageTruck: string | undefined;
}