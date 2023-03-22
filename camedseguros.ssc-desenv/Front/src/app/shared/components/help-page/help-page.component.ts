import { Component, OnInit } from '@angular/core';
import { AppMenuService } from '../../layout';
import { Router } from '@angular/router';
import _ from 'underscore';


@Component({
  selector: 'app-help-page',
  templateUrl: 'help-page.component.html',
})
export class HelpPageComponent implements OnInit {

  menuAtivo: any;
  display: boolean;

  constructor(private menuService: AppMenuService, private router: Router) { }

  ngOnInit() {
    this.menuService.menuAtivo.subscribe(mAtivo => this.menuAtivo = mAtivo);
    this.menuService.displayHelper.subscribe(dsp => this.display = dsp);
  }

}
