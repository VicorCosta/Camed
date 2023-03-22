import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService, VinculoBNB } from 'src/app/core';

@Injectable()
export class AgendamentoDeLigacaoServices extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'agendamentodeligacao');
  }

  getAll(query: string) {
    return this.get(query);
  }

  save(agendamentodeligacao) {
    if (agendamentodeligacao.id === 0) {
      return this.post(agendamentodeligacao);
    } else {
      return this.put(agendamentodeligacao);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
