import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { AngularSlickgridModule } from 'angular-slickgrid';

import { AcaoService } from './service';
import { ListAcoesComponent } from './list';
import { FormAcoesComponent } from './form';
import { CommonModule } from '@angular/common';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';


const routes: Routes = [
  { path: '', component: ListAcoesComponent },
  { path: 'cadastrar', component: FormAcoesComponent },
  { path: 'editar/:id', component: FormAcoesComponent }

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
    ListAcoesComponent,
    FormAcoesComponent
  ],

  providers: [
    AcaoService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class AcaoModule { }
