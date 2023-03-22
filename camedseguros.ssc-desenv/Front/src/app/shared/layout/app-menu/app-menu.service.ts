import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import _ from 'underscore';
import { ValidacaoCargoService } from 'src/app/service/validacao-cargo.service';



@Injectable({ providedIn: 'root' })
export class AppMenuService {

  private menusSubject = new BehaviorSubject<any[]>(null);
  private menuAtivoSubject = new BehaviorSubject<any>(null);
  private displayHelperSubject = new BehaviorSubject<boolean>(false);

  public menus = this.menusSubject.asObservable();
  public menuAtivo = this.menuAtivoSubject.asObservable();
  public displayHelper = this.displayHelperSubject.asObservable();

  isGerente: boolean;

  constructor(private http: HttpClient, private router: Router, private cargoService: ValidacaoCargoService) {
    this.cargoService.setarCargos()
    this.cargoService.isGerente.subscribe(result => {
      this.isGerente = result;
    })
    this.getMenus();
  }

  setMenuAtivo(menu: any): void {
    this.menuAtivoSubject.next(menu);
  }

  setDisplayHelper() {
    this.displayHelperSubject.next(true);
  }

  getAppMenus(): any[] {
    return this.menusSubject.value;
  }

  private getMenus(): void {
    setTimeout(() => {
      let queryValidation = `/odata/menu?$expand=superior,submenus&$filter=superior eq null`
      this.http.get(`${environment.api_url}${queryValidation}`)
        .pipe(map((data: any) => {
          return data.value.map((mn) => {
            return {
              ...mn,
              active: (this.router.url.includes(mn.rota) ||
                mn.submenus.find(sm => this.router.url.includes(sm.rota)) !== undefined),
              submenus: mn.submenus.map(sm => {
                return {
                  ...sm,
                  active: this.router.url.includes(sm.rota),
                  superior: { label: mn.label }
                };
              }) || []
            };
          }) || [];
        }))
        .subscribe(data => {
          this.menusSubject.next(data);

          const ativo = _(data).find((mn) => this.router.url.includes(mn.rota)) ||
            _(data).chain().pluck('submenus').flatten().find((mn) => this.router.url.includes(mn.rota)).value();

          this.menuAtivoSubject.next(ativo);
        });

    }, 500)
  }
}
