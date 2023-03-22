import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ApiBaseService } from "src/app/core";

@Injectable()
export class TipoDeDocumentoService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, "tipoDeDocumento");
  }

  getAll(query: string) {
    return this.get(query);
  }

  getTiposDeProdutoByDocumento(id) {
    const filtro = `$expand=tiposDeProdutoMorte&$filter=id eq ${id}`;
    return this.get(filtro);
  }

  save(frames) {
    if (frames.id === 0) {
      return this.post(frames);
    } else {
      return this.put(frames);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
