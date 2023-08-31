import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { VideoScreenComponent } from './video-screen/video-screen.component';
import { ControlPanelComponent } from './control-panel/control-panel.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { FraudIdentComponent } from './fraud-ident/fraud-ident.component';
import { PastFraudIdentComponent } from './past-fraud-ident/past-fraud-ident.component';
import { EditRegisterTruckComponent } from './edit-register-truck/edit-register-truck.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    VideoScreenComponent,
    ControlPanelComponent,
    FraudIdentComponent,
    PastFraudIdentComponent,
    EditRegisterTruckComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

