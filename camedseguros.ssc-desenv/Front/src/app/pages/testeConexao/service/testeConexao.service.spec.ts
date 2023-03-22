import { TestBed } from '@angular/core/testing';

import { testeConexao } from './testeConexao';

describe('TesteConexao', () => {
  let service: testeConexao;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(testeConexao);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
