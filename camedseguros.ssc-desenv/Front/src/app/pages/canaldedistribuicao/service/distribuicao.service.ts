import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core/services';
import { CanalDeDistribuicao } from 'src/app/core';

@Injectable()
export class DistribuicaoService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'CanalDeDistribuicao');
  }

  getAll(query: string = '') {
    return this.get(query);
  }

  save(canal) {
    if (canal.id === 0) {
      return this.post(canal);
    } else {
      return this.put(canal);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
