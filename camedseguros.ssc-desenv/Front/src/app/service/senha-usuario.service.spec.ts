import { TestBed } from '@angular/core/testing';

import { SenhaUsuarioService } from './senha-usuario.service';

describe('SenhaUsuarioService', () => {
  let service: SenhaUsuarioService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SenhaUsuarioService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
