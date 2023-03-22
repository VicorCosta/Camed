import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService, MotivoRecusa } from 'src/app/core';

@Injectable()
export class MotivoRecusaService extends ApiBaseService  {
  constructor(http: HttpClient) {
    super(http, 'MotivoRecusa');
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
