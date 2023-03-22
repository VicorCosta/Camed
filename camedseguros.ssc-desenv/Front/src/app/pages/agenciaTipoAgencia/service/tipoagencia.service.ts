import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class AgenciaTipoService extends ApiBaseService  {
  constructor(http: HttpClient) {
    super(http, 'AgenciaTipoDeAgencia');
  }

  getAll(query: string) {
    return this.get('$expand=agencia,tipodeagencia&'+query);
  }

  save(request) {
    if (request.Id === 0) {
      return this.post(request);
    } else {
      return this.put(request);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
