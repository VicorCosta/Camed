import { Injectable } from "@angular/core";
import { ApiBaseService } from "src/app/core";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class CotacaoSombreroService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, "cotacaosombrero");
  }

  getAll(query: string = "") {
    return this.get(query);
  }

  save(cotacaosombrero) {
    if (cotacaosombrero.id == 0) {
      return this.post(cotacaosombrero);
    } else {
      return this.put(cotacaosombrero);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
