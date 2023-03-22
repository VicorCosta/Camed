import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';
import { Empresas } from 'src/app/core/';

@Injectable()
export class TipodeParametroService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'tiposdeparametros');
  }

  getAll(query: string) {
    return this.get(query);
  }

  getSegurobyEmpresa(id){
    return this.get('$expand=tiposDeSeguro&$filter=id%20eq%20'+id)
  }

  save(tiposdeparametros) {
    if (tiposdeparametros.id === 0) {
      return this.post(tiposdeparametros);
    } else {
      return this.put(tiposdeparametros);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
