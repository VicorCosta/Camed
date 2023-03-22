import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CheckboxModule } from 'primeng/checkbox';
import { AngularSlickgridModule } from 'angular-slickgrid';

import { TipoSeguroService } from './service';
import { ListTiposSeguroComponent } from './list';
import { FormTiposSeguroComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { GrupoAgenciaService } from '../grupoagencias';
import { RamosService } from '../ramosdeseguro';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';


const routes: Routes = [
  { path: '', component: ListTiposSeguroComponent },
  { path: 'cadastrar', component: FormTiposSeguroComponent },
  { path: 'editar/:id', component: FormTiposSeguroComponent }

];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    AngularSlickgridModule,
    ScrollPanelModule,
    SidebarModule,
    CheckboxModule,
    FormsModule,
    ReactiveFormsModule,
    PlaceholderGridModule,
    HeaderPageModule,
    ToastModule
  ],

  declarations: [
    ListTiposSeguroComponent,
    FormTiposSeguroComponent
  ],

  providers: [
    TipoSeguroService,
    GrupoAgenciaService,
    RamosService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class TipoDeSeguroModule { }
