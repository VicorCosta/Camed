import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { CampanhaService } from './service';
import { ListCampanhaComponent } from './list';
import { FormCampanhaComponent } from './form';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

const routes: Routes = [
  { path: '', component: ListCampanhaComponent },
  { path: 'cadastrar', component: FormCampanhaComponent },
  { path: 'editar/:id', component: FormCampanhaComponent }

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
    ListCampanhaComponent,
    FormCampanhaComponent
  ],

  providers: [
    CampanhaService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class CampanhaModule { }
