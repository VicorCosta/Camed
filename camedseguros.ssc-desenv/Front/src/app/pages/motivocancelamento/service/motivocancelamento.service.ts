import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService, MotivoCancelamento } from 'src/app/core';

@Injectable()
export class MotivoCancelamentoService extends ApiBaseService  {
  constructor(http: HttpClient) {
    super(http, 'MotivoEndossoCancelamento');
  }

  getAll(query: string) {
    return this.get(query);
  }

  save(motivo) {
    if (motivo.id === 0) {
      return this.post(motivo);
    } else {
      return this.put(motivo);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
