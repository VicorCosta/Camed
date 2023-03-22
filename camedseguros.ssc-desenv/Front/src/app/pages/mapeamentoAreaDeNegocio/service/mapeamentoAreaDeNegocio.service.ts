import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class MapeamentoAreaDeNegocioService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'MapeamentoAreaDeNegocio');
  }

  getAll(query: string = '') {
    return this.get(`$expand=tipoDeAgencia,tipoDeSeguro,tipoDeProduto,vinculoBNB,areaDeNegocio&${query}`);
  }

  deletar(id: number) {
    return this.delete(id);
  }



  getMapeamentoSelecionado(id){
    const filtro = `$expand=tipoDeAgencia,tipoDeSeguro,tipoDeProduto,vinculoBNB,areaDeNegocio&$filter=id eq ${id}`;
    return this.get(filtro);
  }


 save(data) {
   if (data.id === 0) {
     return this.post(data);
   } else {
     return this.put(data);
   }
 }

}
