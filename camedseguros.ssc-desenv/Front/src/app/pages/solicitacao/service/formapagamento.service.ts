import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService, Solicitacao, Result } from 'src/app/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class FormaPagamentoService extends ApiBaseService  {
  constructor(http: HttpClient) {
    super(http, 'FormaDePagamento');
  }

  getAll(query: string ) {
    return this.get(query);
  }

}
