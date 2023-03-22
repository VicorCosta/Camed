import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import * as moment from "moment";

import { BaseComponent, AuthenticationService, User } from 'src/app/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';

import Swal from "sweetalert2";
import {
  AngularGridInstance,
  Column,
  GridOption,
  FieldType,
  // //GridOdataService,
  Metrics,
  Formatters,
  OnEventArgs,
  Filters,
  GridService,
  BackendService,
} from "angular-slickgrid";

import { AuditoriaService } from '../service';
import { UsuarioGrupo } from '../service';
import { localePtBR } from 'src/locale/slickgrid.pt';
import { GridOdataService } from '@slickgrid-universal/odata';
import { Query } from 'apollo-angular';
import { type } from 'os';

@Component({
  templateUrl: "list-auditoria.component.html",
})
export class ListAuditoriaComponent extends BaseComponent implements OnInit {
  display = false;
  displayPrint = false;
  auditoria$: any;
  itemSelecionado: any;
  itemSelecionadoDef: any[] = []
  itemsImprimir: any;
  loggedUser: User;
  isGerente: boolean = false;

  angularGrid: AngularGridInstance;
  columnDefinitions: Column[];
  gridOptions: GridOption;
  dataset: any[] = [];
  isCountEnabled = true;
  metrics: Metrics;
  odataQuery = "";
  dataview: any;
  grid: any;
  gridService: any;
  loadingDataGrid = false;
  hasInitialLoading = true;
  atualizarGrid = localePtBR.TEXT_REFRESH_DATASET;
  odataUrl: string = null;

  consultando = false;
  @Output() item = new EventEmitter<any>();

  constructor(private service: AuditoriaService,
    private userGrupoService: UsuarioGrupo,
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router
  ) {
    super(authenticationService, fb, route, router);
  }

  ngOnInit() {
    document.getElementById("alerta").style.display = "none";

    this.userGrupoService.getAll().subscribe((data) => {
      this.loggedUser = this.authenticationService.getLoggedUser();
      for (let i = 0; i < data.value.length; i++) {
        let usuarioId = data.value[i].usuario_Id
        let grupoId = data.value[i].grupo_Id

        if (usuarioId == this.loggedUser.id) {
          if (grupoId == 3) {
            setTimeout(() => {
              document.getElementById("proibicao").style.display = "none";
              document.getElementById("itensAuditoria").style.visibility = "visible";
            }, 1400)
          }
          if (grupoId == 2) {
            setTimeout(() => {
              document.getElementById("evento").style.display = "none";
              document.getElementById("lb_evento").style.display = "none";
              document.getElementById("proibicao").style.display = "none";
              document.getElementById("itensAuditoria").style.visibility = "visible";
              document.getElementById("evento").style.display = "none";
              document.getElementById("lb_evento").style.display = "none";
              document.getElementById("nomeTabela").style.display = "none";
              document.getElementById("lb_nomeTabela").style.display = "none";
              document.getElementById("chave").style.display = "none";
              document.getElementById("lb_chave").style.display = "none";
              document.getElementById("user").style.display = "none";
              document.getElementById("lb_user").style.display = "none";
              document.getElementById("messagem").style.display = "none";
              document.getElementById("lb_messagem").style.display = "none";

            }, 1400)
          }
          break;
        }
      }
    });

    this.columnDefinitions = [
      {
        id: "print",
        field: "",
        toolTip: "Imprimir",
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        formatter: Formatters.infoIcon,
        minWidth: 30,
        maxWidth: 30,

        onCellClick: (e: Event, args: OnEventArgs) => {
          this.detalhes(args.dataContext);
        }
      },
      {
        id: 'dataHora', name: 'Data/Hora', field: 'eventTime',
        formatter(row, cell, value, dataContext) {
          return moment(value).format('DD/MM/YYYY hh:mm:ss')
        },
      },
      {
        id: 'usuario', name: 'Usuário', field: 'userName',
      },
      {
        id: 'evento', name: 'Evento', field: 'eventType',
      },
      {
        id: 'nomeTabela', name: 'Tabela', field: 'tableName',
      },
      {
        id: 'chave', name: 'Chave', field: 'chave',
      },
      {
        id: 'numeroSolicitacao', name: 'Número de Solicitação', field: 'numeroDaSolicitacao',
      },
      {
        id: 'messagem', name: 'Mensagem', field: 'message',
      },
    ];

    this.gridOptions = {
      alwaysShowVerticalScroll: false,
      autoResize: {
        maxWidth: 1200,
      },
      enableEmptyDataWarningMessage: true,
      emptyDataWarning: {
        message: 'Nenhum dado encontrado'
      },
      enableFiltering: true,
      enablePagination: true,
      enableAutoResize: true,
      enableColumnPicker: true,
      pagination: {
        pageSizes: [5, 10, 15, 20, 25, 30, 40, 50, 75, 100],
        pageSize: 5,
        totalItems: 0,
      },
    };
  }

  filterQueryDate(query) {
    const dateRemove = "T00:00:00Z";
    let result = query;
    if (query.includes(dateRemove)) {
      result = query.replace(dateRemove, "");
      if (result.includes(dateRemove, "")) {
        result = result.replace(dateRemove, "");
      }
    }

    return result;
  }

  angularGridReady(angularGrid: AngularGridInstance) {
    this.angularGrid = angularGrid;
    this.dataview = angularGrid.dataView;
    this.grid = angularGrid.slickGrid;
    this.gridService = angularGrid.gridService;
  }

  private displaySpinner(isProcessing): void {
    this.loadingDataGrid = isProcessing;
  }

  reloadGrid() {
    this.angularGrid.gridService.resetGrid();
  }

  openPanel() {
    this.display = true;
  }
  consultar() {
    this.consultando = true;
    this.displaySpinner(this.consultando);

    const dataInicial$ = (<HTMLInputElement>document.getElementById("DI"));
    const dataFinal$ = (<HTMLInputElement>document.getElementById("DF"));
    const numeroSolicitacao$ = (<HTMLInputElement>document.getElementById("numeroSolicitacao"));
    const usuario$ = (<HTMLInputElement>document.getElementById("user"));
    const evento$ = (<HTMLInputElement>document.getElementById("evento"));
    const tabela$ = (<HTMLInputElement>document.getElementById("nomeTabela"));
    const chave$ = (<HTMLInputElement>document.getElementById("chave"));
    const mensagem$ = (<HTMLInputElement>document.getElementById("messagem"));

    if (dataFinal$.value.length == 0 || dataInicial$.value.length == 0) {
      this.consultando = false;
      document.getElementById("alerta").style.display = "block";
      this.displaySpinner(this.consultando);
      return;
    }

    this.odataUrl =
      `DataInicial=${dataInicial$.value}&DataFinal=${dataFinal$.value}&Usuario=${usuario$.value}&Evento=${evento$.value}&Tabela=${tabela$.value}&Chave=${chave$.value}&NumeroSolicitacao=${numeroSolicitacao$.value}&Mensagem=${mensagem$.value}`
    this.service.get(this.odataUrl).subscribe((data) => {
      document.getElementById("alerta").style.display = "none";
      document.getElementById("tabela").style.visibility = "visible";
      this.dataset = data
      this.itemsImprimir = data
      this.consultando = false;

      this.gridOptions.pagination.totalItems = this.dataset.length;
      this.displaySpinner(this.consultando);
    });

    this.reloadGrid();
  }
  openPrint() {
    this.router.navigate(['visualizar-impressao', { odataUrl: this.odataUrl }
    ], { relativeTo: this.route });
  }

  openDetail() {
    this.router.navigate([`detalhes/${this.itemSelecionado.id}/${this.itemSelecionado.userName}/${this.itemSelecionado.eventTime}/${this.itemSelecionado.eventType}/${this.itemSelecionado.tableName}/${this.itemSelecionado.chave}/${this.itemSelecionado.numeroDaSolicitacao}`
    ], { relativeTo: this.route });
  }

  handleClosePanel(fechar) {
    if (fechar) {
      this.itemSelecionado = null;
      this.display = false;
    }
  }

  handleClosePrint(fechar) {
    if (fechar) {
      this.displayPrint = false;
    }
  }

  handleEventoConcluido(retorno) {
    //
  }

  detalhes(item) {
    this.itemSelecionado = item;
    this.openDetail()
  }
}
