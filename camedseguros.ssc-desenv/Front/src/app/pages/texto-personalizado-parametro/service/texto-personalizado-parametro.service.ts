import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class TextoPersonalizadoParametroService extends ApiBaseService {
 constructor(http: HttpClient) {
  super(http, 'TextoParametrosSistema');
}

getAll(query: string = ' ') {
  return this.get(`$expand=tipoDeSeguro,tipoDeProduto&${query}`);
}

deletar(id: number) {
  return this.delete(id);
}

save(data) {
 if (data.id === 0) {
   return this.post(data);
 } else {
   return this.put(data);
 }
}


}
