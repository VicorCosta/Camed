import { Injectable } from "@angular/core";
import { ApiBaseService, Grupo } from "src/app/core";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class GrupoService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, "grupo");
  }

  getAll(query: string = "") {
    return this.get("$expand=menus,subgrupos&" + query);
  }

  save(grupo) {
    if (grupo.id === 0) {
      return this.post(grupo);
    } else {
      return this.put(grupo);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
