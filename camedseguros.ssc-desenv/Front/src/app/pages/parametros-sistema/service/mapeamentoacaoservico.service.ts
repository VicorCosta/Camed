import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class MapeamentoAcaoSituacaoService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'MapeamentoAcaoSituacao');
  }

  getAll(query: string) {
    return this.get(`${query}`);
  }
}
