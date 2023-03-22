import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { AreaDeNegocioService } from './service';
import { ListAreaDeNegocioComponent } from './list';
import { FormAreaDeNegocioComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

const routes: Routes = [
  { path: '', component: ListAreaDeNegocioComponent },
  { path: 'cadastrar', component: FormAreaDeNegocioComponent },
  { path: 'editar/:id', component: FormAreaDeNegocioComponent }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    AngularSlickgridModule,
    ScrollPanelModule,
    SidebarModule,
    FormsModule,
    HeaderPageModule,
    ReactiveFormsModule,
    PlaceholderGridModule,
    ToastModule
  ],

  declarations: [
    ListAreaDeNegocioComponent,
    FormAreaDeNegocioComponent
  ],

  providers: [
    AreaDeNegocioService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class AreaDeNegocioModule { }
