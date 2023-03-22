import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService, VinculoBNB } from 'src/app/core';

@Injectable()
export class MapeamentoDeatendenteServices extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'mapeamentoatendente');
  }

  getAll(query: string ="") {
    return this.get(query);
  }

  save(mapeamentoatendente) {
    if (mapeamentoatendente.id === 0) {
      return this.post(mapeamentoatendente);
    } else {
      return this.put(mapeamentoatendente);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
