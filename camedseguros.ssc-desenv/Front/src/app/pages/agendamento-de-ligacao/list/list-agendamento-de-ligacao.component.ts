import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder } from "@angular/forms";

import Swal from "sweetalert2";

import {
  BaseComponent,
  AuthenticationService,
  User,
  AnexoInbox,
} from "src/app/core";
import {
  AngularGridInstance,
  Column,
  GridOption,
  FieldType,
  Filters,
  // GridOdataService,
  Metrics,
  Formatters,
  OnEventArgs,
  HtmlElementPosition,
} from "angular-slickgrid";
import { GridOdataService } from "@slickgrid-universal/odata";

import { UsuarioService } from "../../usuario";
import { EmpresaService } from "../../empresas";
import { RetonoligacaoService } from "../../tiporetornoligacao";
import { TipoSeguroService } from "../../tiposdeseguro";
import { GrupoAgenciaService } from "../../grupoagencias";
import { AgenciaService } from "src/app/core";
import { AreaDeNegocioService } from "../../areadenegocio";
import { AgendamentoDeLigacaoServices } from "../service";
import { SelectItem } from "primeng/api";
import * as moment from "moment";
import { Observable } from "rxjs";
import { any } from "underscore";
import { query } from "@angular/animations";

@Component({
  selector: "app-list-agendamento-de-ligacao",
  templateUrl: "./list-agendamento-de-ligacao.component.html",
  styleUrls: ["./list-agendamento-de-ligacao.component.css"],
})
export class ListAgendamentoDeLigacaoComponent
  extends BaseComponent
  implements OnInit {
  display = false;
  itemSelecionado: any;
  selectRetorno: any;
  tiporetorno$: any;
  addAnd = false;
  filter = {
    valid: false,
    selectedDevice: null,
    inputDevice: null,
    inputDataInicial: null,
    inputDataFinal: null,
    inputAtendente: null,
  };
  atendentes: Observable<any>;
  angularGrid: AngularGridInstance;
  columnDefinitions: Column[];
  gridOptions: GridOption;
  dataset: any[];
  isCountEnabled = true;
  metrics: Metrics;
  odataQuery = "";
  dataview: any;
  grid: any;
  gridService: any;
  loadingDataGrid = false;
  hasInitialLoading = true;
  editarForm = false;
  filters = false;

  tituto = "Retorno Agendamento";

  @Output() eventoConcluido = new EventEmitter<any>();

  @Output() vinculo = new EventEmitter<any>();
  loggedUser: User;
  retornos: any = [];
  descricaoLigacao: any = null;
  dataLigacao: any = null;

  constructor(
    private service: AgendamentoDeLigacaoServices,
    authenticationService: AuthenticationService,
    private usuarioService: UsuarioService,
    private retornoService: RetonoligacaoService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router
  ) {
    super(authenticationService, fb, route, router);
  }

  getTipoRetorno() {
    this.service.get(`$expand=tiporetornoligacao`).subscribe((value) => {
      this.tiporetorno$ = value.value;
    });
  }

  getTipos(query) {
    this.retornoService.get(``).subscribe((value) => {
      this.tiporetorno$ = value.value;
    });
  }

  ngOnInit() {
    this.loggedUser = this.authenticationService.getLoggedUser();
    if (this.loggedUser.ehAtendente == true) {
      this.filter.inputAtendente = this.loggedUser;
    } else {
      (<HTMLSelectElement>(
        document.getElementById("buttonNovoAgendamento")
      )).classList.add("buttonNovoAgendamento");
    }
    this.getTipos("");
    //#region Definição de colunas
    this.columnDefinitions = [
      {
        id: "numero",
        name: "N° da Solicitação",
        field: "solicitacao",
        sortable: true,
        queryFieldSorter: "solicitacao/numero",
        formatter(row, cell, value, dataContext) {
          return value.numero;
        },
      },
      {
        id: "nome",
        name: "Nome Segurado",
        field: "solicitacao",
        sortable: true,
        queryFieldSorter: "solicitacao/segurado",
        formatter(row, cell, value, dataContext) {
          return value.segurado.nome;
        },
      },

      {
        id: "dataAgendamento",
        name: "Data Agendamento",
        field: "dataAgendamento",
        sortable: true,
        type: FieldType.string,
        filterable: false,
        formatter(row, cell, value, dataContext) {
          return moment(value).format("DD/MM/YYYY");
        },
      },
      {
        id: "motivo",
        name: "Motivo",
        field: "motivo",
        sortable: true,
        type: FieldType.string,
        filterable: false,
      },
      {
        id: "dataLigacao",
        name: "Data Ligação",
        field: "dataLigacao",
        sortable: true,
        type: FieldType.dateTime,
        filterable: false,
        formatter(row, cell, value, dataContext) {
          return value != null ? moment(value).format("DD/MM/YYYY") : "";
        },
      },
      {
        id: "tipoRetornoLigacao",
        name: "Descrição Retorno",
        field: "tipoRetornoLigacao",
        sortable: true,
        queryFieldSorter: "tipoRetornoLigacao",
        formatter(row, cell, value, dataContext) {
          return value ? value.descricao : " ";
        },
        filterable: false,
      },
      {
        id: "atendente",
        name: "Atendente",
        field: "solicitacao",
        sortable: true,
        queryFieldSorter: "solicitacao/atendente",
        formatter(row, cell, value, dataContext) {
          return value.atendente.nome;
        },
      },
      {
        id: "openModal",
        field: "tipoRetornoLigacao",
        toolTip: "",
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        formatter: (row, cell, value, dataContext) =>
          !value
            ? `<a href="javascript:void(0)">
                  <button style="border: none; border-radius: 5px; background-color: #ef8f00; color: white; " >Retorno</button>
              </a>`
            : '   <button disabled style="border: 1px solid white; border-radius: 5px;">Retorno</button>',
        minWidth: 100,
        // #ef8f00 !important
        maxWidth: 100,
        onCellClick: (e: Event, args: OnEventArgs) => {
          if (
            !args.dataContext.tipoRetornoLigacao &&
            this.loggedUser.ehAtendente == true
          ) {
            this.abrir(args);
          } else if (
            !args.dataContext.tipoRetornoLigacao &&
            this.loggedUser.ehAtendente == false
          ) {
            this.alerta();
          }
        },
      },
    ];

    //#endregion

    //#region Definção da Grid
    this.gridOptions = {
      enableAutoResize: true,

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
        process: (query) =>
          this.getData(
            `$expand=solicitacao($expand=atendente,segurado),tiporetornoligacao&${query}`
          ),
        postProcess: (response) => {
          this.metrics = response.metrics;
          this.displaySpinner(false);
          this.getCallback(response);
        },
      },
    };
    //#endregion
  }

  getFormEdit() {
    this.service
      .get(`$filter=id eq ${this.form.value.id}`)
      .subscribe(({ value }) => {
        this.tituto = "Retorno Agendamento";
        this.form = this.fb.group({
          id: this.form.value.id,
          nome: value[0].nome,
          NSolicitacao: value[0].nsolicitacao,
          motivo: value[0].motivo,
          dataagendamento: value[0].dataagendamento,
          dataligacao: value[0].dataligacao,
          tiporetornoligacao_id: value[0].tiporetornoligacao_id,
        });
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

  getData(query) {
    this.odataQuery = query;
    if (this.filters) {
      this.pesquisar();
    } else {
      if (this.loggedUser.ehAtendente == true){
        return this.service.getAll(query+`&$filter=solicitacao/atendente_Id eq ${this.filter.inputAtendente.id}`);
      } else {
        return this.service.getAll(query);
      }
    }
  }

  reloadGrid() {
    document.querySelectorAll(".slick-gridmenu-item").forEach((item) => {
      const element = item as HTMLElement;
      const lastElement = element.lastChild as HTMLElement;

      if (lastElement.innerText === "Refresh Dataset") {
        element.click();
      }
    });
  }

  handleEventoConcluido(retorno) {
    if (retorno) {
      this.itemSelecionado = null;
      this.display = false;
      this.reloadGrid();

      Swal.fire(
        "",
        `Agendamento ${retorno.adicionar ? "adicionado" : "atualizado"}!`,
        "success"
      );
    }
  }

  openPanel() {
    this.router.navigate(["cadastrar"], { relativeTo: this.route });
  }

  pesquisar() {
    let filter = "$filter=";

    this.filter.valid = true;
    if (this.filter.inputDataInicial == "" && this.filter.inputDataFinal == "") {
      this.filter.inputDataInicial = null;
      this.filter.inputDataFinal = null
      return;
    }

    if (this.filter.inputDataInicial == "") {
      this.filter.inputDataInicial = null;
      return;
    }

    if (this.filter.inputDataFinal == "") {
      this.filter.inputDataFinal = null
      return;
    }

    if (
      this.filter.selectedDevice !== null &&
      this.filter.inputDataInicial !== null &&
      this.filter.inputDataFinal !== null
    ) {
      this.filter.valid;
      if (this.filter.selectedDevice === "Todas") {
        if (this.addAnd) {
          this.addAnd = true;
        }
      }
      if (this.filter.selectedDevice === "Fechadas") {
        if (this.addAnd) {
          filter += " and";
        }
        this.addAnd = true;
        filter += `not(tipoRetornoLigacao_Id eq null)`;
      }
      if (this.filter.selectedDevice === "Abertas") {
        if (this.addAnd) {
          filter += " and ";
        }
        this.addAnd = true;
        filter += `tipoRetornoLigacao_Id eq null`;
      }
      if (this.filter.inputAtendente !== null) {
        if (this.addAnd) {
          filter += " and ";
        }
        this.addAnd = true;
        filter += `solicitacao/atendente_Id eq ${this.filter.inputAtendente.id} `;
      }
      if (this.filter.inputDevice !== null && this.filter.inputDevice !== "") {
        if (this.addAnd) {
          filter += " and ";
        }
        this.addAnd = true;
        filter += `solicitacao/numero eq ${this.filter.inputDevice}`;
      }

      // if (this.dataInicialChange !== null && this.dataFinalChange !== null) {
      //   if (this.addAnd) {
      //     filter += " and ";
      //   }
      //   this.addAnd = true;
      //   filter += `dataAgendamento ge ${this.filter.inputDataInicial} && dataAgendamento le ${this.filter.inputDataFinal}`;
      // }

      if ((this.filter.inputDataInicial !== null && this.filter.inputDataFinal !== null) || (this.filter.inputDataInicial == "" || this.filter.inputDataFinal == "")) {
        if (this.filter.inputDataInicial == "" || this.filter.inputDataFinal == "")
          return;
        if (this.addAnd) {
          filter += " and ";
        }
        this.addAnd = true;
        filter += `dataAgendamento ge ${this.filter.inputDataInicial} && dataAgendamento le ${this.filter.inputDataFinal}`;
      }

      if (this.addAnd) {
        this.service
          .getAll(`${filter}&${this.odataQuery}`)
          .subscribe((data) => {
            this.getCallback(data);
          });
        this.filters = true;
        this.addAnd = false;
      }
    }
  }

  alerta() {
    Swal.fire({
      title: "",
      html: `
       <label>Apenas Usuários atendentes podem cadastrar um Retorno de Agendamento!</label>
       `,
    });
  }

  abrir(item) {
    this.dataLigacao = null, this.descricaoLigacao = null;
    this.retornos = [{id: null, descricao: ""}];
    $("#alertRetornoAgendamento").css("display","none")

    this.tiporetorno$.map((retorno) => {
      this.retornos.push({id: retorno.id, descricao: retorno.descricao});
      console.log(this.retornos)
    });

    ($("#modalDeRetorno") as any).modal("show");

    $("#cadastrarRetorno").click(() => {
      const post = {
        id: item.dataContext.id,
        motivo: item.dataContext.motivo,
        dataagendamento: moment(item.dataContext.dataAgendamento).format(
          "DD/MM/yyyy"
        ),
        nsolicitacao: item.dataContext.solicitacao.numero,
        tipoRetornoLigacao: this.descricaoLigacao,
        dataLigacao: moment(this.dataLigacao).format("DD/MM/yyyy"),
      };

      console.log(this.dataLigacao)
      if(this.descricaoLigacao == null || (this.dataLigacao == null || this.dataLigacao == "")){
        $("#alertRetornoAgendamento").css("display","block")
        return;
      }
  
      this.service.put(post).subscribe(
        (response) => {
          this.setResult(response);
          if (response.successfully) {
            this.eventoConcluido.emit({
              adicionar: item.dataContext.id != 0,
            });
          }
        },
        (error) => this.showError(error)
      );

      ($("#modalDeRetorno") as any).modal("hide");

      Swal.fire(
        '',
        `Retorno agendado com Sucesso!`,
        'success'
      );

      this.angularGrid.gridService.resetGrid();
    });

  }

  handleClosePanel(fechar) {
    if (fechar) {
      this.itemSelecionado = null;
      this.display = false;
    }
  }

  remover(item) {
    Swal.fire({
      title: "Atenção!",
      text: `Deseja excluir o Agendamento: ${item.dataContext.id}?`,
      type: "warning",
      showCancelButton: true,
      confirmButtonColor: "#5cd65c",
      cancelButtonColor: "#ff3333",
      confirmButtonText: "Sim",
      cancelButtonText: "Não",
      showLoaderOnConfirm: true,
      preConfirm: () => {
        this.service.deletar(item.dataContext.id).subscribe(() => {
          const metadata =
            this.angularGrid.gridService.getColumnFromEventArguments(item);
          this.angularGrid.gridService.deleteItemById(metadata.dataContext.id);
        });
      },
    }).then((result) => {
      if (result.value) {
        const metadata =
          this.angularGrid.gridService.getColumnFromEventArguments(item);
        this.angularGrid.gridService.deleteItemById(metadata.dataContext.id);
        Swal.fire("", "O Agendamento foi deletado.", "success");
      }
    });
  }

  editar(item) {
    this.router.navigate([`editar/${item.id}`], { relativeTo: this.route });
  }

  numeroChange(event) {
    this.service
      .getAll(
        `$expand=solicitacao,tiporetornoligacao&$filter=solicitacao/numero eq ${event.target.value}`
      )
      .subscribe((response) => (this.dataset = response.value));
  }

  // dataInicialChange(event) {
  //   this.service
  //     .getAll(
  //       `$expand=solicitacao,tiporetornoligacao&$filter=dataAgendamento eg ${event.target.value}`
  //     )
  //     .subscribe((response) => (this.dataset = response.value));
  // }

  // dataFinalChange(event) {
  //   this.service
  //     .getAll(
  //       `$expand=solicitacao,tiporetornoligacao&$filter=dataAgendamento le ${event.target.value}`
  //     )
  //     .subscribe((response) => (this.dataset = response.value));
  // }

  atendenteChange(event) {
    var atendenteInfo = (this.loggedUser =
      this.authenticationService.getLoggedUser());

    if (atendenteInfo.ehAtendente === false) {
      (<HTMLSelectElement>document.getElementById("atendente")).disabled = true;
      event.target.value = atendenteInfo.nome;
    } else {
      this.usuarioService
        .get(`$select=id,nome&$filter=(contains(nome,'${event.query}'))`)
        .subscribe((data) => {
          this.atendentes = data.value;
        });
    }
  }

  setAtendente(atendente) {

    this.filter.inputAtendente.id = atendente.id;
  }

  search(event) {
    this.usuarioService
      .get(`$select=id,nome&$filter=(contains(nome,'${event.query}'))`)
      .subscribe((data) => {
        this.atendentes = data.value;
      });
  }

  onChange(event) {
    if (event.target.value === "Todas") {
      this.service
        .getAll(`$expand=solicitacao,tiporetornoligacao`)
        .subscribe((response) => (this.dataset = response.value));
    }
    if (event.target.value === "Fechadas") {
      this.service
        .getAll(
          `$expand=solicitacao,tiporetornoligacao&$filter=not(tipoRetornoLigacao_Id eq null  )`
        )
        .subscribe((response) => (this.dataset = response.value));
    }
    if (event.target.value === "Abertas") {
      this.service
        .getAll(
          `$expand=solicitacao,tiporetornoligacao&$filter=(tipoRetornoLigacao_Id eq null) `
        )
        .subscribe((response) => (this.dataset = response.value));
    }
  }
}
