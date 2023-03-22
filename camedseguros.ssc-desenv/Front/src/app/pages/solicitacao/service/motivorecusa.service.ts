
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService, Solicitacao, Result } from 'src/app/core';
import { environment } from 'src/environments/environment';
import { observable, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class MotivoRecusaService extends ApiBaseService  {
  constructor(http: HttpClient) {
    super(http, 'MotivoRecusa');
  }

  getAll(query: string = '') {
    return this.get(query);
  }
}
