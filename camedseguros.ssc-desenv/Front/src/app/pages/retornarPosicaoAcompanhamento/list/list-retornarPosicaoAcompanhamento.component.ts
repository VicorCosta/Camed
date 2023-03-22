//#region Imports

import { Component, OnInit } from '@angular/core';
import { BaseComponent, AuthenticationService } from 'src/app/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';

import { SolicitacaoService } from '../../solicitacao';
import { AcaoService } from '../../acao';
import { MessageService } from 'primeng/api';
import { RetornoDaPosicaoDeAcompanhamentoAtualizarSolicitacaoeService } from '../service';
//#endregion

@Component({
  templateUrl: 'list-retornarPosicaoAcompanhamento.component.html',
  styleUrls: ['list-retornarPosicaoAcompanhamento.component.css']

})

export class LisrRtornarPosicaoAcompanhamentoComponent extends BaseComponent implements OnInit {

  documentos: any[];
  solicitacoes: any[] = [];
  solicitacaoSelecionaada: any;
  consultando = false;
  posicao = 0;
  alertError: string = ""
  cpfcgc: string = null;
  tipoSolicitacao: string = null;
  ultAcompanhamento: any;

  constructor(
    private messageService: MessageService,
    private service: RetornoDaPosicaoDeAcompanhamentoAtualizarSolicitacaoeService,
    private SolicitacaoService: SolicitacaoService,
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router
  ) {
    super(authenticationService, fb, route, router);
  }
  ngOnInit(): void {
    this.solicitacaoSelecionaada = null
  }

  searchSolicitacao(event) {
    this.SolicitacaoService.get(`$select=id,numero,cadastrado_gs,cd_estudo&$filter=numero eq ${event.query}&$orderby=numero`).subscribe(data => {
      this.solicitacoes = [{
        id: data.value[0]?.id,
        numero: data.value[0]?.numero.toString(),
        cadastradoGS: data.value[0]?.cadastrado_GS,
        cdEstudo: data.value[0]?.cd_estudo
      }]
    });
  }

  setSolicitacao(solicitacao) {
    this.solicitacaoSelecionaada = solicitacao;
  }

  processar() {
    console.log('this.posicao', this.posicao)
    this.consultando = true;
    if (this.solicitacaoSelecionaada === null || this.posicao === 0 || !this.posicao) {
      this.alertError = "Preencha todos os campos obrigatórios."
    } else {
      this.alertError = ""

      this.service.getAcompanhamento(`$filter=Solicitacao_Id eq ${this.solicitacaoSelecionaada.id} and ordem eq ${this.posicao}`).subscribe(data => {
        if (data.length <= 0 && (!this.solicitacaoSelecionaada.cadastradoGS || !this.solicitacaoSelecionaada.cdEstudo))
          this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Acompanhamento não encontrado.' });

        if (this.solicitacaoSelecionaada.cadastradoGS || this.solicitacaoSelecionaada.cdEstudo)
          this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Solicitação já foi enviada ao GS e não poderá ser modificado.' });

        if (data.length > 0 && (!this.solicitacaoSelecionaada.cadastradoGS || !this.solicitacaoSelecionaada.cdEstudo)) {
          this.service.getAcompanhamento(`$filter=Solicitacao_Id eq ${this.solicitacaoSelecionaada.id}`).subscribe(result => {
            this.ultAcompanhamento = result[result.length - 1];
            
            if (this.ultAcompanhamento.ordem == data[0].ordem)
              this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'O Acompanhamento já está na última posição, processo finalizado.' });

            if (data[0].ordem == 1)
              this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'O Acompanhamento não pode ter sua ordem alterada.' });

            if (data[0].ordem != 1 && this.ultAcompanhamento.ordem != data[0].ordem) {
              this.service.deletar(this.solicitacaoSelecionaada.id, this.posicao).subscribe((data) => { });
              this.service.processar(this.solicitacaoSelecionaada.id, data[0].dataEHora, data[0].situacao_Id, this.posicao).subscribe((data) => { });
              this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Processamento realizado.' });
              this.solicitacaoSelecionaada = null;
              this.posicao = 0;
            }
          });
        }
      });
    }
    // const doc = this.cpfcgc.match( /\d+/g ).join('');
    this.consultando = false;
  }
}
