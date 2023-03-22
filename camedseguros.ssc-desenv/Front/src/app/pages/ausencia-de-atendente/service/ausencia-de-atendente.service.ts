import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService, VinculoBNB } from 'src/app/core';

@Injectable()
export class AusenciaDeAtendenteServices extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'ausenciaatendente');
  }

  getAll(query: string) {
    return this.get(query);
  }

  save(ausenciaatendente) {
    if (ausenciaatendente.id === 0) {
      return this.post(ausenciaatendente);
    } else {
      return this.put(ausenciaatendente);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
