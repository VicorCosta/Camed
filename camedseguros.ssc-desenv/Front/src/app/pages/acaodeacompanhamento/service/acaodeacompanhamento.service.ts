import { Injectable } from '@angular/core';
import { ApiBaseService, AcaoDeAcompanhamento } from 'src/app/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AcaoDeAcompanhamentoService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'acaodeacompanhamento');
  }

  getAll(query: string = '') {
    return this.get(query);
  }

  save(acaodeacompanhamento) {
    if (acaodeacompanhamento.id === 0) {
      return this.post(acaodeacompanhamento);
    } else {
      return this.put(acaodeacompanhamento);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
