import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class SenhaUsuarioService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'AlteracaoSenhaUsuario');
  }

  save(data) {
      return this.put(data);
    }
  }

  /* getAll(query: string = '') {
    return this.get('$expand=Situacao,SituacaoRenovacao&'+query);
  }

  getSituacoesByRamo(id){
    const filtro = `$expand=Situacao,SituacaoRenovacao&$filter=id eq ${id}`;
    return this.get(filtro);
  }


  deletar(id: number) {
    return this.delete(id);
  } */


