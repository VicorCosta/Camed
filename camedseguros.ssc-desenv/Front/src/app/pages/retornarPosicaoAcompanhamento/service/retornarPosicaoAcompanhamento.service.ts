import { Injectable } from '@angular/core';
import { ApiBaseService, Result } from 'src/app/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class RetornoDaPosicaoDeAcompanhamentoAtualizarSolicitacaoeService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'RetornoDaPosicaoDeAcompanhamentoAtualizarSolicitacao');
  }
// RetornoDaPosicaoDeAcompanhamentoAtualizarSolicitacao?Solicitacao_Id = 489151 & DataEHora=19% 2F11 % 2F2021 % 2014 % 3A56 % 3A46 & Situacao_Id=180
  processar(solicitacao: number, dataEHora: string, situacaoId: string, ordem: number): Observable<Result> {
    return this.http.post(`${environment.api_url}/api/${this.endpoint}?Solicitacao_Id=${solicitacao}&DataEHora=${dataEHora}&Situacao_Id=${situacaoId}`, { params:{} })
        .pipe(map( response => response as Result));
  }

  deletar(solicitacao: number, ordem: number): Observable<Result> {
    return this.http.delete(`${environment.api_url}/api/${this.endpoint}?Solicitacao_Id=${solicitacao}&ordem=${ordem}`, { params: {} })
      .pipe(map(response => response as Result));
  }



  getAcompanhamento(query: string): Observable<Result> {
    return this.http.get(`${environment.api_url}/api/Acompanhamento?${query}`, { params: {} })
      .pipe(map(response => response as Result));
  }
}
