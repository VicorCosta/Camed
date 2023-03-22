import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MultiSelectModule } from "primeng/multiselect";

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CheckboxModule } from 'primeng/checkbox';
import { AngularSlickgridModule } from 'angular-slickgrid';
import { InputSwitchModule } from 'primeng/inputswitch';
import { OrderListModule } from 'primeng/orderlist';

import { MapeamentoAreaDeNegocioService, TipoDeProdutoService } from './service';
import { TiposAgenciaService } from '../tiposDeAgencia';
import { TipoSeguroService } from '../tiposdeseguro';
import { VinculoService } from "../vinculobnb";


import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { EditorModule } from 'primeng/editor';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';


import { ListMapeamentoAreaDeNegocioComponent } from './list';
import { FormMapeamentoAreaDeNegocioComponent } from './form';
import { AreaDeNegocioService } from '../areadenegocio/service';




const routes: Routes = [
  { path: '', component: ListMapeamentoAreaDeNegocioComponent },
  { path: 'cadastrar', component: FormMapeamentoAreaDeNegocioComponent},
  { path: 'editar/:id', component: FormMapeamentoAreaDeNegocioComponent}

];

@NgModule({
  imports: [
CommonModule,
    RouterModule.forChild(routes),
    MultiSelectModule,
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
    ListMapeamentoAreaDeNegocioComponent,
    FormMapeamentoAreaDeNegocioComponent
  ],

  providers: [
    MapeamentoAreaDeNegocioService,
    TiposAgenciaService,
    TipoDeProdutoService,
    VinculoService,
    AreaDeNegocioService,
    TipoSeguroService,
    MessageService,
  ],

  exports: [
    RouterModule
  ]
})
export class MapeamentoAreaDeNegocioModule { }
