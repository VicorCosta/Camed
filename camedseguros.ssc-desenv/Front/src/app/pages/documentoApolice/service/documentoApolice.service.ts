import { Injectable } from '@angular/core';
import { ApiBaseService, Result } from 'src/app/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class DocumentoApoliceService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'apoliceGS');
  }

  obterDocumentos(agencia: string, cpfcgc: string, tipoSolicitacao: string): Observable<Result> {
    return this.http.get(`${environment.api_url}/api/${this.endpoint}/consultar`, { params: { agencia, cpfcgc, tipoSolicitacao } })
        .pipe(map( response => response as Result));
  }

  downloadDocumento(docCaminho) {
    let headers = new HttpHeaders();
    headers = headers.set('Accept', 'application/pdf');
    return this.http.get(`${environment.api_url}/api/${this.endpoint}?caminho=${docCaminho}`, {headers: headers, responseType: 'blob'});
  }
}
