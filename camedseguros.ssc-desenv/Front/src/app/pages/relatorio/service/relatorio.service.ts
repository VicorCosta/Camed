import { Injectable } from '@angular/core';
import { ApiBaseService, Result } from 'src/app/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class RelatorioService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'Relatorio');
  }

  getAll(query: string = '') {
    return this.get(query);
  }

  getRelatorio(relatorio: JSON): Observable<any> {
    let params = JSON.stringify(relatorio);

    return this.http.get(`${environment.api_url}/api/${this.endpoint}?filtros=${params}`);
  }

}
