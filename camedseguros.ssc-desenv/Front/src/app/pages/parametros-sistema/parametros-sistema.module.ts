import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CheckboxModule } from 'primeng/checkbox';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { MultiSelectModule } from 'primeng/multiselect';
import { InputMaskModule } from 'primeng/inputmask';
import { QuillModule } from 'ngx-quill'

import { AngularSlickgridModule } from 'angular-slickgrid';

import { MapeamentoAcaoSituacaoService, ParametrosSistemaService } from './service';
import { ListParametrosSistemaComponent } from './list';
import { FormParametrosSistemaComponent } from './form';
import { CommonModule } from '@angular/common';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared/components';

import { TipodeParametroService } from '../tipos-de-parametros';
import { VariaveisDeEmailService } from '../variaveisdeemail/service';
import { GrupoAgenciaService } from '../grupoagencias';
import { GrupoService } from '../grupo';
import { AreaDeNegocioService } from '../areadenegocio';
import { MessageService } from 'primeng/api';
import {ToastModule} from 'primeng/toast';
import { AcaoService } from '../acao';


const routes: Routes = [
  { path: '', component: ListParametrosSistemaComponent },
  { path: 'cadastrar', component: FormParametrosSistemaComponent },
  { path: 'editar/:id', component: FormParametrosSistemaComponent }

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
    ToastModule,
    QuillModule.forRoot(),
  ],

  declarations: [
    ListParametrosSistemaComponent,
    FormParametrosSistemaComponent
  ],

  providers: [
    ParametrosSistemaService,
    TipodeParametroService,
    VariaveisDeEmailService,
    GrupoAgenciaService,
    GrupoService,
    AreaDeNegocioService,
    MessageService,
    MapeamentoAcaoSituacaoService,
    AcaoService
  ],
  exports: [
    RouterModule
  ]
})
export class ParametrosSistemaModule { }
