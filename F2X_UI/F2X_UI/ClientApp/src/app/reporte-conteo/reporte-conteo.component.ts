import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ConsumoServiceService } from '../Models/consumo-service.service';

@Component({
  selector: 'app-reporte-conteo',
  templateUrl: './reporte-conteo.component.html'
})
export class ReporteConteoComponent implements OnInit {
  

  constructor(public service: ConsumoServiceService) { }

  ngOnInit():void {
    this.service.FechaConsultada = formatDate(new Date(), 'yyyy-MM-dd', 'en_US');
    this.service.getConteo();
    this.service.getRecaudo();
  }

  public onDate(event): void {
    this.service.FechaConsultada = formatDate(event.value, 'yyyy-MM-dd', 'en_US');
    this.service.getConteo();
    this.service.getRecaudo();
  }
  
}
