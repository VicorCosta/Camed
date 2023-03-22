import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from './engine';



@Injectable()
export class ParametrosSistemaService extends ApiBaseService {

  constructor(http: HttpClient) {
    super(http, 'parametrossistema');
  }

  getAll(query: string = '') {
    return this.get(query);
  }
}
