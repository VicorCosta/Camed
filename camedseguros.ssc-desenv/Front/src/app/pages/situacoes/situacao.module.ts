import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CheckboxModule } from 'primeng/checkbox';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { SituacaoService } from './service';
import { ListSituacoesComponent } from './list';
import { FormSituacoesComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

const routes: Routes = [
  { path: '', component: ListSituacoesComponent },
  { path: 'cadastrar', component: FormSituacoesComponent },
  { path: 'editar/:id', component: FormSituacoesComponent }

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
    ListSituacoesComponent,
    FormSituacoesComponent
  ],

  providers: [
    SituacaoService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class SituacaoModule { }
