import { Injectable } from '@angular/core';
import { AnexoInbox, ApiBaseService, Inbox, Result } from 'src/app/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';


@Injectable()
export class InboxService extends ApiBaseService {

  updateAsRead(body: any): Observable<Result>{
    return this.http.put(`${environment.api_url}/api/${this.endpoint}/atualizarLida`, JSON.stringify(body))
    .pipe(map(data => data as Result));
  }

  constructor(http: HttpClient) {
    super(http, 'inbox');
  }

  getAll(query: string = '') {
    return this.get(query);
  }

  save(inbox) {
    return this.post(inbox);
  }

  deletar(id: number) {
    return this.delete(id);
  }

  formatFile(post: File[]): Observable<Result>{
    const formData: FormData = new FormData();
    for(let file of post){
      formData.append('anexos', file);
    }
    return this.http.post(`${environment.api_url}/api/${this.endpoint}/FormatarArquivo`, formData)
    .pipe(map(data => data as Result));
  }

  markAsRead(body: any): Observable<Result>{
    return this.http.patch(`${environment.api_url}/api/${this.endpoint}/marcarMensagemComoLida`, JSON.stringify(body))
    .pipe(map(data => data as Result));
  }

  downloadFile(post: any){
    return this.http.post(`${environment.api_url}/api/${this.endpoint}/Download`, JSON.stringify(post))
  }
}
