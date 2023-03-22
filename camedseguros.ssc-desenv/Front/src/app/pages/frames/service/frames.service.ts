import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from 'src/app/core';
import { Frames } from 'src/app/core/';

@Injectable({
  providedIn: 'root'
})
export class FramesService extends ApiBaseService {
  constructor(http: HttpClient) {
    super(http, 'frame');
  }

  getAll(query: string) {
    return this.get(query);
  }

  getAcoesByFrame(id){
    const filtro = '$expand=AcoesAcompanhamento&$filter=id%20eq%20'+id;
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
