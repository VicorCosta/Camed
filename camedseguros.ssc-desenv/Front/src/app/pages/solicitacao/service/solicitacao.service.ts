
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService, Solicitacao, Result } from 'src/app/core';
import { environment } from 'src/environments/environment';
import { observable, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class SolicitacaoService extends ApiBaseService  {
  constructor(http: HttpClient) {
    super(http, 'Solicitacao');
  }

  getAll(query: string = '') {
    return this.get(query);
  }

  getSolicitcaoByAtendente(atendente){
    const filtro = '$expand=Solicitante&$filter=atendente%20eq%20' + atendente;
    return this.get(filtro)
  }

  save(post) {
    if (post.id === 0) {
      return this.post(post);
    } else {
      return this.put(post);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }

  formatFile(solicitacaoId:number, post: any[]): Observable<Result>{
    const formData: FormData = new FormData();
    for(let file of post){
      formData.append('files', file);
    }
    return this.http.post(`${environment.api_url}/api/${this.endpoint}/FormatarArquivo?solicitacaoId=${solicitacaoId}`, formData)
    .pipe(map(data => data as Result));
  }

  salvarAtendente(post:any){
    return this.http.post(`${environment.api_url}/api/${this.endpoint}/SalvarAtendente`, JSON.stringify(post))
  }

  downloadFile(post: any){
    return this.http.post(`${environment.api_url}/api/${this.endpoint}/Download`, JSON.stringify(post))
  }

  inserirAcompanhamento(post: any){
    return this.http.post(`${environment.api_url}/api/${this.endpoint}/InserirAcompanhamento`, JSON.stringify(post))
  }

  novoAcompanhamento(acaoNovoAcompanhamento: { solicitacao_Id: any; acao: any; }) {
    return this.http.post(`${environment.api_url}/api/${this.endpoint}/AdicionarAcompanhamento`, JSON.stringify(acaoNovoAcompanhamento))
  }
}
