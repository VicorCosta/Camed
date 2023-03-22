import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CheckboxModule } from 'primeng/checkbox';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { EmpresaService } from './service';
import { ListEmpresasComponent } from './list';
import { FormEmpresasComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { TipoSeguroService } from '../tiposdeseguro';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { InputSwitchModule } from 'primeng/inputswitch';

const routes: Routes = [
  { path: '', component: ListEmpresasComponent },
  { path: 'cadastrar', component: FormEmpresasComponent },
  { path: 'editar/:id', component: FormEmpresasComponent }
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
    ToastModule,
    InputSwitchModule,
  ],

  declarations: [
    ListEmpresasComponent,
    FormEmpresasComponent
  ],

  providers: [
    EmpresaService,
    TipoSeguroService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class EmpresaModule { }
