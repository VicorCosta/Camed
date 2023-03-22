import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { CheckboxModule } from 'primeng/checkbox';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { InputSwitchModule } from 'primeng/inputswitch';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { FluxoDeOperacaoService } from './service';
import { ListFluxoDeOperacaoComponent } from './list';
import { FormFluxoDeOperacaoComponent } from './form';
import { CommonModule } from '@angular/common';
import { PlaceholderGridModule } from '../../shared/components/placeholder-grid';
import { SituacaoService } from '../situacoes';
import { AcaoDeAcompanhamentoService } from '../acaodeacompanhamento';
import { GrupoService } from '../grupo';
import { ParametrosSistemaService } from 'src/app/core';
import { HeaderPageModule } from 'src/app/shared';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

const routes: Routes = [
  { path: '', component: ListFluxoDeOperacaoComponent },
  { path: 'cadastrar', component: FormFluxoDeOperacaoComponent },
  { path: 'editar/:id', component: FormFluxoDeOperacaoComponent }

];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    AngularSlickgridModule,
    ScrollPanelModule,
    SidebarModule,
    AutoCompleteModule,
    CheckboxModule,
    InputSwitchModule,
    FormsModule,
    ReactiveFormsModule,
    PlaceholderGridModule,
    HeaderPageModule,
    ToastModule
  ],

  declarations: [
    ListFluxoDeOperacaoComponent,
    FormFluxoDeOperacaoComponent
  ],

  providers: [
    FluxoDeOperacaoService,
    SituacaoService,
    AcaoDeAcompanhamentoService,
    GrupoService,
    ParametrosSistemaService,
    MessageService
  ],
  exports: [
    RouterModule,
  ]
})
export class FluxoDeOperacaoModule { }
