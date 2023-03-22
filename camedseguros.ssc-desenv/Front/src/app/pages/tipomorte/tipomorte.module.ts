import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list/list.component';
import {  RouterModule, Routes } from '@angular/router';
import { AngularSlickgridModule } from 'angular-slickgrid';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { SidebarModule } from 'primeng/sidebar';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HeaderPageModule, PlaceholderGridModule } from 'src/app/shared';
import { ToastModule } from 'primeng/toast';
import { FormComponent } from './form/form.component';
import { MessageService } from 'primeng/api';
import { TipomorteService } from './service/tipomorte.service';


const routes: Routes = [
  { path: '', component: ListComponent },
  {path: 'cadastrar', component:FormComponent},
  {path: 'editar/:id', component:FormComponent},
  

];

@NgModule({
  declarations: [
    ListComponent,
    FormComponent,
  ],
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
  providers: [
    MessageService,
    TipomorteService
  ],
  exports: [
    RouterModule
  ]
})
export class TipomorteModule { }
