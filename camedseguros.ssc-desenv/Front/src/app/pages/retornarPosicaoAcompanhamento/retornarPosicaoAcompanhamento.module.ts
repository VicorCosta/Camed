import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { AutoCompleteModule } from 'primeng/autocomplete';
import { InputMaskModule } from 'primeng/inputmask';
import { HeaderPageModule } from 'src/app/shared';
import { SolicitacaoService } from '../solicitacao';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { LisrRtornarPosicaoAcompanhamentoComponent } from './list/list-retornarPosicaoAcompanhamento.component';
import { RetornoDaPosicaoDeAcompanhamentoAtualizarSolicitacaoeService } from './service';


const routes: Routes = [
  { path: '', component: LisrRtornarPosicaoAcompanhamentoComponent }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    FormsModule,
    ReactiveFormsModule,
    AutoCompleteModule,
    InputMaskModule,
    HeaderPageModule,
    ToastModule
  ],

  declarations: [
    LisrRtornarPosicaoAcompanhamentoComponent,
  ],

  providers: [
    RetornoDaPosicaoDeAcompanhamentoAtualizarSolicitacaoeService,
    SolicitacaoService,
    MessageService
  ],
  exports: [
    RouterModule
  ]
})
export class RetornarPosicaoAcompanhamentoModule { }
