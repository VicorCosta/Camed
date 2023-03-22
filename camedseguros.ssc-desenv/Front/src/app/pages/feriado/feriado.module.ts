import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { FeriadoService } from './service';
import { ListFeriadoComponent } from './list';
import { FormFeriadoComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';


const routes: Routes = [
  { path: '', component: ListFeriadoComponent },
  { path: 'cadastro', component: FormFeriadoComponent },
  { path: 'editar/:id', component: FormFeriadoComponent }

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
    ListFeriadoComponent,
    FormFeriadoComponent
  ],

  providers: [
    FeriadoService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class FeriadoModule { }
