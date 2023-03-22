import { query } from '@angular/animations';
import { Component, OnInit, Output, EventEmitter, AfterViewInit, Directive, ElementRef } from "@angular/core";
import { Observable, from } from "rxjs";



import {
  BaseComponent,
  AuthenticationService,
  Solicitacao,
  VinculoBNBService,
} from "src/app/core";
import { Router, ActivatedRoute, NavigationExtras } from "@angular/router";
import { FormBuilder } from "@angular/forms";

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
  windowScrollPosition,
} from "angular-slickgrid";
import { GridOdataService } from "@slickgrid-universal/odata";

import { SolicitacaoService } from "../service";
import { localePtBR } from "src/locale/slickgrid.pt";

import { AgenciaService } from "../../../core/services/agencia.service";
import { TipoSeguroService } from "../../tiposdeseguro/service/tiposeguro.service";
import { TiposCancelamentoService } from "../../tiposDeCancelamento/service/tipocancelamento.service";
import { UsuarioService } from "../../usuario/service/usuario.service";
import { SeguradoService } from "../../../core/services/segurado.service";
import { SituacaoService } from "../../../core/services/situacao.service";
import { MomentModule } from "angular2-moment";
import * as moment from "moment";
import { DistribuicaoService } from "../../canaldedistribuicao";
import { TipoDeProdutoService } from "../../mapeamentoAreaDeNegocio";
import { AreaDeNegocioService } from "../../areadenegocio";
import { VW_SEGURADORA } from "../../textopersonalizadoseguradora";
import { runInThisContext } from "vm";
import { AgendamentoDeLigacaoServices } from "../../agendamento-de-ligacao";
import { RetonoligacaoService } from "../../tiporetornoligacao";
import { param, post } from "jquery";


import { AcompanhamentoService } from '../service/acompanhamento.service';
import { ThisReceiver } from '@angular/compiler';
import { UsuarioGrupo } from "../../auditoria";
import { GrupoService } from "../../grupo";
import { indexOf } from "underscore";
import { AvAtendimentoService } from "../service/avAtendimento.service";
import { CookieService } from 'ngx-cookie-service';

@Component({
  templateUrl: "./list-solicitacao.component.html",
  styleUrls: ["./list-solicitacao.component.css"],
  selector: 'autofocus'
})

export class ListSolicitacaoComponent extends BaseComponent implements OnInit {
  display = false;
  solicitacao$: Solicitacao;
  solicitacaoSelecionado: any;
  dadosParaNovoAcompanhamento: any = {};

  items = ['Filtro Avançado'];
  expandedIndex = 0;

  atendentes: any[];
  usuarioGrupos: any[];
  vwSeguradora: any[];
  superintendencias: any[];
  superContas: any[];
  agencias: any[];
  agenciasConta: any[];
  situacoes: any[];
  anexosSolicitacao: string[];
  addAnd = false;

  uploadedFiles: any[] = [];
  atendenteSelecionado: any = null;
  areaDeNegocioSelecionada: any;

  tipoSeguro: Observable<any>;
  ramosDeSeguro: Observable<any>;
  canalDistribuicao: Observable<any>;
  tiposCancelamento: Observable<any>;
  tiposDeProduto: Observable<any>;
  areaDeNegocio: Observable<any>;
  vinculosBNB: Observable<any>;
  agendamentosDeLigacao: any = [];
  tiporetorno$: any = [];

  obsAvAtendimento = "";
  obsAvAtendimentoFlag = false;
  notaDinamica = "";
  notaDoAtendimento = "";
  solicitacaoIdAtendimento = "";
  isDisabled = false;

  areasDeNegocios: any = [];

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
  filters = false;
  atualizarGrid = localePtBR.TEXT_REFRESH_DATASET;
  @Output() eventoConcluido = new EventEmitter<any>();

  @Output() solicitacao = new EventEmitter<any>();
  conteudoParaSalvarAtendente: any = {};
  cookieValue: any;
  numeroSolicitacao: any;
  atendente: any;
  LoggedUser: any;
  loggedUser: any;

  constructor(
    private cookieService: CookieService,
    private avAtendimentoService: AvAtendimentoService,
    private solicitacaoService: SolicitacaoService,
    authenticationService: AuthenticationService,
    private agenciaService: AgenciaService,
    private tipoSeguroService: TipoSeguroService,
    private tiposCancelamentoService: TiposCancelamentoService,
    private usuarioService: UsuarioService,
    private usuarioGrupoService: UsuarioGrupo,
    private grupoService: GrupoService,
    private seguradoService: SeguradoService,
    private situacaoService: SituacaoService,
    private acompanhamentoService: AcompanhamentoService,
    private canalDistribuicaoServie: DistribuicaoService,
    private tipoDeProdutoService: TipoDeProdutoService,
    private retornoService: RetonoligacaoService,
    private areaDeNegocioService: AreaDeNegocioService,
    private vW_SEGURADORAService: VW_SEGURADORA,
    private vinculoBNBService: VinculoBNBService,
    private agendamentoDeLigacaoService: AgendamentoDeLigacaoServices,
    private ElementRef: ElementRef,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router
  ) {
    super(authenticationService, fb, route, router);
  }

  ngOnInit() {

   
    this.usuarioGrupoService.getAll(`$select=Usuario_Id,Grupo_Id&$filter=Usuario_Id eq ${this.authenticationService.getLoggedUser().id}`).subscribe((data) => {
      let gruposId: any[] = [];
      let dataResult: any[] = data.value;

      for (let i = 0; i < dataResult.length; i++) gruposId[i] = dataResult[i].grupo_Id;

      this.grupoService.getAll(`$filter=id in (${gruposId})`).subscribe((res) => {
        this.usuarioGrupos = res.value;
      });
    })


    this.agendamentoDeLigacaoService
      .get(`$expand=tiporetornoligacao`)
      .subscribe((data) => {
        this.tiporetorno$ = data.value;
      });

    this.retornoService.get(``).subscribe((value) => {
      this.tiporetorno$ = value.value;
    });

    this.areaDeNegocioService.get("$select=id,nome&$orderby=nome").subscribe((data) => {
      data.value.forEach(element => {
        this.areasDeNegocios.push({ label: element.nome, value: element.id })
      });
    });

    this.tipoSeguro = this.tipoSeguroService.get(
      "$select=id,nome&$orderby=nome"
    );
    this.ramosDeSeguro = this.tipoSeguroService.get(
      "$select=id,nome&$orderby=nome"
    );
    this.canalDistribuicao = this.canalDistribuicaoServie.get(
      "$select=id,nome&$orderby=nome"
    );
    this.tiposCancelamento = this.tiposCancelamentoService.get(
      "$select=id,descricao&$orderby=descricao"
    );
    this.tiposDeProduto = this.tipoDeProdutoService.get(
      "$select=id,nome&$orderby=nome"
    );
    this.areaDeNegocio = this.areaDeNegocioService.get(
      "$select=id,nome&$orderby=nome"
    );
    this.vinculosBNB = this.vinculoBNBService.get(
      "$select=id,nome&$orderby=nome"
    );

    this.form = this.fb.group({
      listar: "",
      atendente: "",
      numeroSolicitacao: "",
      canalDistribuicao: "",
      segurado: "",
      tipoCancelamento: "",
      dataInicial: "",
      dataFInal: "",
      tipoSeguro: "",
      ramoDeSeguro: "",
      seguradora: "",
      areaDeNegocio: "",
      vinculoBnb: "",
      cnpj: "",
      observacao: "",
      numProposta: "",
      emProcesso: "",
      emAtraso: "",
      superintendencia: "",
      superConta: "",
      agencia: "",
      agenciaConta: "",
      situacao: "",
      statusSituacao: "",
    });
    this.columnDefinitions = [
      {
        id: "total",
        name: "SLA Total",
        field: "",
        /*queryFieldSorter: "cast(numero, 'Edm.String')",*/
        sortable: true,
        type: FieldType.number,
        filterable: false,
        formatter: (row, cell, value, dataContext) =>
        (value = `<i style="
        cursor: pointer;
        background-color: #cccccc;
        width: 110px;
        height: 20px;
        text-align: center;
        border: solid 2px #aaaaaa;
        border-radius: 5px;" class="fa fa-clock-o" aria-hidden="true"> 2hr 03min</i>`)
      },
      {
        id: "renoatual",
        name: "SLA Atual",
        field: "",
        /*queryFieldSorter: "cast(numero, 'Edm.String')",*/
        sortable: true,
        type: FieldType.number,
        filterable: false,
        formatter: (row, cell, value, dataContext) =>
        (value = `<i style="
        cursor: pointer;
        background-color: #cccccc;
        width: 110px;
        height: 20px;
        text-align: center;
        border: solid 2px #aaaaaa;
        border-radius: 5px;" class="fa fa-clock-o" aria-hidden="true"> 2hr 03min</i>`)
      },
      {
        id: "renovacao",
        name: "SLA Renovação",
        field: "",
        /*queryFieldSorter: "cast(numero, 'Edm.String')",*/
        sortable: true,
        type: FieldType.number,
        filterable: false,
        formatter: (row, cell, value, dataContext) =>
        (value = `<i style="
                          cursor: pointer;
                          background-color: #cccccc;
                          width: 110px;
                          height: 20px;
                          text-align: center;
                          border: solid 2px #aaaaaa;
                          border-radius: 5px;" class="fa fa-calendar" aria-hidden="true"> 0 Dias</i>`)
      },
      {
        id: "edit",
        field: "",
        toolTip: "",
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        // formatter: Formatters.editIcon,
        formatter: (row, cell, value, dataContext) =>
        (value = `<i style="
        cursor: pointer;
        background-color: #cccccc;
        width: 23px;
        height: 20px;
        text-align: center;
        border: solid 2px #aaaaaa;
        border-radius: 5px;" class="fa fa-folder-open" aria-hidden="true"></i>`),
        minWidth: 30,
        maxWidth: 30,

        onCellClick: (e: Event, args: OnEventArgs) => {
          this.editar(args.dataContext);
        },
      },
      {
        id: "acompanhamento",
        field: "",
        toolTip: "",
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        // formatter: Formatters.editIcon,
        formatter: (row, cell, value, dataContext) =>
        (value = `<i style="
        cursor: pointer;
        background-color: #cccccc;
        width: 23px;
        height: 20px;
        text-align: center;
        border: solid 2px #aaaaaa;
        border-radius: 5px;" class="fa fa-book" aria-hidden="true"></i>`),
        minWidth: 30,
        maxWidth: 30,

        onCellClick: (e: Event, args: OnEventArgs) => {
          this.abrirSol(args.dataContext);
        },
      },
      {
        id: "agendamentoLigacao",
        field: "",
        toolTip: "",
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        // formatter: Formatters.editIcon,
        formatter: (row, cell, value, dataContext) =>
        (value = `<i style="
        cursor: pointer;
        background-color: #cccccc;
        width: 23px;
        height: 20px;
        text-align: center;
        border: solid 2px #aaaaaa;
        border-radius: 5px;" class="fa fa-phone" aria-hidden="true"></i>`),
        minWidth: 30,
        maxWidth: 30,
        onCellClick: (e: Event, args: OnEventArgs) => {
          this.agendamentoLigacao(args);
        },
      },
      {
        id: "btnsSolicitacao",
        name: "Ações",
        field: "",
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        formatter: (row, cell, value, dataContext) => (
          value = `
            <i id="1" style="
              display: ${this.atrAtendenteBtnDisplay(this.dataset[row].situacaoAtual.eFimFluxo)};
              cursor: pointer;
              background-color: #cccccc;
              width: 23px;
              height: 20px;
              text-align: center;
              border: solid 2px #aaaaaa;
              border-radius: 5px;" class="fa fa-user" aria-hidden="true">
            </i>
            <i id="2" style="
              display: ${this.atrOperadorBtnDisplay(this.dataset[row])};
              cursor: pointer;
              background-color: #cccccc;
              width: 23px;
              height: 20px;
              text-align: center;
              border: solid 2px #aaaaaa;
              border-radius: 5px;" class="fa fa-refresh" aria-hidden="true">
            </i>
            <i id="3" style="
              display: ${this.cancelarSolicitacaoBtnDisplay(this.dataset[row])};
              cursor: pointer;
              background-color: #cccccc;
              width: 23px;
              height: 20px;
              text-align: center;
              border: solid 2px #aaaaaa;
              border-radius: 5px;" class="fa fa-trash" aria-hidden="true">
            </i>
            <i id="4" style="
              display: ${this.permissaoBtnDisplay(this.dataset[row])};
              cursor: pointer;
              background-color: #cccccc;
              width: 23px;
              height: 20px;
              text-align: center;
              border: solid 2px #aaaaaa;
              border-radius: 5px;" class="fa fa-star" aria-hidden="true">
            </i>
          `),
        onCellClick: (e: Event, args: OnEventArgs) => {
          this.btnTargetHandler(e, args);
        },
        minWidth: 160,
        maxWidth: 160
      },
      {
        id: "numero",
        name: "N° Solicitação",
        field: "numero",
        /*queryFieldSorter: "cast(numero, 'Edm.String')",*/
        sortable: true,
        type: FieldType.number,
        filterable: false,
      },
      {
        id: "segurado",
        name: "Segurado",
        field: "segurado",
        sortable: true,
        queryFieldSorter: "segurado/nome",
        formatter(row, cell, value, dataContext) {
          return value?.nome;
        },
        filterable: false,
      },
      {
        id: "agencia",
        name: "Agência",
        field: "agencia",
        queryFieldSorter: "agencia/nome",
        formatter(row, cell, value, dataContext) {
          if (value != null) {
            return value.nome;
          } else {
            return "";
          }
        },
        filterable: false,
      },
      {
        id: "agenciaConta",
        name: "Agência Conta",
        field: "agenciaConta",
        queryFieldSorter: "agenciaConta/nome",
        formatter(row, cell, value, dataContext) {
          if (value != null) {
            return value.nome;
          } else {
            return "";
          }
        },
        sortable: true,
        filterable: false,
      },
      {
        id: "areaDeNegocio",
        name: "Área de Negócio",
        field: "areaDeNegocio",
        queryFieldSorter: "areaDeNegocio/nome",
        formatter(row, cell, value, dataContext) {
          if (value != null) {
            return value.nome;
          } else {
            return "";
          }
        },
        sortable: true,
        filterable: false,
      },
      {
        id: "situacaoAtual",
        name: "Situação Atual",
        field: "situacaoAtual",
        queryFieldSorter: "situacaoAtual/nome",
        formatter(row, cell, value, dataContext) {
          if (value != null) {
            return value.nome;
          } else {
            return "";
          }
        },
        sortable: true,
        filterable: false,
      },
      {
        id: "tipoCancelamento",
        name: "Tipo de cancelamento",
        field: "agencia",
        queryFieldSorter: "agencia/nome",
        formatter(row, cell, value, dataContext) {
          if (value != null) {
            return value.nome;
          } else {
            return "";
          }
        },
        filterable: false,
      },

      {
        id: "tipoDeSeguro",
        name: "Tipo De Seguro",
        field: "tipoDeSeguro",
        queryFieldSorter: "tipoDeSeguro/nome",
        formatter(row, cell, value, dataContext) {
          if (value != null) {
            return value.nome;
          } else {
            return "";
          }
        },
        sortable: true,
        filterable: false,
      },
      {
        id: "dataFimVigencia",
        name: "Data FIm Da VIgência",
        field: "dataFimVigencia",
        sortable: true,
        filterable: false,
      },
      {
        id: "atendente",
        name: "Atendente",
        field: "atendente",
        queryFieldSorter: "atendente/nome",
        formatter(row, cell, value, dataContext) {
          if (value != null) {
            return value.nome;
          } else {
            return "";
          }
        },
        sortable: true,
        filterable: false,
      },
      {
        id: "canalDeDistribuicao",
        name: "Canal De Distribuição",
        field: "canalDeDistribuicao",
        queryFieldSorter: "canalDeDistribuicao/nome",
        formatter(row, cell, value, dataContext) {
          if (value != null) {
            return value.nome;
          } else {
            return "";
          }
        },
        sortable: true,
        filterable: false,
      },
    ];

    this.gridOptions = {
      alwaysShowVerticalScroll: false,
      autoResize: {
        maxWidth: 1200,
      },
      enableEmptyDataWarningMessage: true,
      emptyDataWarning: {
        message: "Nenhum dado encontrado",
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
          this.getCallback(response);
          this.displaySpinner(false);
        },
      },
    };

    let returnPesquisa = localStorage.getItem('listPesquisa');
    if (returnPesquisa){
      this.form.setValue(JSON.parse(returnPesquisa));
      this.onSubmit();
    }
    /* console.log(returnPesquisa) */
  }

  btnTargetHandler(e, item) {
    let modalId = e.target.id

    if (modalId === "1") this.atribuirAtendente(item);
    else if (modalId === "2") this.atribuirOperador(item);
    else if (modalId === "3") this.remover(item);
    else if (modalId === "4") this.avaliarAtendimento(item);
  }

  atrAtendenteBtnDisplay(eFimFluxo) {
    let perm = false;
    this.usuarioGrupos.forEach(g => {
      if (g.atribuirAtendente == true) {
        perm = true;
      }
    });

    if (perm == false) {
      let loggedUser = this.authenticationService.getLoggedUser();
      if (loggedUser.ehAtendente && loggedUser.ehCalculista) perm = true;
    }

    if (perm == false || eFimFluxo) return `none`;
  }

  atrOperadorBtnDisplay(solicitacao) {
    let perm = false;
    this.usuarioGrupos.forEach(g => {
      if (g.atribuirOperador == true) {
        perm = true;
      }
    });

    let loggedUser = this.authenticationService.getLoggedUser();
    if (!loggedUser.ehAtendente && solicitacao.atendente.nome != "-") perm = true;

    if (perm == false || solicitacao.situacaoAtual.eFimFluxo) return `none`;
  }

  cancelarSolicitacaoBtnDisplay(solicitacao) {
    let perm = false;
    let loggedUser = this.authenticationService.getLoggedUser();
    this.usuarioGrupos.forEach(g => {
      if (g.cancelarSolicitacao == true) {
        perm = true;
      }
    });

    if (loggedUser.ehAtendente && solicitacao.tipoDeSeguro.nome === "Recontratação" && loggedUser.id == solicitacao.atendente.id) perm = true;

    if (perm == false || solicitacao.situacaoAtual.eFimFluxo) return `none`;
  }

  permissaoBtnDisplay(solicitacao) {
    if (!solicitacao.situacaoAtual.eFimFluxo) {
      return `none`;
    } else if (!this.authenticationService.getLoggedUser().ehSolicitante ) {
      return `none`;
    }
  }  /* && solicitacao.avaliacoes.length < 1 */

  searchAtendentes(event) {
    let query = `$filter=contains(nome, '${event.query}') eq true`;
    this.usuarioService.get(query).subscribe((data) => {
      this.atendentes = data.value;
    });
  }

  searchSeguradora(event) {
    let query = `$filter=contains(nm_Seguradora, '${event.query}') eq true`;
    this.vW_SEGURADORAService.get(query).subscribe((data) => {
      this.vwSeguradora = data.value;
    });
  }

  searchSuperitendencia(event) {
    let query = `$filter=contains(super, '${event.query}') eq true`;
    this.agenciaService.get(query).subscribe((data) => {
      this.superintendencias = data.value;
    });
  }

  searchSuperContas(event) {
    let query = `$filter=contains(super, '${event.query}') eq true`;
    this.agenciaService.get(query).subscribe((data) => {
      this.superContas = data.value;
    });
  }

  searchAgecias(event) {
    let query = `$filter=contains(nome, '${event.query}') eq true`;
    this.agenciaService.get(query).subscribe((data) => {
      this.agencias = data.value;
    });
  }

  searchAgeciasConta(event) {
    let query = `$filter=contains(nome, '${event.query}') eq true`;
    this.agenciaService.get(query).subscribe((data) => {
      this.agenciasConta = data.value;
    });
  }

  searchSituacao(event) {
    let query = `$filter=contains(nome, '${event.query}') eq true`;
    this.situacaoService.get(query).subscribe((data) => {
      this.situacoes = data.value;
    });
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
  }

  private displaySpinner(isProcessing) {
    this.loadingDataGrid = isProcessing;
  }

  getData(query: string) {
    /* this.odataQuery = localStorage.getItem('_OdataQuery') || query; */
   /*  localStorage.setItem('_OdataQuery',query); */
    this.odataQuery = query;
    if (this.filters) {
      this.onSubmit();
    } else {
      /*let data = new Date();
      data.setTime(data.getTime() - 1296000000);
      let dataInicioPesquisa = moment(data).format('Y-M-D');*/
      let dataInicioPesquisa = "2020-05-01";
      // )

      // console.log(
      //   "this.authenticationService.getLoggedUser()",
      //   this.authenticationService.getLoggedUser()
      // );

      if (this.authenticationService.getLoggedUser().ehSolicitante) {
        return this.solicitacaoService.getAll(
          `$select=numero,dadosAdicionais,dataDeIngresso,operacaoDeFinanciamento,origem,dataFimVigencia,orcamentoPrevio,mercado,id&$expand=segurado($expand=vinculoBNB),solicitante,seguradora($select=nm_seguradora,id),agencia($select=nome,id),tipoDeSeguro,tipoDeCancelamento($select=descricao),atendente($select=nome),situacaoAtual($select=nome,eFimFluxo,pendenciacliente),solicitante,areaDeNegocio,tipoDeProduto($select=id,nome),canalDeDistribuicao,segmento,areaDeNegocio,anexos,situacaoAtual,indicacoes,agenciaConta($select=nome,id),produtor($select=nome,id)&$orderby=dataDeIngresso desc&$filter=(solicitante/id eq ${this.authenticationService.getLoggedUser().id}) &${query}`);
      } else {
        let lista = this.authenticationService
          .getLoggedUser()
          .areasDeNegocio.split(",");

        if (query.indexOf("filter") >= 0) {
          return this.solicitacaoService.getAll(
            `$select=numero,dadosAdicionais,dataDeIngresso,operacaoDeFinanciamento,origem,dataFimVigencia,orcamentoPrevio,mercado,id&$expand=segurado($expand=vinculoBNB),solicitante,seguradora($select=nm_seguradora,id),agencia($select=nome,id),tipoDeSeguro,tipoDeCancelamento($select=descricao),atendente($select=nome),situacaoAtual($select=nome,eFimFluxo,pendenciacliente),solicitante,areaDeNegocio,tipoDeProduto($select=id,nome),canalDeDistribuicao,segmento,areaDeNegocio,anexos,situacaoAtual,indicacoes,agenciaConta($select=nome,id),produtor($select=nome,id)&$orderby=dataDeIngresso desc&${query} and (areaDeNegocio/id in (${lista}))`
          );
        }

        return this.solicitacaoService.getAll(
          `$select=numero,dadosAdicionais,dataDeIngresso,operacaoDeFinanciamento,origem,dataFimVigencia,orcamentoPrevio,mercado,id&$expand=segurado($expand=vinculoBNB),solicitante,seguradora($select=nm_seguradora,id),agencia($select=nome,id),tipoDeSeguro,tipoDeCancelamento($select=descricao),atendente($select=nome),situacaoAtual($select=nome,eFimFluxo,pendenciacliente),solicitante,areaDeNegocio,tipoDeProduto($select=id,nome),canalDeDistribuicao,segmento,areaDeNegocio,anexos,situacaoAtual,indicacoes,agenciaConta($select=nome,id),produtor($select=nome,id)&$orderby=dataDeIngresso desc&$filter=(atendente/id eq ${this.authenticationService.getLoggedUser().id}) &${query}`);
      }

      //   this.odataQuery = "";
      //   let select = `$select=numero,dadosAdicionais,dataDeIngresso,operacaoDeFinanciamento,origem,dataFimVigencia,orcamentoPrevio,mercado,id`;
      //   let expand = `&$expand=segurado($expand=vinculoBNB),solicitante,agencia($select=nome,id),tipoDeSeguro,tipoDeCancelamento($select=descricao),atendente($select=nome),situacaoAtual($select=nome,eFimFluxo,pendenciacliente),solicitante,areaDeNegocio,tipoDeProduto($select=id,nome),canalDeDistribuicao,segmento,areaDeNegocio,anexos,situacaoAtual,indicacoes,agenciaConta($select=nome,id),produtor($select=nome,id)`;
      //   let orderBy = `&$orderby=dataDeIngresso desc`
      //   let filter = "&$filter=";
      //   let contAndTop = null;

      //   if (this.authenticationService.getLoggedUser().ehSolicitante) {
      //     this.odataQuery += `&$filter=(solicitante/id eq ${this.authenticationService.getLoggedUser().id})
      // )`;
      //   }
      //   else {
      //     let areas = this.authenticationService.getLoggedUser().areasDeNegocio.split(',');
      //     this.odataQuery += `&$filter=(areaDeNegocio/id in (${areas}))
      // )`;
      //   }

      //   if (query.includes("$filter")) {
      //     let querySplit = query.split("&$filter=");
      //     query += `and ${querySplit[1]}&${querySplit[0]}`
      //   }

      //   this.odataQuery += `${orderBy}&${query}`;
      //   return this.solicitacaoService.getAll(`${select}${expand}${this.odataQuery}`);
      // return this.solicitacaoService.getAll(`$select=numero,dadosAdicionais,dataDeIngresso,operacaoDeFinanciamento,origem,dataFimVigencia,orcamentoPrevio,mercado,id&$expand=segurado($expand=vinculoBNB),solicitante,agencia($select=nome,id),tipoDeSeguro,tipoDeCancelamento($select=descricao),atendente($select=nome),situacaoAtual($select=nome,eFimFluxo,pendenciacliente),solicitante,areaDeNegocio,tipoDeProduto($select=id,nome),canalDeDistribuicao,segmento,areaDeNegocio,anexos,situacaoAtual,indicacoes,agenciaConta($select=nome,id),produtor($select=nome,id)&$orderby=dataDeIngresso desc&&${query}`);
    }
  }

  reloadGrid() {
    this.angularGrid.gridService.resetGrid();
  }

  handleEventoConcluido(retorno) {
    if (retorno) {
      this.solicitacaoSelecionado = null;
      this.display = false;
      this.reloadGrid();

      Swal.fire(
        "",
        `Solicitação ${retorno.adicionar ? "adicionada" : "atualizada"}!`,
        "success"
      );
    }
  }

  openPanel() {
    // this.solicitacaoSelecionado = null;
    // this.display = true;
  }

  handleClosePanel(fechar) {
    if (fechar) {
      this.solicitacaoSelecionado = null;
      this.display = false;
      this.reloadGrid();
    }
  }

  remover(item) {
    let perm = false;
    let loggedUser = this.authenticationService.getLoggedUser();
    this.usuarioGrupos.forEach(g => {
      if (g.cancelarSolicitacao == true) {
        perm = true;
      }
    });

    if (loggedUser.ehAtendente && item.dataContext.tipoDeSeguro.nome === "Recontratação" && loggedUser.id == item.dataContext.atendente.id) perm = true;

    if (perm && !item.dataContext.situacaoAtual.eFimFluxo) {
      Swal.fire({

        title: "Atenção!",
        text: `Deseja excluir a solicitação: ${item.dataContext.numero}?`,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#5cd65c",
        cancelButtonColor: "#ff3333",
        confirmButtonText: "Sim",
        cancelButtonText: "Não",
        showLoaderOnConfirm: true,
        preConfirm: () => {
          this.solicitacaoService.deletar(item.dataContext.id).subscribe((response) => {
            if (response.successfully) {
              this.reloadGrid();
            }
          });
        },
      }).then((result) => {
        if (result.value) {
          Swal.fire("", "A solicitação foi deletado.", "success");
        }
      });
    }
  }

  onSubmit() {

    let query =
      "$select=numero,aplicacao,dadosAdicionais,dataDeIngresso,operacaoDeFinanciamento,origem,dataFimVigencia,orcamentoPrevio,mercado,id&$expand=avaliacoes,segurado($expand=vinculoBNB),solicitante,seguradora($select=nm_seguradora,id),agencia($select=nome,id),tipoDeSeguro,tipoDeCancelamento($select=descricao),atendente($select=nome,id),situacaoAtual($select=nome,eFimFluxo,pendenciacliente),solicitante,areaDeNegocio,tipoDeProduto($select=id,nome),canalDeDistribuicao,segmento,areaDeNegocio,anexos,situacaoAtual,indicacoes,agenciaConta($select=nome,id),produtor($select=nome,id)&$orderby=dataDeIngresso desc";
    let filter = "$filter=";

    // console.log('this.form.value', this.form.value)
    // console.log('this.form.value', new Date(this.form.value.dataInicial))

    var date1 = new Date(this.form.value.dataInicial)
    var date2 = new Date(this.form.value.dataFInal)
    var diff = date2.getTime() - date1.getTime()
    var diffdias = diff / (1000 * 3600 * 24)

    if (diffdias < 0) {
      Swal.fire("", "Data Inicial não pode superior a data Final", "warning");
    }

    if (diffdias > 60) {
      Swal.fire("", "O intervalo de datas não de pode ser superior a 60 dias.", "warning");
    }

    if (this.form.value.atendente != "") {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(atendente/nome eq '${this.form.value.atendente.nome}')`;
    }

    if (this.form.value.numeroSolicitacao) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(numero eq ${this.form.value.numeroSolicitacao})`;
    }

    if (this.form.value.canalDistribuicao) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(canalDeDistribuicao/id eq ${this.form.value.canalDistribuicao})`;
    }

    if (this.form.value.segurado) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(segurado/nome eq '${this.form.value.segurado}')`;
    }

    if (this.form.value.tipoCancelamento) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(tipoDeCancelamento/descricao eq '${this.form.value.tipoCancelamento}')`;
    }

    if (this.form.value.agencia) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(agencia/id eq ${this.form.value.agencia.id})`;
    }

    if (this.form.value.seguradora) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(seguradora/id eq ${this.form.value.seguradora.id})`;
    }

    if (this.form.value.agencia) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(agencia/id eq ${this.form.value.agencia.id})`;
    }

    if (this.form.value.agenciaConta) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(agenciaConta/id eq ${this.form.value.agenciaConta.id})`;
    }

    if (this.form.value.tipoSeguro) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(tipoDeSeguro/id eq ${this.form.value.tipoSeguro})`;
    }

    if (this.form.value.situacao) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(situacaoAtual/nome eq '${this.form.value.situacao.nome}')`;
    }

    if (this.form.value.cnpj) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(segurado/cpfCnpj eq '${this.form.value.cnpj}')`;
    }

    if (this.form.value.vinculoBnb) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(segurado/vinculoBNB/id eq ${this.form.value.vinculoBnb})`;
    }

    if (this.form.value.areaDeNegocio) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(areaDeNegocio/id eq ${this.form.value.areaDeNegocio})`;
    }

    if (this.form.value.listar) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      if (this.form.value.listar == "todos") {
        filter += `(tipoDeSeguro/id ge 1)`;
      } else if (this.form.value.listar == "renovacao") {
        filter += `(tipoDeSeguro/id eq 2)`;
      } else if (this.form.value.listar == "outros") {
        filter += `(tipoDeSeguro/id ne 2)`;
      }
    }

    if (this.form.value.ramoDeSeguro) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(tipoDeProduto/id eq ${this.form.value.ramoDeSeguro})`;
    }

    if (this.form.value.dataInicial || this.form.value.dataFInal) {
      console.log(this.form.value.dataInicial, this.form.value.dataFInal);

      if (this.form.value.dataInicial.length == 0) {
        alert("Escolha uma data inicial para executar a filtragem");
        return;
      }

      if (this.form.value.dataFInal.length == 0) {
        alert("Escolha uma data final para executar a filtragem");
        return;
      }

      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(dataDeIngresso ge ${this.form.value.dataInicial} and dataDeIngresso le ${this.form.value.dataFInal})`;
    }

    if (this.form.value.emProcesso) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(situacaoAtual/eFimFluxo eq false)`;
    }

    if (this.form.value.superConta) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(agencia/superId eq ${this.form.value.superConta.superId})`;
    }

    if (this.form.value.superintendencia) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      filter += `(agencia/superId eq ${this.form.value.superintendencia.superId})`;
    }

    if (this.form.value.statusSituacao) {
      if (this.addAnd) {
        filter += "and";
      }
      this.addAnd = true;
      if (this.form.value.statusSituacao == "interna") {
        filter += `(situacaoAtual/pendenciaCliente eq false)`;
      } else if (this.form.value.statusSituacao == "externa") {
        filter += `(situacaoAtual/pendenciaCliente eq true)`;
      }
    }

    if (this.addAnd) {
      this.solicitacaoService
        .getAll(`${query}&${filter}&${this.odataQuery}`)
        .subscribe((data) => {
          this.getCallback(data);
        });
      this.addAnd = false;
      this.filters = true;
    }

    localStorage.setItem('listPesquisa', JSON.stringify(this.form.value));
  }

  abrirSol(item) {
    this.router.navigate([`historico`], {
      relativeTo: this.route,
      state: item,
    });

    // this.router.navigate([`editar/${item.id}`], {
    //   relativeTo: this.route,
    //   state: item,
    // });
  }

  historicoacompanhamento(item) {
    this.router.navigate([`acompanhamneto/${item.id}`], {
      relativeTo: this.route,
      state: item,
    });
  }

  mostrarBotaoRetornoLigacao(): boolean {
    var result =
      this.authenticationService.getLoggedUser().id ==
      this.solicitacaoSelecionado.atendente.id;
    return result;
  }

  salvarAtendente() {
    console.log(this.conteudoParaSalvarAtendente)
    console.log(this.atendenteSelecionado, "Atendente")
    console.log(this.areaDeNegocioSelecionada, "AreaDeNegocio");

    if (this.atendenteSelecionado == null && (this.areaDeNegocioSelecionada == undefined || this.areaDeNegocioSelecionada == "")) {
      $("#alertAtendente").css("visibility", "visible")
      $("#alertArea").css("visibility", "visible")
      return;
    }

    if (this.atendenteSelecionado == null) {
      $("#alertAtendente").css("visibility", "visible")
      $("#alertArea").css("visibility", "hidden")
      return;
    }

    if (this.areaDeNegocioSelecionada == undefined || this.areaDeNegocioSelecionada == "") {
      $("#alertAtendente").css("visibility", "hidden")
      $("#alertArea").css("visibility", "visible")
      return;
    }

    const solicitacaoId = this.conteudoParaSalvarAtendente.Solicitacao_Id;

    this.conteudoParaSalvarAtendente = {
      Solicitacao_Id: solicitacaoId,
      Atendente_Id: this.atendenteSelecionado.id,
      AreaDeNegocio_Id: this.areaDeNegocioSelecionada
    }

    console.log(this.conteudoParaSalvarAtendente)

    this.solicitacaoService.salvarAtendente(this.conteudoParaSalvarAtendente).subscribe((response) => {
      console.log(response);
    });

    ($("#atribuirAtendente") as any).modal("hide");

    Swal.fire(
      '',
      `Atendente Atribuido com Sucesso!`,
      'success'
    );

    this.atendenteSelecionado = null;
    this.areaDeNegocioSelecionada = undefined
  }

  atribuirAtendente(item) {
    let perm = false;
    this.usuarioGrupos.forEach(g => {
      if (g.atribuirAtendente == true) perm = true;
    });

    if (perm == false) {
      let loggedUser = this.authenticationService.getLoggedUser();
      if (loggedUser.ehAtendente && loggedUser.ehCalculista) perm = true;
    }

    if (perm == true && !item.dataContext.situacaoAtual.eFimFluxo) {
      $("#alertAtendente").css("visibility", "hidden")
      $("#alertArea").css("visibility", "hidden")
      this.solicitacaoSelecionado = item.dataContext;

      this.conteudoParaSalvarAtendente = {
        Solicitacao_Id: this.solicitacaoSelecionado.id
      };

      ($("#atribuirAtendente") as any).modal("show");
    };
  }

  agendamentoLigacao(item) {
    this.solicitacaoSelecionado = item.dataContext;
    this.agendamentoDeLigacaoService
      .get(
        `$select=id,dataAgendamento,motivo,solicitacao_Id,tipoRetornoLigacao_Id&$orderby=dataAgendamento%20desc&$filter=solicitacao_Id eq ${this.solicitacaoSelecionado.id}`
      )
      .subscribe((data) => {
        this.agendamentosDeLigacao = data.value;
      });
    $("#agendamentoLigacaoModalLabel").text(
      "Solicitação: " + this.solicitacaoSelecionado.id
    );

    ($("#agendamentoLigacaoModal") as any).modal("show");

    if (
      this.authenticationService.getLoggedUser().id !==
      this.solicitacaoSelecionado.atendente.id
    ) {
      document.getElementById("novoAgendamento").classList.toggle("exibir");
    }
  }

  retornoAgendamento() {
    ($("#agendamentoLigacaoModal") as any).modal("toggle");
    this.agendamentoDeLigacaoService
      .get(
        `$select=id,dataAgendamento,motivo,solicitacao_Id,tipoRetornoLigacao_Id&$orderby=dataAgendamento%20desc&$filter=solicitacao_Id eq ${this.solicitacaoSelecionado.id}`
      )
      .subscribe((data) => {
        this.agendamentosDeLigacao = data.value;
      });

    var tiporetorno = "";
    this.tiporetorno$.map((retorno) => {
      tiporetorno += `<option value=${retorno.id} [ngValue]=${retorno.id}> ${retorno.descricao} </option>`;
    });
    Swal.fire({
      title: "Retorno de Ligação",
      html: `
       <label>Data Ligação</label>
       <input id="dataRetorno" class="form-control" type="date" formControlName="dataligacao" / >

       <div class="form-group">
         <label>Tipo Retorno Ligação</label>
         <br>
         <div *ngIf="(this.tiporetorno$ | async) as tipos">
          <select class="form-control" id="optionT" formControlName="tiporetornoligacao_id"
            [ngClass]="{ 'is-invalid': submitted && invalid('tiporetornoligacao_id') }">
            ${tiporetorno}
          </select>
          </div>
        </div> `,
      text: ``,
      showCancelButton: true,
      confirmButtonColor: "#5cd65c",
      cancelButtonColor: "#ff3333",
      confirmButtonText: "Cadastrar",
      cancelButtonText: "Cancelar",
      showLoaderOnConfirm: true,
      preConfirm: () => {
        const post = {
          id: this.agendamentosDeLigacao[0].id,
          motivo: this.agendamentosDeLigacao[0].motivo,
          dataagendamento: moment(
            this.agendamentosDeLigacao[0].dataAgendamento
          ).format("DD/MM/yyyy"),
          nsolicitacao: this.solicitacaoSelecionado.id,
          tipoRetornoLigacao: (<HTMLSelectElement>(
            document.getElementById("optionT")
          )).value,
          dataLigacao: moment(
            (<HTMLDataElement>document.getElementById("dataRetorno")).value
          ).format("DD/MM/yyyy"),
        };

        this.agendamentoDeLigacaoService.put(post).subscribe(
          (response) => {
            this.setResult(response);
            if (response.successfully) {
              this.eventoConcluido.emit({
                adicionar: this.agendamentosDeLigacao[0].id != 0,
              });
            }
          },
          (error) => this.showError(error)
        );
      },
    }).then((result) => {
      if (result.value) {
        Swal.fire("", "Retorno cadastrado.", "success");
      }
    });
  }

  NovoAgendamento() {
    ($("#agendamentoLigacaoModal") as any).modal("toggle");

    Swal.fire({
      title: "Novo Agendamento",
      html: `
      <label for="example-time-input">Data Agendamento *</label>
      <input required class="form-control" type="date" id="dataagendamento" [ngClass]="{ 'is-invalid': submitted && invalid('nome') }">
      <label>Motivo *</label>
      <input type="text" placeholder="Motivo" class="form-control" id="motivo" [ngClass]="{ 'is-invalid': submitted && invalid('nome') }">
      `,
      text: ``,
      showCancelButton: true,
      confirmButtonColor: "#5cd65c",
      cancelButtonColor: "#ff3333",
      confirmButtonText: "Cadastrar",
      cancelButtonText: "Cancelar",
      showLoaderOnConfirm: true,
      preConfirm: () => {
        const post = {
          id: 0,
          dataagendamento: (<HTMLDataElement>(
            document.getElementById("dataagendamento")
          )).value
            ? moment(
              (<HTMLDataElement>document.getElementById("dataagendamento"))
                .value
            ).format("DD/MM/yyyy")
            : null,
          nsolicitacao: this.solicitacaoSelecionado.id,
          motivo: (<HTMLInputElement>document.getElementById("motivo")).value,
        };
        this.agendamentoDeLigacaoService.save(post).subscribe(
          (response) => {
            this.setResult(response);
            if (response.successfully) {
              this.eventoConcluido.emit({
                adicionar: this.agendamentosDeLigacao[0].id == 0,
              });
            }
          },
          (error) => this.showError(error)
        );
      },
    }).then((result) => {
      if (result.value) {
        Swal.fire("", "Agendamento cadastrado.", "success");
      }
    });
  }

  histDeAcom(item) {
    if (item.dataContext.situacaoAtual.eFimFluxo) {
      ($("#exampleModal") as any).modal("show");
      document.getElementById("numero").innerHTML = item.dataContext.numero;
      document.getElementById("produtorNome").innerHTML =
        item.dataContext.produtor.nome;
      document.getElementById("seguradoNome").innerHTML =
        item.dataContext.segurado.nome;
      document.getElementById("seguradoCpfCnpj").innerHTML =
        item.dataContext.segurado.cpfCnpj;
      document.getElementById("seguradoVinculoBNBNome").innerHTML =
        item.dataContext.segurado.vinculoBNB.nome;
      document.getElementById("seguradoEmail").innerHTML =
        item.dataContext.segurado.email;
      document.getElementById("seguradoTelefonePrincipal").innerHTML =
        item.dataContext.segurado.telefonePrincipal;
      document.getElementById("seguradoTelefoneCelular").innerHTML =
        item.dataContext.segurado.telefoneCelular;
      document.getElementById("seguradoTelefoneAdicional").innerHTML =
        item.dataContext.segurado.telefoneAdicional;
      document.getElementById("operacaoDeFinanciamento").innerHTML =
        item.dataContext.operacaoDeFinanciamento;
      document.getElementById("origem").innerHTML = item.dataContext.origem;
      document.getElementById("orcamentoPrevio").innerHTML =
        item.dataContext.orcamentoPrevio;
      document.getElementById("segmentoNome").innerHTML =
        item.dataContext.segmento.nome;
      document.getElementById("produtorNome").innerHTML =
        item.dataContext.produtor.nome;
      document.getElementById("CrossUP").innerHTML = item.dataContext.CrossUP;
      document.getElementById("mercado").innerHTML = item.dataContext.mercado;
      document.getElementById("participaCampanha").innerHTML = "participa";
      document.getElementById("indicacoes").innerHTML =
        item.dataContext.indicacoes.value != null
          ? item.dataContext.indicacoes.value
          : "Não possui";
      document.getElementById("seguradora").innerHTML =
        item.dataContext.seguradora != null
          ? item.dataContext.seguradora
          : "Não possui";
      document.getElementById("ramo_Id").innerHTML =
        item.dataContext.ramo_Id != null
          ? item.dataContext.ramo_Id
          : "Não possui";
      document.getElementById("num_Proposta_Seguradora").innerHTML =
        item.dataContext.num_Proposta_Seguradora;
      document.getElementById("tipoDeSeguroNome").innerHTML =
        item.dataContext.tipoDeSeguro.nome;
      document.getElementById("FL_Forma_Pagamento_Demais_Id").innerHTML =
        item.dataContext.FL_Forma_Pagamento_Demais_Id;
      document.getElementById("rastreador").innerHTML =
        item.dataContext.rastreador;
      document.getElementById("rechaco").innerHTML = item.dataContext.rechaco;
      document.getElementById("grupoDeProducao_Id").innerHTML =
        item.dataContext.grupoDeProducao_Id;
      document.getElementById("cd_Estudo").innerHTML = item.dataContext.cd_Estudo;
      document.getElementById("nu_Apolice_Anterior").innerHTML =
        item.dataContext.nu_Apolice_Anterior;
      document.getElementById("pc_comissao").innerHTML =
        item.dataContext.pc_comissao;
      document.getElementById("co_Corretagem").innerHTML =
        item.dataContext.co_Corretagem;
      document.getElementById("pc_agenciamento").innerHTML =
        item.dataContext.pc_agenciamento;
      document.getElementById("vL_IS").innerHTML = item.dataContext.vL_IS;
      document.getElementById("FL_Forma_Pagamento_1a_Id").innerHTML =
        item.dataContext.FL_Forma_Pagamento_1a_Id;
      document.getElementById("Sede_Envia_Doc_Fisico").innerHTML =
        item.dataContext.Sede_Envia_Doc_Fisico;
      document.getElementById("vistorianec").innerHTML =
        item.dataContext.vistorianec;
      document.getElementById("cadastro_GS").innerHTML =
        item.dataContext.cadastro_GS != null
          ? item.dataContext.cadastro_GS
          : "Não possui";
      document.getElementById("situacaoAtualNome").innerHTML =
        item.dataContext.situacaoAtual.nome;

      document.getElementById("atendenteNome").innerHTML =
        item.dataContext.atendente.nome;
      document.getElementById("dataDeIngresso").innerHTML = moment(
        item.dataContext.dataDeIngresso
      ).format("DD/MM/yyyy HH:mm");
      this.anexosSolicitacao = item.dataContext.anexos;

      // let anexos = "";
      // for(let i = 0; i < item.dataContext.anexos.length; i++) {
      //   let nomeAnexo = item.dataContext.anexos[i].nome;
      //   anexos = anexos + " " + nomeAnexo
      // }
      // document.getElementById("anexos").innerHTML = anexos != null ?anexos :"Não possui" ;

      $("#checkList").click(() => {
        ($("#modalCheck") as any).modal("toggle");
        ($("#exampleModal") as any).modal("toggle");
        document.getElementById("numeroSol").innerHTML = item.dataContext.numero;

        this.anexosSolicitacao = item.dataContext.anexos;
      });

      $("#NovoAcompanhamento").click(() => {
        ($("#modalCheck") as any).modal("toggle");
        ($("#exampleModal") as any).modal("toggle");
      });
    }
  }

  onSave() {
    document.getElementById("trExecutante").classList.toggle("exibir");
  }

  editar(item) {
    this.router.navigate(["form"], {
      relativeTo: this.route,
      state: item,
    });

    this.solicitacaoSelecionado = item;
    // this.display = true;
  }

  acompanhamneto(item) {
    this.router.navigate(["form"], {
      relativeTo: this.route
    })
  }


  onClear() {
    this.uploadedFiles = new Array<any>();
  }

  UploadFiles(event: any) {
    if (event.files[0].size <= 1000000) {
      this.solicitacaoService
        .formatFile(this.solicitacaoSelecionado.id, <File[]>event.files)
        .subscribe((response) => {
          this.uploadedFiles.push(response.payload[0]);
          console.log(this.uploadedFiles);
        });
    }
  }

  enviarSolicitacao() {
    if (!this.dadosParaNovoAcompanhamento.Acao_Id) {
      $('#acaoSelectAlert').css("visibility", "visible");
      return;
    }

    this.solicitacaoService.inserirAcompanhamento(this.dadosParaNovoAcompanhamento).subscribe((response) => {
      console.log(response)
      Swal.fire(
        '',
        `Situação Atualizada!`,
        'success'
      );
    });

    ($('#exampleModal') as any).modal('hide');
    this.uploadedFiles = [];
  }

  abrirModalSolicitacao(item) {
    this.solicitacaoSelecionado = item;

    this.dadosParaNovoAcompanhamento = {
      Situacao: this.solicitacaoSelecionado.situacaoAtual, // Ok
      Atendente: this.solicitacaoSelecionado.atendente,
      Grupo: null, // Ok
      Solicitacao_Id: this.solicitacaoSelecionado.id, // Ok
      TempoSLADef: this.solicitacaoSelecionado.situacaoAtual.tempoSLA,
      Acao_Id: null
    };

    console.log(this.dadosParaNovoAcompanhamento, 'dadosParaNovoAcompanhamento');
    console.log(this.solicitacaoSelecionado, 'solicitacaoSelecionado');

    $('.invalid-feedback').css("visibility", "hidden");
    ($('#exampleModal') as any).modal('show');
  }

  avaliarAtendimento(item) {
    this.solicitacaoIdAtendimento = item.dataContext.id;
    this.obsAvAtendimento = "";
    $("#alertObsAtendimento").css("visibility", "hidden");

    if (item.dataContext.situacaoAtual.eFimFluxo) {
      let avaliacao = item.dataContext.avaliacoes;
      if (avaliacao.length < 1 && this.authenticationService.getLoggedUser().ehSolicitante) {
        this.isDisabled = false;
        this.notaDinamica = '0';
        this.notaDoAtendimento = "";
        let element = <HTMLInputElement>document.getElementById('rating2-0');
        element.checked = true;
        $("#alertAvAtendimento").css("visibility", "hidden");
        ($("#avaliarAtendimento") as any).modal("show");
      } else if (avaliacao.length >= 1) {
        this.isDisabled = true;
        this.obsAvAtendimento = avaliacao[0].observacao;
        $("#alertAvAtendimento").css("visibility", "hidden");
        ($("#avaliarAtendimento") as any).modal("show");

        if (avaliacao[0].nota == "5") {
          let element = <HTMLInputElement>document.getElementById("rating2-50");
          element.checked = true;
          this.notaDinamica = '5';
        }

        else if (avaliacao[0].nota == "4.5") {
          let element = <HTMLInputElement>document.getElementById("rating2-45");
          element.checked = true;
          this.notaDinamica = '4.5';
        }

        else if (avaliacao[0].nota == "4") {
          let element = <HTMLInputElement>document.getElementById("rating2-40");
          element.checked = true;
          this.notaDinamica = '4';
        }

        else if (avaliacao[0].nota == "3.5") {
          let element = <HTMLInputElement>document.getElementById("rating2-35");
          element.checked = true;
          this.notaDinamica = '3.5';
        }

        else if (avaliacao[0].nota == "3") {
          let element = <HTMLInputElement>document.getElementById("rating2-30");
          element.checked = true;
          this.notaDinamica = '3';
        }

        else if (avaliacao[0].nota == "2.5") {
          let element = <HTMLInputElement>document.getElementById("rating2-25");
          element.checked = true;
          this.notaDinamica = '2.5';
        }

        else if (avaliacao[0].nota == "2") {
          let element = <HTMLInputElement>document.getElementById("rating2-20");
          element.checked = true;
          this.notaDinamica = '2';
        }

        else if (avaliacao[0].nota == "1.5") {
          let element = <HTMLInputElement>document.getElementById("rating2-15");
          element.checked = true;
          this.notaDinamica = '1.5';
        }

        else if (avaliacao[0].nota == "1") {
          let element = <HTMLInputElement>document.getElementById("rating2-10");
          element.checked = true;
          this.notaDinamica = '1';
        }

        else if (avaliacao[0].nota == "0.5") {
          let element = <HTMLInputElement>document.getElementById("rating2-05");
          element.checked = true;
          this.notaDinamica = '0.5';
        }

        else if (avaliacao[0].nota == "0") {
          let element = <HTMLInputElement>document.getElementById("rating2-0");
          element.checked = true;
          this.notaDinamica = '0';
        }
      }
    }
  }

  setNotaDinamica(nota) {
    if (!this.isDisabled) this.notaDinamica = nota;
  }

  notaAtual() {
    if (this.notaDoAtendimento && !this.isDisabled) {
      this.notaDinamica = this.notaDoAtendimento;
    } else if (!this.isDisabled) {
      this.notaDinamica = '0';
    }
  }

  notaParaAtendimento(nota) {
    if (!this.isDisabled) this.notaDoAtendimento = nota;
  }

  salvarAvAtendimento() {
    //"2023-01-20T13:29:07.208Z"
    const atendimentoSolicitacao = {
      observacao: this.obsAvAtendimento,
      nota: this.notaDoAtendimento,
      solicitacao_Id: this.solicitacaoIdAtendimento,
      dataAvaliacao: new Date(),
      usuario_Id: this.authenticationService.getLoggedUser().id
    }

    if (this.obsAvAtendimento.length <= 0 && this.notaDoAtendimento != "5") {
      $("#alertObsAtendimento").css("visibility", "visible");
    } else {
      this.avAtendimentoService.save(atendimentoSolicitacao).subscribe((response) => {
        console.log(response);
      });
    }
  }

  atribuirOperador(item) {
    let perm = false;
    this.usuarioGrupos.forEach(g => {
      if (g.atribuirOperador == true) {
        perm = true;
      }
    });

    let loggedUser = this.authenticationService.getLoggedUser();
    if (!loggedUser.ehAtendente && item.dataContext.atendente.nome != "-") perm = true;

    if (perm && !item.dataContext.situacaoAtual.eFimFluxo) {
      $("#alertOperador").css("visibility", "hidden")
      this.solicitacaoSelecionado = item.dataContext;

      this.conteudoParaSalvarAtendente = {
        Solicitacao_Id: this.solicitacaoSelecionado.id
      };

      ($("#atribuirOperador") as any).modal("show");
    }
  }

  permissaoCriarSolicitacao() {
    let loggedUser = this.authenticationService.getLoggedUser();
    if(loggedUser.ehAtendente || loggedUser.ehSolicitante) {return true;} else {return false};
  }


  ClearFields() {
    this.form = this.fb.group({
      listar: "",
      atendente: "",
      numeroSolicitacao: "",
      canalDistribuicao: "",
      segurado: "",
      tipoCancelamento: "",
      dataInicial: "",
      dataFInal: "",
      tipoSeguro: "",
      ramoDeSeguro: "",
      seguradora: "",
      areaDeNegocio: "",
      vinculoBnb: "",
      cnpj: "",
      observacao: "",
      numProposta: "",
      emProcesso: "",
      emAtraso: "",
      superintendencia: "",
      superConta: "",
      agencia: "",
      agenciaConta: "",
      situacao: "",
      statusSituacao: "",
    });
  }
}
