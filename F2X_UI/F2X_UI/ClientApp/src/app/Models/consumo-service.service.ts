import { Injectable } from '@angular/core';
import {RequestCollection} from './request-collection';
import {RequestCounter} from './request-counter';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConsumoServiceService {

  FechaConsultada: string;
  readonly rootURL = 'https://localhost:44386/api/Values/';

  conteo : RequestCounter[];
  recaudo : RequestCollection[];

  constructor(private http: HttpClient) { }

  getConteo() {
    
    var reqHeader = new HttpHeaders({
      'ApiKey': 'ApiKeySecret',
      'Access-Control-Allow-Origin': 'https://localhost:44355',
      'Access-Control-Allow-Credentials': 'true'
    });
    
    this.http.get(this.rootURL + 'Conteo/' + this.FechaConsultada, { headers: reqHeader }).toPromise()
      .then(res => this.conteo = res as RequestCounter[])
      .catch((err: HttpErrorResponse) => {
        console.error('An error occurred:', err.error);
      });
  }
  
  getRecaudo() {

    var reqHeader = new HttpHeaders({
      'ApiKey': 'ApiKeySecret',
      'Access-Control-Allow-Origin': 'https://localhost:44355',
      'Access-Control-Allow-Credentials': 'true'
    });

    this.http.get(this.rootURL + 'Recaudo/' + this.FechaConsultada, { headers: reqHeader }).toPromise()
      .then(res => this.conteo = res as RequestCounter[])
      .catch((err: HttpErrorResponse) => {
        console.error('An error occurred:', err.error);
      });
  }
}
