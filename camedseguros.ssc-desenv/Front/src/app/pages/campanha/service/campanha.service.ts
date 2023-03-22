import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core/services';
import { CanalDeDistribuicao } from 'src/app/core';

@Injectable()
export class CampanhaService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'campanha');
  }

  getAll(query: string = '') {
    return this.get(query);
  }

  save(campanha) {
    if (campanha.id === 0) {
      return this.post(campanha);
    } else {
      return this.put(campanha);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
