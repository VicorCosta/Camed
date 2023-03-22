import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class FeriadoService extends ApiBaseService  {
  constructor(http: HttpClient) {
    super(http, 'feriado');
  }

  getAll(query: string) {
    return this.get('$expand=municipio&'+query);
  }

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
