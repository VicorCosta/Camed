import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { AngularSlickgridModule } from 'angular-slickgrid';

import { VariaveisDeEmailService } from './service';
import { FormVariaveisComponent } from './form/form-variaveis-email.componet';
import { CommonModule } from '@angular/common';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { ListVariaveisComponent } from './list/list-variaveis-email.component';
import { ParametrosSistemaService } from 'src/app/core';


const routes: Routes = [
  { path: '', component: ListVariaveisComponent },
  { path: 'cadastrar', component: FormVariaveisComponent },
  { path: 'editar/:id', component: FormVariaveisComponent }

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
    ListVariaveisComponent,
    FormVariaveisComponent
  ],

  providers: [
    VariaveisDeEmailService,
    MessageService,
    ParametrosSistemaService
  ],
  exports: [
    RouterModule
  ]
})
export class VariaveisDeEmailModule { }
