import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CheckboxModule } from 'primeng/checkbox';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { TipodeParametroService } from './service';
import { ListTipodeParametrosComponent } from './list';
import { FormTipodeParametrosComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { TipoSeguroService } from '../tiposdeseguro';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

const routes: Routes = [
  { path: '', component: ListTipodeParametrosComponent },
  { path: 'cadastrar', component: FormTipodeParametrosComponent },
  { path: 'editar/:id', component: FormTipodeParametrosComponent }
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
    HeaderPageModule,
    ToastModule
  ],

  declarations: [
    ListTipodeParametrosComponent,
    FormTipodeParametrosComponent
  ],

  providers: [
    TipodeParametroService,
    TipoSeguroService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class TipodeParametroModule { }
