import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class ParametrosSistemaService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'parametrossistema');
  }

  getAll(query: string) {
    return this.get(`$expand=tipoDeParametro,variaveisDeEmail&${query}`);
  }
  
  save(data) {
    if (data.id === 0) {
      return this.post(data);
    } else {
      return this.put(data);
    }
   }

  // save(parametrossistema) {
  //   if (parametrossistema.id === 0) {
  //     return this.post(parametrossistema);
  //   } else {
  //     return this.put(parametrossistema);
  //   }
  // }

  deletar(id: number) {
    return this.delete(id);
  }
}
