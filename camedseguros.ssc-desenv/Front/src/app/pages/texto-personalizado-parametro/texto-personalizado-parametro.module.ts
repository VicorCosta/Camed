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

import { TextoPersonalizadoParametroService, TipoDeProdutoService } from './service';
import { TiposAgenciaService } from '../tiposDeAgencia';
import { TipoSeguroService } from '../tiposdeseguro';
import { VinculoService } from "../vinculobnb";


import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { EditorModule } from 'primeng/editor';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';


import { ListTextoPersonalizadoParametroComponent } from './list';
import { FormTextoPersonalizadoParametroComponent } from './form';
import { AreaDeNegocioService } from '../areadenegocio/service';
import { QuillModule } from 'ngx-quill';




const routes: Routes = [
  { path: '', component: ListTextoPersonalizadoParametroComponent },
  { path: 'cadastrar', component: FormTextoPersonalizadoParametroComponent},
  { path: 'editar/:id', component: FormTextoPersonalizadoParametroComponent}

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
    ToastModule,
    
    ReactiveFormsModule,
    CommonModule,
    QuillModule.forRoot()
  ],

  declarations: [
    ListTextoPersonalizadoParametroComponent,
    FormTextoPersonalizadoParametroComponent
  ],

  providers: [
    TextoPersonalizadoParametroService,
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

export class TextoPersonalizadoParametroModule { }
