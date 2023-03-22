import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { TiposCategoriaService } from './service';
import { ListTiposCategoriaComponent } from './list';
import { FormTiposCategoriaComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { TipoProdutoService } from 'src/app/core';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

const routes: Routes = [
  { path: '', component: ListTiposCategoriaComponent },
  { path: 'cadastrar', component: FormTiposCategoriaComponent },
  { path: 'editar/:id', component: FormTiposCategoriaComponent }
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
    ListTiposCategoriaComponent,
    FormTiposCategoriaComponent
  ],

  providers: [
    TiposCategoriaService,
    TipoProdutoService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class TipoDeCategoriaModule { }
