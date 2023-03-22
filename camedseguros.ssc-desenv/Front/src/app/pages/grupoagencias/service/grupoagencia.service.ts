import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService, GrupoAgencia } from 'src/app/core';

@Injectable()
export class GrupoAgenciaService extends ApiBaseService  {
  constructor(http: HttpClient) {
    super(http, 'grupoagencia');
  }

  getAll(query: string = '') {
    return this.get(query);
  }

  save(grupo) {
    if (grupo.id === 0) {
      return this.post(grupo);
    } else {
      return this.put(grupo);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
