import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { TiposCancelamentoService } from './service';
import { ListTiposCancelamentoComponent } from './list';
import { FormTiposCancelamentoComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { GrupoAgenciaService } from '../grupoagencias';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';


const routes: Routes = [
  { path: '', component: ListTiposCancelamentoComponent },
  { path: 'cadastrar', component: FormTiposCancelamentoComponent },
  { path: 'editar/:id', component: FormTiposCancelamentoComponent }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    AngularSlickgridModule,
    ScrollPanelModule,
    SidebarModule,
    FormsModule,
    ReactiveFormsModule,
    PlaceholderGridModule,
    HeaderPageModule,
    ToastModule
  ],

  declarations: [
    ListTiposCancelamentoComponent,
    FormTiposCancelamentoComponent
  ],

  providers: [
    TiposCancelamentoService,
    GrupoAgenciaService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class TipoDeCancelamentoModule { }
