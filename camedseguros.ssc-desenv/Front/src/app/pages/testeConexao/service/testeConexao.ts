import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class testeConexao extends ApiBaseService {

  constructor(http: HttpClient) {
    super(http, 'TesteConexao');
  }

  send(testeConexao) {
    return this.post(testeConexao);
  }
}
