import { Injectable } from '@angular/core';
import { ApiBaseService, AreaDeNegocio } from 'src/app/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AreaDeNegocioService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'areadenegocio');
  }

  getAll(query: string = '') {
    return this.get(query);
  }

  save(areadenegocio) {
    if (areadenegocio.id === 0) {
      return this.post(areadenegocio);
    } else {
      return this.put(areadenegocio);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
