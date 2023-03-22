import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class MenuService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'menu');
  }

  getAll(query: string = '') {
    return this.get(`${query}&$expand=Superior`);
  }

  // getAcoesByFrame(id){
  //   const filtro = '$expand=AcoesAcompanhamento&$filter=id%20eq%20'+id;
  //   return this.get(filtro);
  // }

  save(request) {
    if (request.id === 0) {
      return this.post(request);
    } else {
      return this.put(request);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }

}
