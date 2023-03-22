import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CheckboxModule } from 'primeng/checkbox';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { FramesService } from './service';
import { ListFramesComponent } from './list';
import { FormFramesComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { AcaoDeAcompanhamentoService } from '../acaodeacompanhamento';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

const routes: Routes = [
  { path: '', component: ListFramesComponent },
  { path: 'cadastrar', component: FormFramesComponent },
  { path: 'editar/:id', component: FormFramesComponent }

];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    AngularSlickgridModule,
    ScrollPanelModule,
    SidebarModule,
    CheckboxModule,
    FormsModule,
    ReactiveFormsModule,
    PlaceholderGridModule,
    HeaderPageModule,
    ToastModule
  ],

  declarations: [
    ListFramesComponent,
    FormFramesComponent
  ],

  providers: [
    FramesService,
    AcaoDeAcompanhamentoService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class FrameModule { }
