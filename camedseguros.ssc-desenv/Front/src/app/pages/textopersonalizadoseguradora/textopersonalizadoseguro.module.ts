import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { TextoPersonalizadoSeguradora } from './service';
import { VW_SEGURADORA } from './service';
import { ListTextoPersonalizadoSeguroComponent } from './list';
import { FormTextoPersonalizadoSeguroComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { QuillModule } from 'ngx-quill';


const routes: Routes = [
  { path: '', component: ListTextoPersonalizadoSeguroComponent },
  { path: 'cadastro', component: FormTextoPersonalizadoSeguroComponent },
  { path: 'editar/:id', component: FormTextoPersonalizadoSeguroComponent }
//
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
    ToastModule,
    QuillModule.forRoot()
  ],

  declarations: [
    ListTextoPersonalizadoSeguroComponent,
    FormTextoPersonalizadoSeguroComponent
  ],

  providers: [
    TextoPersonalizadoSeguradora,
    VW_SEGURADORA,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class TextoPersonalizadoSeguroModule { }
