import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CheckboxModule } from 'primeng/checkbox';
import { AngularSlickgridModule } from 'angular-slickgrid';
import { InputSwitchModule } from 'primeng/inputswitch';
import { OrderListModule } from 'primeng/orderlist';

import { MenuService } from './service';
import { ListMenuComponent } from './list';
import { FormMenusComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { EditorModule } from 'primeng/editor';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { AcaoService } from '../acao';



const routes: Routes = [
  { path: '', component: ListMenuComponent },
  { path: 'cadastrar', component: FormMenusComponent },
  { path: 'editar/:id', component: FormMenusComponent }

];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    AngularSlickgridModule,
    ScrollPanelModule,
    SidebarModule,
    CheckboxModule,
    FormsModule,
    ReactiveFormsModule,
    PlaceholderGridModule,
    InputSwitchModule,
    OrderListModule,
    EditorModule,
    HeaderPageModule,
    ToastModule
  ],

  declarations: [
    ListMenuComponent,
    FormMenusComponent
  ],

  providers: [
    MenuService,
    MessageService,
    AcaoService
  ],
  exports: [
    RouterModule
  ]
})
export class MenuModule { }
