import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { HomeComponent } from './home/home.component';
import { FraudIdentComponent } from "./fraud-ident/fraud-ident.component";
import { PastFraudIdentComponent } from "./past-fraud-ident/past-fraud-ident.component";
import { EditRegisterTruckComponent } from "./edit-register-truck/edit-register-truck.component";

const appRoute: Routes = [
    {path: '', component: HomeComponent},
    // {path: '', redirectTo: 'Home', pathMatch: 'full'},
    {path: 'Home', component: HomeComponent},
    {path: 'FraudIdent', component: FraudIdentComponent},
    {path: 'PastFraudIdent', component: PastFraudIdentComponent},
    {path: 'EditRegisterTruck', component: EditRegisterTruckComponent}    
  ];

  
@NgModule({
    imports: [
        RouterModule.forRoot(appRoute, {enableTracing: true})
    ],
    exports: [
        RouterModule
    ]
})
export class AppRoutingModule{

}