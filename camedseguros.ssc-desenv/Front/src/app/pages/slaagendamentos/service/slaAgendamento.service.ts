import { Injectable } from '@angular/core';
import { ApiBaseService} from 'src/app/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class SLAAgendamentoService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'slaagendamento');
  }

  getAll(query: string = '') {
    return this.get(`${query}`);
  }
   

  save(slaagendamento) {
    if (slaagendamento.id === 0) {
      return this.post(slaagendamento);
    } else {
      return this.put(slaagendamento);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
