import { Injectable } from '@angular/core';
import { ApiBaseService } from 'src/app/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AuditoriaService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'Auditoria');
  }

  getAll(query: string = '') {
    return this.get(query);
    /*return this.get(query +'&$expand=AuditoriaDetalhes&$select=id,tpReg,eventTime,userName,eventType,tableName,chave,numeroDaSolicitacao,message,details');*/
  }

}
