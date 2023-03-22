import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { AuditoriaDetalhesService, AuditoriaService } from '../service';
import * as moment from 'moment';
import { param } from 'jquery';

@Component({
  selector: 'app-detail-auditoria',
  templateUrl: './detail-auditoria.component.html',
  styleUrls: ['./detail-auditoria.component.css']
})
export class DetailAuditoriaComponent extends BaseComponent implements OnInit {

  itemId: any;
  eventTime: any;
  userName: any;
  eventType: any;
  tableName: any;
  chave: any;
  numeroDaSolicitacao: any;

  submitted = false;
  display = false;
  titulo: string;
  auditdetalhes: any = [];
  detalhes: any = {
    "id": '',
    "dataHora": '',
    "usuario": '',
    "tipo": '',
    "tabela": '',
    "chave": '',
    "numeroSolicitacao": '',
    auditoriaDetalhes: {
      coluna: '',
      novoValor: '',
      originalValor: ''
    }
  };

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() item: any;


  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private service: AuditoriaService,
    private auditlog: AuditoriaDetalhesService) {
    super(authenticationService, fb, route, router);

    this.route.params.subscribe(params => {
      console.log(params)
      this.itemId = params['id']
      this.eventTime = params['eventTime']
      this.userName = params['userName']
      this.eventType = params['eventType']
      this.tableName = params['tableName']
      this.chave = params['chave']
      this.numeroDaSolicitacao = params['numeroDaSolicitacao']
    });

    console.log(this.eventTime)
  }
  ngOnInit(): void {
    if (this.itemId) {
      this.auditlog.get("RecordId=550992&DataInicial=2022-04-04&DataFinal=2022-10-10").subscribe((data) => {
        this.auditdetalhes = data;
      });
      this.titulo = 'Detalhes Auditoria';
      this.detalhes.id = this.itemId
      this.detalhes.dataHora = moment(this.eventTime).format("DD/MM/YYYY HH:mm:ss");
      this.detalhes.usuario = this.userName == "null" ? "" : this.userName;
      this.detalhes.tipo = this.eventType == "null" ? "" : this.eventType;
      this.detalhes.tabela = this.tableName == "null" ? "" : this.tableName;
      this.detalhes.chave = this.chave == "null" ? "" : this.chave;
      this.detalhes.numeroSolicitacao = this.numeroDaSolicitacao != null ? this.numeroDaSolicitacao: 'Sem número de solicitação.'
    }
  }

  onClosePanel() {
    window.history.back();
  }

  imprimir() {
    this.submitted = true;
    this.loading = true;
  }
}



