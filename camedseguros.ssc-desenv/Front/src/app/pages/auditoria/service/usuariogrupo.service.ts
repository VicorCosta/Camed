import { Injectable } from '@angular/core';
import { ApiBaseService } from 'src/app/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class UsuarioGrupo extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'UsuarioGrupo');
  }

  getAll(query: string = '') {
    return this.get(query);
  }

}
