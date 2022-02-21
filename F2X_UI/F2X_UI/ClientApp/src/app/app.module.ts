import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReporteConteoComponent } from './reporte-conteo/reporte-conteo.component';
import { MatDatepickerModule, MatNativeDateModule, MatInputModule } from '@angular/material';
import { ConsumoServiceService } from './Models/consumo-service.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ReporteConteoComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    MatInputModule,
    ApiAuthorizationModule,        // <----- this module will be deprecated in the future version.
    MatDatepickerModule,        // <----- import(must)
    MatNativeDateModule,  
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
    ]),
    BrowserAnimationsModule,
    
  ],
  providers: [ConsumoServiceService],
  bootstrap: [AppComponent]
})
export class AppModule { }
