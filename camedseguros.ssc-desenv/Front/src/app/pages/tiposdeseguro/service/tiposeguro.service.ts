import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';
import { TiposSeguro } from 'src/app/core/';

@Injectable()
export class TipoSeguroService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'TipoDeSeguro');
  }

  getAll(query: string = '') {
    return this.get('$expand=GrupoAgencia&'+query);
  }

  getAgenciaRamoByTipoSeguro (id: number){
    return this.get('$expand=tiposDeProduto,GrupoAgencia&$filter=id%20eq%20'+id)
  }

  save(seguro) {
    if (seguro.id === 0) {
      return this.post(seguro);
    } else {
      return this.put(seguro);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }


}
