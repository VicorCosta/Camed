import { Injectable } from '@angular/core';
import { ApiBaseService} from 'src/app/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class VariaveisDeEmailService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'variaveisdeemail');
  }

  getAll(query: string = '') {
    return this.get(query);
  }

  save(variaveisdeemail) {
    if (variaveisdeemail.id === 0) {
      return this.post(variaveisdeemail);
    } else {
      return this.put(variaveisdeemail);
    }
  }

  deletar(id: number) {
    return this.delete(id);
  }
}
