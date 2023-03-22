import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class TipomorteService extends ApiBaseService {

  constructor(http: HttpClient) {
    super(http, 'tipomorte');
  }

  getAll(query: string = '') {
    return this.get(query);
  }
  
  save(tipomorte) {
    if (tipomorte.id === 0) {
      return this.post(tipomorte);
    } else {
      return this.put(tipomorte);
    }
  }
}
