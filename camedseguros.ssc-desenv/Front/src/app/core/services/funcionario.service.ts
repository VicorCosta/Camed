import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from './engine';

@Injectable()
export class FuncionarioService extends ApiBaseService  {
  constructor(http: HttpClient) {
    super(http, 'Funcionario');
  }

  getAll(query: string) {
    return this.get(query);
  }
}
