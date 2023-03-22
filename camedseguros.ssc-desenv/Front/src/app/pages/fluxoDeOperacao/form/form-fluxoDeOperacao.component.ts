import { Component, Output, EventEmitter, Input, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, ParametrosSistemaService } from 'src/app/core';
import { FormBuilder } from '@angular/forms';

import _ from 'underscore';


import { FluxoDeOperacaoService } from '../service';
import { GrupoService } from '../../grupo';
import { SituacaoService } from '../../situacoes';
import { AcaoDeAcompanhamentoService } from '../../acaodeacompanhamento';
import { Observable } from 'rxjs';
import { MessageService } from 'primeng/api';
import { ThisReceiver } from '@angular/compiler';

@Component({
  selector: 'app-form-fluxo-de-operacao',
  templateUrl: './form-fluxoDeOperacao.component.html',
  styleUrls: ['./form-fluxoDeOperacao.component.css']
})
export class FormFluxoDeOperacaoComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  titulo: string;

  situacaoAtualSelecionada: any;
  acaoSelecionada: any;
  grupoSelecionado: any;
  proximaSituacaoSelecionada: any;
  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() id: any;

  situacoesAtuais: any[];
  proximasSituacoes: any[];
  acoes: any[];
  grupos: any[];

  parametrosSistema$: Observable<any>;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private service: FluxoDeOperacaoService,
    private situacaoService: SituacaoService,
    private acaoService: AcaoDeAcompanhamentoService,
    private grupoService: GrupoService,
    private parametrosSistemaService: ParametrosSistemaService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  ngOnInit(): void {
    this.parametrosSistema$ = this.parametrosSistemaService.get('$select=id,parametro&$orderby=parametro');


    this.situacaoAtualSelecionada = null;
    this.acaoSelecionada = null;
    this.grupoSelecionado = null;
    this.proximaSituacaoSelecionada = null;
    this.alertError = '';

    this.situacoesAtuais = [];
    this.proximasSituacoes = [];
    this.acoes = [];
    this.grupos = [];

    this.getForm()
  }

  getFormEditar() {
    this.titulo = 'Editar Fluxo de Operação';
    this.service.get(`$filter=id eq ${this.id}&$expand=acao,situacaoAtual,proximaSituacao,parametrosSistema,parametroSistemaSMS,grupo`).subscribe(({ value }) => {
      this.situacaoAtualSelecionada = (value[0].situacaoAtual !== null ? value[0].situacaoAtual : null);
      this.acaoSelecionada = (value[0].acao !== null ? value[0].acao : null);
      this.proximaSituacaoSelecionada = (value[0].proximaSituacao !== null ? value[0].proximaSituacao : null);
      this.grupoSelecionado = (value[0].grupo !== null ? value[0].grupo : null);

      this.form = this.fb.group({
        id: value[0].id,
        ordemFluxo: value[0].ordemFluxo,
        ordemFluxo2: value[0].ordemFluxo2,
        situacaoAtual_Id: (value[0].situacaoAtual !== null ? value[0].situacaoAtual.id : null),
        acao_Id: (value[0].acao !== null ? value[0].acao.id : null),
        proximaSituacao_Id: (value[0].proximaSituacao !== null ? value[0].proximaSituacao.id : null),
        grupo_Id: (value[0].grupo !== null ? value[0].grupo.id : null),
        permiteEnvioDeArquivo: value[0].permiteEnvioDeArquivo,
        exigeEnvioDeArquivo: value[0].exigeEnvioDeArquivo,
        enviaEmailSolicitanteAtendente: value[0].enviaEmailSolicitanteAtendente,
        exigeObservacao: value[0].exigeObservacao,
        enviaEmailAoSegurado: value[0].enviaEmailAoSegurado,
        parametrosSistema_Id: (value[0].parametrosSistema !== null ? value[0].parametrosSistema.id : null),
        enviaSMSAoSegurado: value[0].enviaSMSAoSegurado,
        parametroSistemaSMS_Id: (value[0].parametroSistemaSMS !== null ? value[0].parametroSistemaSMS.id : null)
      });
    })
  }

  getForm() {
    this.titulo = 'Novo Fluxo de Operação';
    this.form = this.fb.group({
      id: [0],
      ordemFluxo: [null],
      ordemFluxo2: [null],
      situacaoAtual_Id: [null],
      acao_Id: [null],
      proximaSituacao_Id: [null],
      grupo_Id: [null],
      permiteEnvioDeArquivo: [false],
      exigeEnvioDeArquivo: [false],
      enviaEmailSolicitanteAtendente: [false],
      exigeObservacao: [false],
      enviaEmailAoSegurado: [false],
      parametrosSistema_Id: [null],
      enviaSMSAoSegurado: [false],
      parametroSistemaSMS_Id: [null]
    });

    if (this.id) {
      this.getFormEditar()
    }
  }

  onClosePanel() {
    this.setResult({} as Result);
    // this.closePanel.emit(true);
    window.history.back()
  }

  searchSituacaoAtual(event) {
    this.situacaoService.get(`$select=id,nome&$filter=(contains(nome,'${event.query}'))&$orderby=nome`).subscribe(data => {
      this.situacoesAtuais = data.value;
    });
  }

  searchProximaSituacao(event) {
    this.situacaoService.get(`$select=id,nome&$filter=(contains(nome,'${event.query}'))&$orderby=nome`).subscribe(data => {
      this.proximasSituacoes = data.value;
    });
  }

  searchAcoes(event) {
    this.acaoService.get(`$select=id,nome&$filter=(contains(nome,'${event.query}'))&$orderby=nome`).subscribe(data => {
      this.acoes = data.value;
    });
  }

  searchGrupos(event) {
    this.grupoService.get(`$select=id,nome&$filter=(contains(nome,'${event.query}'))&$orderby=nome`).subscribe(data => {
      this.grupos = data.value;
    });
  }

  setSituacaoAtual(situacaoAtual) {
    this.form.controls.situacaoAtual_Id.setValue(situacaoAtual.id);
  }

  setProximaSituacao(proximaSituacao) {
    this.form.controls.proximaSituacao_Id.setValue(proximaSituacao.id);
  }

  setAcao(acao) {
    this.form.controls.acao_Id.setValue(acao.id);
  }

  setGrupo(grupo) {
    this.form.controls.grupo_Id.setValue(grupo.id);
  }


  onSubmit() {
    this.submitted = true;
    this.loading = true;
    const newValue = this.form.controls.id.value === 0;
    this.service.save(this.form.value).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        if (this.id) {
          this.getFormEditar()
          this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Fluxo de Operação Editada' });
        } else {
          this.ngOnInit()
          this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Fluxo de Operação Cadastrada' });

        }
        this.eventoConcluido.emit({ adicionar: newValue });
      }
    }, (error) => this.showError(error));
  }

}

