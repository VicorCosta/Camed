import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class RamosService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'TipoDeProduto');
  }

  getAll(query: string = '') {
    return this.get('$expand=Situacao,SituacaoRenovacao&'+query);
  }

  getSituacoesByRamo(id){
    const filtro = `$expand=Situacao,SituacaoRenovacao&$filter=id eq ${id}`;
    return this.get(filtro);
  }

  save(ramo) {
    if (ramo.id === 0) {
      return this.post(ramo);
    } else {
      return this.put(ramo);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }


}
