import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { MotivoCancelamentoService } from './service';
import { ListMotivoCancelamentoComponent } from './list';
import { FormMotivoCancelamentoComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';


const routes: Routes = [
  { path: '', component: ListMotivoCancelamentoComponent },
  { path: 'cadastrar', component: FormMotivoCancelamentoComponent },
  { path: 'editar/:id', component: FormMotivoCancelamentoComponent }
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
    ListMotivoCancelamentoComponent,
    FormMotivoCancelamentoComponent
  ],

  providers: [
    MotivoCancelamentoService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class MotivoCancelamentoModule { }
