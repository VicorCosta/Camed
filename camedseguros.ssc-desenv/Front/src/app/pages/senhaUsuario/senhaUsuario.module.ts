import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { CheckboxModule } from 'primeng/checkbox';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { AngularSlickgridModule } from 'angular-slickgrid';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { SenhaUsuarioService } from './service';
import { FormSenhaUsuarioComponent } from './form';

const routes: Routes = [
  { path: '', component: FormSenhaUsuarioComponent }
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
    FormSenhaUsuarioComponent
  ],

  providers: [
    SenhaUsuarioService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})

export class SenhaUsuarioModule { }
