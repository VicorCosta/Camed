import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { DocumentoApoliceService } from './service';
import { ListDocumentoApoliceComponent } from './list';
import { CommonModule } from '@angular/common';

import { AutoCompleteModule } from 'primeng/autocomplete';
import { InputMaskModule } from 'primeng/inputmask';
import { AgenciaService } from 'src/app/core';
import { HeaderPageModule } from 'src/app/shared';


const routes: Routes = [
  { path: '', component: ListDocumentoApoliceComponent }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    FormsModule,
    ReactiveFormsModule,
    AutoCompleteModule,
    InputMaskModule,
    HeaderPageModule
  ],

  declarations: [
    ListDocumentoApoliceComponent,
  ],

  providers: [
    DocumentoApoliceService,
    AgenciaService
  ],
  exports: [
    RouterModule
  ]
})
export class DocumentoApoliceModule { }
