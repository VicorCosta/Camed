import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { TiposAgenciaService } from './service';
import { ListTiposAgenciaComponent } from './list';
import { FormTiposAgenciaComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { GrupoAgenciaService } from '../grupoagencias';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';


const routes: Routes = [
  { path: '', component: ListTiposAgenciaComponent },
  { path: 'cadastrar', component: FormTiposAgenciaComponent },
  { path: 'editar/:id', component: FormTiposAgenciaComponent }
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
    ListTiposAgenciaComponent,
    FormTiposAgenciaComponent
  ],

  providers: [
    TiposAgenciaService,
    GrupoAgenciaService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class TipoDeAgenciaModule { }
