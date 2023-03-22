import { TestBed } from '@angular/core/testing';

import { TipomorteService } from './tipomorte.service';

describe('TipomorteService', () => {
  let service: TipomorteService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TipomorteService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
