import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { VinculoService } from './service';
import { ListVinculoComponent } from './list';
import { FormVinculoComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';


const routes: Routes = [
  { path: '', component: ListVinculoComponent },
  { path: 'cadastrar', component: FormVinculoComponent },
  { path: 'editar/:id', component: FormVinculoComponent }
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
    ListVinculoComponent,
    FormVinculoComponent
  ],

  providers: [
    VinculoService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class VinculoBNBModule { }
