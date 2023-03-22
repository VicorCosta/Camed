import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { ExpedienteService } from './service';
import { ListExpedienteComponent } from './list';
import { FormExpedienteComponent } from './form';
import { PlaceholderGridModule, AppMenuService, HeaderPageModule } from 'src/app/shared';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

const routes: Routes = [
  { path: '', component: ListExpedienteComponent },
  { path: 'cadastrar', component: FormExpedienteComponent },
  { path: 'editar/:id', component: FormExpedienteComponent }
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
    ToastModule
  ],

  declarations: [
    ListExpedienteComponent,
    FormExpedienteComponent
  ],

  providers: [
    ExpedienteService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class ExpedienteModule { }
