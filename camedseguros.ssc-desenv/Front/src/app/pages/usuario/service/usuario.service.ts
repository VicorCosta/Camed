import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService, VinculoBNB } from 'src/app/core';

@Injectable()
export class UsuarioService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'usuario');
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
