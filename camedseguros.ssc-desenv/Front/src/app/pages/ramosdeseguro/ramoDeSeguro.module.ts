import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { CheckboxModule } from 'primeng/checkbox';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { RamosService } from './service';
import { ListRamosComponent } from './list';
import { FormRamosComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { SituacaoService } from '../situacoes';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

const routes: Routes = [
  { path: '', component: ListRamosComponent },
  { path: 'cadastrar', component: FormRamosComponent },
  { path: 'editar/:id', component: FormRamosComponent }

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
    ListRamosComponent,
    FormRamosComponent
  ],

  providers: [
    RamosService,
    SituacaoService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class RamoDeSeguroModule { }
