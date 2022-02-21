import { TestBed } from '@angular/core/testing';

import { ConsumoServiceService } from './consumo-service.service';

describe('ConsumoServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ConsumoServiceService = TestBed.get(ConsumoServiceService);
    expect(service).toBeTruthy();
  });
});
