import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { DistribuicaoService } from './service';
import { ListDistribuicaoComponent } from './list';
import { FormDistribuicaoComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

const routes: Routes = [
  { path: '', component: ListDistribuicaoComponent },
  { path: 'cadastrar', component: FormDistribuicaoComponent },
  { path: 'editar/:id', component: FormDistribuicaoComponent }

];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    AngularSlickgridModule,
    CheckboxModule,
    ScrollPanelModule,
    SidebarModule,
    FormsModule,
    ReactiveFormsModule,
    PlaceholderGridModule,
    HeaderPageModule,
    ToastModule
  ],

  declarations: [
    ListDistribuicaoComponent,
    FormDistribuicaoComponent
  ],

  providers: [
    DistribuicaoService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class CanalDeDistribuicaoModule { }
