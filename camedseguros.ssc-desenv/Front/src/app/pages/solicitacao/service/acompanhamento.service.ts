import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService, Solicitacao, Result } from 'src/app/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class AcompanhamentoService extends ApiBaseService  {
  constructor(http: HttpClient) {
    super(http, 'Acompanhamento');
  }

  getAll(query: string) {
    return this.get(query);
  }

  save(acaodeacompanhamento) {
    if (acaodeacompanhamento.id === 0) {
      return this.post(acaodeacompanhamento);
    } else {
      return this.put(acaodeacompanhamento);
    }
  }
}
