import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService, RetornoLigacao} from 'src/app/core';

@Injectable()
export class RetonoligacaoService extends ApiBaseService  {
  constructor(http: HttpClient) {
    super(http, 'TipoRetornoLigacao');
  }

  getAll(query: string) {
    return this.get(query);
  }

  save(retorno) {
    if (retorno.id === 0) {
      return this.post(retorno);
    } else {
      return this.put(retorno);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
