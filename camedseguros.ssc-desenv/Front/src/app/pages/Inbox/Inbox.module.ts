import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MultiSelectModule } from 'primeng/multiselect';
import { AutoCompleteModule } from 'primeng/autocomplete';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { AngularSlickgridModule } from 'angular-slickgrid';
import { ToastModule } from 'primeng/toast';

import { InboxService } from './service';
import { UsuarioService } from '../usuario'

import { ListInboxComponent } from './list';
import { FormInboxComponent } from './FormInbox';
import { CommonModule } from '@angular/common';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { SelectButtonModule } from 'primeng/selectbutton';
import { FileUploadModule } from 'primeng/fileupload';
import { MessageService } from 'primeng/api';
import { SolicitacaoService } from '../solicitacao';
import { DropdownModule } from 'primeng/dropdown';
import { TiposCancelamentoService } from '../tiposDeCancelamento';
import { AcaoService } from '../acao';

import { NgxMaskModule, IConfig } from 'ngx-mask'


const routes: Routes = [
  { path: '', component: ListInboxComponent },
  { path: 'form', component: FormInboxComponent },

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
    DropdownModule,
    AutoCompleteModule,
    MultiSelectModule,
    SelectButtonModule,
    FileUploadModule,
    ToastModule,
    NgxMaskModule.forRoot(),
  ],

  declarations: [ListInboxComponent, FormInboxComponent],

  providers: [InboxService, UsuarioService, SolicitacaoService, MessageService, TiposCancelamentoService, AcaoService],
  exports: [RouterModule],
})
export class InboxModule {}
