import { Component, OnInit, Output, EventEmitter, Attribute } from "@angular/core";

import { BaseComponent, AuthenticationService } from "src/app/core";
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder } from "@angular/forms";
import { SelectItem } from "primeng/api";
import * as moment from "moment";

import Swal from "sweetalert2";
import {
  AngularGridInstance,
  Column,
  GridOption,
  FieldType,
  Filters,
  //GridOdataService,
  Metrics,
  Formatters,
  OnEventArgs,
} from "angular-slickgrid";
import { GridOdataService } from "@slickgrid-universal/odata";

import { InboxService } from "../service";
import { localePtBR } from "src/locale/slickgrid.pt";
import { UsuarioService } from "../../usuario";
import { query } from "@angular/animations";
import { any } from "underscore";
import { identifierName, ThisReceiver } from "@angular/compiler";
import { debug } from "console";
import { SolicitacaoService } from "../../solicitacao/service/solicitacao.service";

@Component({
  templateUrl: "list-Inbox.component.html",
  styleUrls: ["./list-Inbox.component.css"],
})
export class ListInboxComponent extends BaseComponent implements OnInit {
  display = false;
  inbox$: any;
  InboxSelecionada: any;
  filtroEnviados: boolean = false;
  tituloBotaoFiltro: string;
  nomeLabel: string;
  opcoes: SelectItem[] = [
    { label: "Recebidos", value: false },
    { label: "Enviados", value: true },
  ];
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
  solicitacaoSelecionada: any;
  solicitacoes: any[];
  numeroSolicitacao: string = null;
  qntdMsgLidas: number = 0;
  skip: number = 0;

  podeVerBotao: boolean = true;

  loadingDataGrid = false;
  hasInitialLoading = true;
  atualizarGrid = localePtBR.TEXT_REFRESH_DATASET;

  filter: any = {
    dataInicial: null,
    dataFinal: null,
    solicitacaoSelecionada: null,
    status: "",
    origem: "",
    cpfCnpj: null,
  };

  @Output() inbox = new EventEmitter<any>();
  recebido: number;
  enviado: number;

  constructor(
    private Inboxservice: InboxService,
    private usuarioService: UsuarioService,
    private solicitacaoService: SolicitacaoService,
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router
  ) {
    super(authenticationService, fb, route, router);
  }

  searchSolicitacao(event) {
    this.solicitacaoService
      .get(`&$filter=numero eq ${event.query}`)
      .subscribe((data) => {
        this.solicitacoes = data.value;
      });
  }

  setSolicitacao(solicitacao) {
    this.numeroSolicitacao = solicitacao.id;
  }

  ngOnInit() {
    this.Inboxservice.get("$select=lida,")
    this.tituloBotaoFiltro =
      this.filtroEnviados == true ? "Recebidos" : "Enviados";
    this.nomeLabel =
      this.filtroEnviados == true ? "Destinatário" : "Remetentes";
    this.columnDefinitions = [
      {
        id: "edit",
        field: "",
        toolTip: "Editar",
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        formatter: Formatters.infoIcon,
        minWidth: 30,
        maxWidth: 30,

        onCellClick: (e: Event, args: OnEventArgs) => {
          this.editar(args.dataContext);
          const post = {
            id: args.dataContext.id,
          };
          this.Inboxservice
            .updateAsRead(post)
            .subscribe();
        }
      },
      {
        id: "delete",
        field: "",
        toolTip: "Excluir",
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        formatter: Formatters.deleteIcon,
        minWidth: 30,
        maxWidth: 30,

        onCellClick: (e: Event, args: OnEventArgs) => {
          this.remover(args.dataContext);
        },
      },
      {
        id: "remetente",
        name: "Remetentes",
        field: "usuarioRemetente",
        sortable: false,
        type: FieldType.string,
        filterable: false,
        queryFieldFilter: "usuarioRemetente/nome",
        formatter(row, cell, value, dataContext) {
          return value?.nome;
        },
      },

      {
        id: "assunto",
        name: "Assunto",
        field: "assunto",
        sortable: false,
        type: FieldType.string,
        filterable: false,
      },
      {
        id: "texto",
        name: "Texto",
        field: "texto",
        sortable: false,
        type: FieldType.string,
        filterable: false,
      },
      {
        id: "dataCriacao",
        name: "Data de Envio",
        field: "dataCriacao",
        formatter: (row, cell, value, dataContext) => {
          return moment(value).format("DD/MM/YYYY HH:mm");
        },
        sortable: false,
        type: FieldType.string,
        filterable: false,
      },
      {
        id: "cpf",
        name: "CPF/CNPJ",
        field: "usuarioRemetente",
        sortable: false,
        type: FieldType.string,
        filterable: false,
        queryFieldFilter: "usuarioRemetente/cpf",
        formatter(row, cell, value, dataContext) {
          return value?.cpf;
        },
      },
      {
        id: "lida",
        name: "Status",
        field: "lida",
        formatter: (row, cell, value, dataContext) => {
          if (value != true) {
            value =
              this.filtroEnviados == true ? "" : "<span style='color:white;font-weight:bold;background-color:red;padding:0.2em;border-radius: 0.4em;'>Nova</span>";
          } else {
            value = "";
          }
          return value;
        },
        sortable: false,
        type: FieldType.string,
        filterable: false,
      },
      {
        id: "solicitacao_Id",
        name: "Número da Solicitação",
        field: "solicitacao_Id",
        sortable: false,
        type: FieldType.string,
        filterable: false,
      },
    ];

    this.gridOptions = {
      alwaysShowVerticalScroll: false,
      autoResize: {
        maxWidth: 800,
      },
      enableEmptyDataWarningMessage: true,
      emptyDataWarning: {
        message: "Nenhum dado encontrado",
      },
      //enableFiltering: true,
      enablePagination: true,
      enableAutoResize: true,
      enableColumnPicker: true,
      pagination: {
        pageSizes: [5, 10, 15, 20, 25, 30, 40, 50, 75, 100],
        pageSize: 5,
        totalItems: 0,
      },
      backendServiceApi: {
        service: new GridOdataService(),
        options: {
          enableCount: this.isCountEnabled,
          version: 4,
        },
        preProcess: () => this.displaySpinner(true),
        process: (query) => this.getData(query),
        postProcess: (response) => {
          this.metrics = response.metrics;
          this.displaySpinner(false);
          this.getCallback(response);
        },
      },
    };

  }

  angularGridReady(angularGrid: AngularGridInstance) {
    this.angularGrid = angularGrid;
    this.dataview = angularGrid.dataView;
    this.grid = angularGrid.slickGrid;
    this.gridService = angularGrid.gridService;
  }

  getCallback(response) {
    this.gridOptions.pagination.totalItems = response["@odata.count"];

    if (this.metrics) {
      this.metrics.totalItemCount = response["@odata.count"];
    }

    this.gridOptions = Object.assign({}, this.gridOptions);
    this.dataset = response.value;
    this.hasInitialLoading = false;

    this.qntdMsgLidas = 0;

    if(this.podeVerBotao){
      for (let x = 0; x < this.dataset.length; x++) {
        if (this.dataset[x].lida == false) {
          this.qntdMsgLidas++;
        }
      }
    }

  }

  private displaySpinner(isProcessing): void {
    this.loadingDataGrid = isProcessing;
  }

  private remetenteDestinatarioPredicado(): string {
    return `${this.filtroEnviados ? "usuarioRemetente" : "usuarioDestinatario"
      }/id eq ${this.authenticationService.getLoggedUser().id}`;
  }

  private montarFiltroFormulario(): string {
    let query = `and 0 eq 0`;
    if (this.filter.dataInicial && !this.filter.dataFinal) {
      let dataFinalEmDiante = "2522-11-24"
      query += ` and dataCriacao ge ${this.filter.dataInicial} and dataCriacao le ${dataFinalEmDiante}`;
    }

    if (!this.filter.dataInicial && this.filter.dataFinal) {
      let dataInicialEmDiante = "2522-11-24"
      query += ` and dataCriacao ge ${this.filter.dataFinal} and dataCriacao le ${dataInicialEmDiante}`;
    }

    if (this.filter.dataInicial && this.filter.dataFinal) {
      query += ` and dataCriacao ge ${this.filter.dataInicial} and dataCriacao le ${this.filter.dataFinal}`;
    }

    if (this.filter.solicitacaoSelecionada) {
      query += `and solicitacao_Id eq ${this.filter.solicitacaoSelecionada}`;
    }

    if (this.filter.status) {
      query += ` and lida eq ${this.filter.status}`;
    }

    if (this.filter.origem) {
      if (this.filter.origem == "user"){
        query += ` and usuarioRemetente ne null`;
      } else {
        query += ` and usuarioRemetente eq ${this.filter.status}`;
      }
    }

    if (this.filter.cpfCnpj) {
      query += ` and usuarioRemetente/cpf eq '${this.filter.cpfCnpj}'`;
    }

    return query;
  }

  private getODataQuery(): string {
    // debugger;
    this.skip = !this.gridService || this.skip < 0 ? 0 : (this.gridService.paginationService.getCurrentPageNumber() - 1) * this.gridService.paginationService.getCurrentItemPerPage();

    return `$count=true&$top=${this.gridOptions.pagination.pageSize}&$skip=${this.skip}&$expand=usuarioRemetente,usuarioDestinatario,anexos&$orderby=dataCriacao desc&$filter=(visivelSaida eq true and ${this.remetenteDestinatarioPredicado()} ${this.montarFiltroFormulario()})`;
  }

  pesquisar() {
    this.reloadGrid();
  }

  marcarComoLidas() {
    for (let x = 0; x < this.dataset.length; x++) {
      if (this.dataset[x].lida == false) {
        let id = this.dataset[x].id;
        const GetId = {
          id: id
        };
        this.Inboxservice.updateAsRead(GetId).subscribe();
      }
    }
    this.qntdMsgLidas = 0;
    this.angularGrid.gridService.resetGrid();
  }

  getData(query) {
    if (query) this.odataQuery = query;
    return this.Inboxservice.get(this.getODataQuery());
  }

  reloadGrid() {
    this.angularGrid.gridService.resetGrid();
  }

  handleEventoConcluido(retorno) {
    if (retorno) {
      this.InboxSelecionada = null;
      this.display = false;
      this.reloadGrid();

      Swal.fire(
        "",
        `Inbox ${retorno.adicionar ? "Respondido" : "Enviado"}!`,
        "success"
      );
    }
  }

  openPanel() {
    this.InboxSelecionada = null;
    this.display = true;
  }

  handleClosePanel(fechar) {
    if (fechar) {
      this.InboxSelecionada = null;
      this.display = false;
      this.reloadGrid();
    }
  }

  remover(item) {
    Swal.fire({
      title: "Atenção!",
      text: `Deseja excluir o inbox: ${item.assunto}?`,
      type: "warning",
      showCancelButton: true,
      confirmButtonColor: "#5cd65c",
      cancelButtonColor: "#ff3333",
      confirmButtonText: "Sim",
      cancelButtonText: "Não",
      showLoaderOnConfirm: true,
      preConfirm: () => {
        this.Inboxservice.deletar(item.id).subscribe((response) => {
          if (response.successfully) {
            this.reloadGrid();
          }
        });
      },
    }).then((result) => {
      if (result.value) {
        Swal.fire("", "Inbox foi deletado.", "success");
      }
    });
  }

  editar(item) {
    this.router.navigate(["form"], { relativeTo: this.route, state: item });
    this.display = true;
  }

  trocarFiltro() {
    if(this.filtroEnviados == true){
      this.podeVerBotao = false;
    } else {
      this.podeVerBotao = true;
    }
    this.skip = -1;
    this.reloadGrid();
  }
}
