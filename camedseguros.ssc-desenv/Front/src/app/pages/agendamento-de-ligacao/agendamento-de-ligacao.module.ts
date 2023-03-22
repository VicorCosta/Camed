import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CheckboxModule } from 'primeng/checkbox';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { MultiSelectModule } from 'primeng/multiselect';
import { InputMaskModule } from 'primeng/inputmask';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { UsuarioService } from '../usuario';
import { AgendamentoDeLigacaoServices } from './service';

import { ListAgendamentoDeLigacaoComponent } from './list';
import { FormAgendamentoDeLigacaoComponent } from './form';
import { CommonModule } from '@angular/common';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared/components';

import { RetonoligacaoService } from '../tiporetornoligacao';
import { EmpresaService } from '../empresas';
import { TipoSeguroService } from '../tiposdeseguro';
import { AgenciaService } from 'src/app/core';
import { GrupoAgenciaService } from '../grupoagencias';
import { GrupoService } from '../grupo';
import { AreaDeNegocioService } from '../areadenegocio';
import { MessageService } from 'primeng/api';
import {ToastModule} from 'primeng/toast';


const routes: Routes = [
  { path: '', component: ListAgendamentoDeLigacaoComponent },
  { path: 'cadastrar', component: FormAgendamentoDeLigacaoComponent },
  { path: 'editar/:id', component: FormAgendamentoDeLigacaoComponent }

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
    HeaderPageModule,
    ToastModule

  ],

  declarations: [
    ListAgendamentoDeLigacaoComponent,
    FormAgendamentoDeLigacaoComponent
  ],

  providers: [
    RetonoligacaoService,
    AgendamentoDeLigacaoServices,
    UsuarioService,
    EmpresaService,
    TipoSeguroService,
    AgenciaService,
    GrupoAgenciaService,
    GrupoService,
    AreaDeNegocioService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class AgendamentoDeLigacaoModule { }
