import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { AngularSlickgridModule } from 'angular-slickgrid';

import { SLAAgendamentoService } from './service';
import { CommonModule } from '@angular/common';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { ListAgendamentoComponent } from './list/list-sla-agendamento.component';


const routes: Routes = [
  { path: '', component: ListAgendamentoComponent },

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
    ListAgendamentoComponent,
  ],

  providers: [
    SLAAgendamentoService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class SLAAgendametoModule { }
