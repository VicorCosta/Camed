import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CheckboxModule } from 'primeng/checkbox';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { MultiSelectModule } from 'primeng/multiselect';
import { InputMaskModule } from 'primeng/inputmask';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { UsuarioService } from './service';
import { ListUsuarioComponent } from './list';
import { FormUsuarioComponent } from './form';
import { CommonModule } from '@angular/common';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared/components';

import { EmpresaService } from '../empresas';
import { GrupoAgenciaService } from '../grupoagencias';
import { GrupoService } from '../grupo';
import { AreaDeNegocioService } from '../areadenegocio';
import { MessageService } from 'primeng/api';
import {ToastModule} from 'primeng/toast';


const routes: Routes = [
  { path: '', component: ListUsuarioComponent },
  { path: 'cadastrar', component: FormUsuarioComponent },
  { path: 'editar/:id', component: FormUsuarioComponent }

];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    AngularSlickgridModule,
    ScrollPanelModule,
    SidebarModule,
    CheckboxModule,
    AutoCompleteModule,
    MultiSelectModule,
    InputMaskModule,
    FormsModule,
    ReactiveFormsModule,
    PlaceholderGridModule,
    DialogModule,
    ButtonModule,
    HeaderPageModule,
    ToastModule
  ],

  declarations: [
    ListUsuarioComponent,
    FormUsuarioComponent
  ],

  providers: [
    UsuarioService,
    EmpresaService,
    GrupoAgenciaService,
    GrupoService,
    AreaDeNegocioService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class UsuarioModule { }
