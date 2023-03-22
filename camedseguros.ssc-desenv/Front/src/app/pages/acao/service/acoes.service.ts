import { Injectable } from '@angular/core';
import { ApiBaseService, AcaoDeAcompanhamento } from 'src/app/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AcaoService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'acao');
  }

  getAll(query: string = '') {
    return this.get(query);
  }

  save(acao) {
    if (acao.id === 0) {
      return this.post(acao);
    } else {
      return this.put(acao);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
