import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {  RouterModule, Routes } from '@angular/router';
import { AngularSlickgridModule } from 'angular-slickgrid';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { SidebarModule } from 'primeng/sidebar';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HeaderPageModule, PlaceholderGridModule } from 'src/app/shared';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { testeConexao } from './service/testeConexao';
import { FormTesteConexaoComponent } from './form';


const routes: Routes = [
  { path: '', component: FormTesteConexaoComponent }
  

];

@NgModule({
  declarations: [
    FormTesteConexaoComponent
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
    testeConexao
  ],
  exports: [
    RouterModule
  ]
})
export class TesteCoonexaoModule { }
