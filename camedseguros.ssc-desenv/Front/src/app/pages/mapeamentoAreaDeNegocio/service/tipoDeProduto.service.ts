import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ApiBaseService } from "src/app/core";

@Injectable()
export class TipoDeProdutoService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, "TipoDeProduto");
  }

  getAll(query: string) {
    return this.get(`&${query}`);
  }
}
