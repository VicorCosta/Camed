import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { AppMenuService } from './app-menu.service';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

declare var $: any;

@Component({
  selector: 'app-menu',
  templateUrl: 'app-menu.component.html',
})
export class AppMenuComponent implements OnInit{
  menus: Observable<any[]>;

  @ViewChild('sideMenu', { static: true }) sideMenu: ElementRef;

  constructor(private menuService: AppMenuService, private router: Router) { }


  ngOnInit() {
    this.menus = this.menuService.menus;
  }

  setMenuAtivo(menu): void {
    this.menuService.getAppMenus().forEach(mn => {
      mn.submenus.forEach(sm => {
        sm.active = mn.active = menu.id === sm.id;
      });

      mn.active = (menu.id === mn.id || mn.submenus.find(sm => menu.id === sm.id) !== undefined);
    });

    this.menuService.setMenuAtivo(menu);
  }

  startMetisMenu() {
    const sdMenu = $(this.sideMenu.nativeElement).metisMenu();

    sdMenu.on('shown.metisMenu', (e) => {

      const heightWithoutNavbar = ($('body > #wrapper')).height() - 62;
      ($('.sidebar-panel')).css('min-height', heightWithoutNavbar + 'px');

      const navbarheight = ($('nav.navbar-default')).height();
      const wrapperHeight = ($('#page-wrapper')).height();

      if (navbarheight > wrapperHeight) {
        ($('#page-wrapper')).css('min-height', navbarheight + 'px');
      }

      if (navbarheight < wrapperHeight) {
        ($('#page-wrapper')).css('min-height', $(window).height() + 'px');
      }

      if ($('body').hasClass('fixed-nav')) {
        if (navbarheight > wrapperHeight) {
          ($('#page-wrapper')).css('min-height', navbarheight + 'px');
        } else {
          ($('#page-wrapper')).css('min-height', $(window).height() - 60 + 'px');
        }
      }
    });
  }
}
