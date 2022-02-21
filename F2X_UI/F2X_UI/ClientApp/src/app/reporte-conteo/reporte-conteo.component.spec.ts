import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReporteConteoComponent } from './reporte-conteo.component';

describe('ReporteConteoComponent', () => {
  let component: ReporteConteoComponent;
  let fixture: ComponentFixture<ReporteConteoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReporteConteoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReporteConteoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
