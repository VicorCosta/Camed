import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class SituacaoService extends ApiBaseService  {
  constructor(http: HttpClient) {
    super(http, 'Situacao');
  }

  getAll(query: string) {
    return this.get(query);
  }

  save(situacao) {
    if (situacao.id === 0) {
      return this.post(situacao);
    } else {
      return this.put(situacao);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
