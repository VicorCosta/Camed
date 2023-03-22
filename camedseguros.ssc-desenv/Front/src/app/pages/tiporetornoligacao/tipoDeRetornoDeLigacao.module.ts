import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { RetonoligacaoService } from './service';
import { ListRetornoComponent } from './list';
import { FormRetornoComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';


const routes: Routes = [
  { path: '', component: ListRetornoComponent },
  { path: 'cadastrar', component: FormRetornoComponent },
  { path: 'editar/:id', component: FormRetornoComponent }
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
    ListRetornoComponent,
    FormRetornoComponent
  ],

  providers: [
    RetonoligacaoService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class TipoDeRetornoDeLigacaoModule { }
