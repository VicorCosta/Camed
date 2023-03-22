import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiBaseService } from './engine';

@Injectable()
export class VinculoBNBService extends ApiBaseService {

  constructor(http: HttpClient) {
    super(http, 'VinculoBNB');
  }
}
