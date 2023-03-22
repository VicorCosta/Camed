import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { distinctUntilChanged } from 'rxjs/operators';

import { JwtService } from './jwt.service';
import { User, Identity } from '../models';

import { ApiBaseService } from './engine/api.base.service';
import { Result } from '../util';


@Injectable()
export class AuthenticationService extends ApiBaseService {
  private currentUserSubject: BehaviorSubject<User>;
  public loggedUser: Observable<User>;

  constructor(
    private jwtService: JwtService,
    http: HttpClient
  ) {
    super(http, 'login');

    this.currentUserSubject = new BehaviorSubject<User>(jwtService.getCurrentUser());
    this.loggedUser = this.currentUserSubject.asObservable().pipe(distinctUntilChanged());
  }

  setAuth(identity: Identity) {
    this.currentUserSubject.next(identity.user);
    this.jwtService.save(identity);
  }

  login(credentials): Observable<Result> {
    return this.post(credentials);
  }

  unauthorize() {
    this.currentUserSubject.next(null);
    this.jwtService.destroy();
  }

  getLoggedUser(): User {
    return this.currentUserSubject.value;
  }

  isAuthenticated(): boolean {
    return this.getLoggedUser() !== null && this.jwtService.tokenHasExpired() === false;
  }

  isAuthorized(): boolean {
    return true;
    // this.jwtService.getAccess()
  }

}
