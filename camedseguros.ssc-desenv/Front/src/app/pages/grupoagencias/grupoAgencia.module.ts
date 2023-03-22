import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { GrupoAgenciaService } from './service';
import { ListGrupoAgenciasComponent } from './list';
import { FormGrupoAgenciasComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

const routes: Routes = [
  { path: '', component: ListGrupoAgenciasComponent },
  { path: 'cadastrar', component: FormGrupoAgenciasComponent },
  { path: 'editar/:id', component: FormGrupoAgenciasComponent }
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
    ListGrupoAgenciasComponent,
    FormGrupoAgenciasComponent
  ],

  providers: [
    GrupoAgenciaService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class GrupoAgenciaModule { }
