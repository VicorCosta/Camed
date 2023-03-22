import { Injectable } from '@angular/core';
import { ApiBaseService } from 'src/app/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AuditoriaDetalhesService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'AuditLogDetail');
  }

  getAll(query: string = '') {
    return this.get(query);
  }

}
