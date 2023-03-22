import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from './engine';

@Injectable()
export class TipoProdutoService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'tipodeproduto');
  }

  getAll(query: string) {
    return this.get(query);
  }
}
