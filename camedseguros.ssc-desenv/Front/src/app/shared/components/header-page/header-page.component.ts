import { Component, OnInit } from '@angular/core';
import { AppMenuService } from '../../layout';
import { Router } from '@angular/router';
import _ from 'underscore';


@Component({
  selector: 'app-header-page',
  templateUrl: 'header-page.component.html',
})
export class HeaderPageComponent implements OnInit {

  menuAtivo: any;

  constructor(private menuService: AppMenuService, private router: Router) { }

  ngOnInit() {
    this.menuService.menuAtivo.subscribe(mAtivo => this.menuAtivo = mAtivo);
  }

  showHelper() {
    this.menuService.setDisplayHelper();
  }
}
