import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';

import { AngularSlickgridModule } from 'angular-slickgrid';
import { DropdownModule } from 'primeng/dropdown';

import { ListAgenciaTipoComponent } from './list';
import { FormAgenciaTipoComponent } from './form';

import { TiposAgenciaService } from '../tiposDeAgencia';
import { AgenciaTipoService } from './service';
import { AgenciaService } from 'src/app/core';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

const routes: Routes = [
  { path: '', component: ListAgenciaTipoComponent },
  { path: 'cadastrar', component: FormAgenciaTipoComponent },
  { path: 'editar/:id', component: FormAgenciaTipoComponent }

];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    ScrollPanelModule,
    SidebarModule,
    PlaceholderGridModule,
    AngularSlickgridModule,
    HeaderPageModule,
    DropdownModule,
    ToastModule
  ],

  declarations: [
    ListAgenciaTipoComponent,
    FormAgenciaTipoComponent
  ],

  providers: [
    TiposAgenciaService,
    AgenciaTipoService,
    AgenciaService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class AgenciaTipoAgenciaModule { }
