import { ActivatedRoute, Router } from "@angular/router";

import { BehaviorSubject, Observable } from "rxjs";
import { distinctUntilChanged, take } from "rxjs/operators";
import { FormGroup, FormBuilder } from "@angular/forms";

import _ from "underscore";

import { Result } from "../util";
import { AuthenticationService } from "../services";

export class BaseComponent {
  private resultSubject$: BehaviorSubject<Result>;

  public result$: Observable<Result>;
  public form: FormGroup;
  public loading: boolean;
  public alertError: string;
  public appReady: boolean = false;

  constructor(
    public authenticationService: AuthenticationService,
    public fb: FormBuilder,
    public route: ActivatedRoute,
    public router: Router
  ) {
    this.resultSubject$ = new BehaviorSubject<Result>({} as Result);
    this.result$ = this.resultSubject$
      .asObservable()
      .pipe(distinctUntilChanged());

    this.loading = false;

    if (!this.authenticationService.isAuthenticated()) {
      this.router.navigate(["login"]);
      return;
    }
  }

  //Função criada para gerar um "delay" nos formulários que necessitam de dados
  //requisitados em API.
  //Garante que o formulário só carregue após todos os dados dos quais ele depende
  //sejam carregados;
  delay(ms: number) {
    return new Promise((resolve) => {
      setTimeout(resolve, ms);
    });
  }

  get f() {
    return this.form.controls;
  }

  setResult(result: Result): void {
    this.loading = false;

    result.validators = _.groupBy(result.notifications, (n) => n.key);
    this.resultSubject$.next(result);
  }

  invalid(prop: string): boolean {
    const result = this.resultSubject$.value as Result;

if(result.validators != undefined)

    if (result.validators) {
      if (result.validators[prop]) {
        return true;
      } else {
        return false;
      }
    }
    return false;
  }

  showError(msg: any): void {
    if (typeof msg === "string") {
      this.alertError = msg;
    }

    if (typeof msg === "object") {
      this.alertError = msg.error.message;
    }

    this.loading = false;
  }
}
