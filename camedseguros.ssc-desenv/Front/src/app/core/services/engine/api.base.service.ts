import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { Result } from '../../util';
import { Inject, Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable()
export class ApiBaseService {


  constructor(protected http: HttpClient, @Inject(String) protected endpoint: string) {
  }


  fetch(query: string = '', odataFormat = true): any {
    return this.http.get(`${environment.api_url}/${odataFormat ? 'odata' : 'api'}/${this.endpoint}?${query}`);
  }

  get(query: string = '', odataFormat = true): Observable<any> {
    return this.http.get(`${environment.api_url}/${odataFormat ? 'odata' : 'api'}/${this.endpoint}?${query}`);
  }

  put(body: object = {}): Observable<Result> {
    return this.http.put(`${environment.api_url}/api/${this.endpoint}/`, JSON.stringify(body))
      .pipe(map(data => data as Result));
  }

  post(body: object = {}): Observable<Result> {
    return this.http.post(`${environment.api_url}/api/${this.endpoint}/`, JSON.stringify(body))
      .pipe(map(data => data as Result));
  }

  delete(id: number): Observable<Result> {
    return this.http.delete(`${environment.api_url}/api/${this.endpoint}/?id=${id}`)
      .pipe(map(data => data as Result));
  }

}
