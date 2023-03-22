import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';

@Injectable()
export class TipoMorteService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'TipoMorte');
  }

  getAllTipoMorte(query: string = '') {
    return this.get(`${query}&$select=id,descricao&$orderby=descricao`);
  }

  getTipoMorte(id){
    const filtro = `$filter=id eq ${id}`;
    return this.get(filtro);
  }

}
