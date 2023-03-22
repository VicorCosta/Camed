import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';
import { Empresas } from 'src/app/core/';

@Injectable()
export class EmpresaService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'empresa');
  }

  getAll(query: string) {
    return this.get(query);
  }

  getSegurobyEmpresa(id){
    return this.get('$expand=tiposDeSeguro&$filter=id%20eq%20'+id)
  }

  save(empresa) {
    if (empresa.id === 0) {
      return this.post(empresa);
    } else {
      return this.put(empresa);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }


}
