import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { MotivoRecusaService } from './service';
import { ListMotivoComponent } from './list';
import { FormMotivoComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { CheckboxModule } from 'primeng/checkbox';


const routes: Routes = [
  { path: '', component: ListMotivoComponent },
  { path: 'cadastrar', component: FormMotivoComponent },
  { path: 'editar/:id', component: FormMotivoComponent }
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
    CheckboxModule
  ],

  declarations: [
    ListMotivoComponent,
    FormMotivoComponent
  ],

  providers: [
    MotivoRecusaService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class MotivoDeRecusaModule { }
