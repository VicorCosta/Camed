import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';

import Swal from 'sweetalert2';

import { BaseComponent, AuthenticationService, User } from 'src/app/core';
import {
  AngularGridInstance,
  Column,
  GridOption,
  FieldType,
  Filters,
  // GridOdataService,

  Metrics,
  Formatters,
  OnEventArgs
} from 'angular-slickgrid';
import { GridOdataService } from '@slickgrid-universal/odata';

import { UsuarioService } from '../../usuario';
import { EmpresaService } from '../../empresas';
import { TipoSeguroService } from '../../tiposdeseguro';
import { GrupoAgenciaService } from '../../grupoagencias';
import { AgenciaService } from 'src/app/core';
import { AreaDeNegocioService } from '../../areadenegocio';
import { AusenciaDeAtendenteServices } from '../service';
import { UsuarioGrupo } from '../../auditoria';
import { query } from '@angular/animations';


@Component({
  selector: 'app-list-ausencia-de-atendente',
  templateUrl: './list-ausencia-de-atendente.component.html',
  styleUrls: ['./list-ausencia-de-atendente.component.css']
})

export class ListAusenciaDeAtendenteComponent extends BaseComponent implements OnInit {
  display = false;
  itemSelecionado: any;

  angularGrid: AngularGridInstance;
  columnDefinitions: Column[];
  gridOptions: GridOption;
  dataset: any[];
  isCountEnabled = true;
  metrics: Metrics;
  odataQuery = '';
  dataview: any;
  grid: any;
  gridService: any;
  loadingDataGrid = false;
  hasInitialLoading = true;

  loggedUser: User;
  isGerente: boolean = false;

  filtro: any = {
    atendente: null,
    dataInicial: null,
    dataFinal: null
  }

  @Output() vinculo = new EventEmitter<any>();

  constructor(private service: AusenciaDeAtendenteServices,
    private usuarioServices: UsuarioService,
    private empresaService: EmpresaService,
    private agenciaService: AgenciaService,
    private grupoAgenciaService: GrupoAgenciaService,
    private tiposeguroService: TipoSeguroService,
    private areaDeNegocioService: AreaDeNegocioService,
    private userGrupoService: UsuarioGrupo,
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
  ) {
    super(authenticationService, fb, route, router);
  }

  ngOnInit() {
    this.userGrupoService.getAll().subscribe((data) => {
      this.loggedUser = this.authenticationService.getLoggedUser();
      for (let i = 0; i < data.value.length; i++) {
        let usuarioId = data.value[i].usuario_Id
        let grupoId = data.value[i].grupo_Id
        if (usuarioId == this.loggedUser.id) {
          if (grupoId == 2) {
            this.isGerente = true;
          }

          break;
        }
      }

      if(this.isGerente){
        document.getElementById("addAtendente").style.visibility = "visible"
      }

    });

    //#region Definição de colunas
    this.columnDefinitions = [
      {
        id: "edit",
        field: "",
        toolTip: "Editar",
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        formatter: Formatters.editIcon,
        minWidth: 30,
        maxWidth: 30,

        onCellClick: (e: Event, args: OnEventArgs) => {
          this.editar(args.dataContext);
        },
      },
      /*{
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
          this.remover(args);
        },
      },*/
      {
        id: "atendente",
        name: "",
        field: "atendente",
        sortable: true,
        queryFieldSorter: "atendente/nome",
        formatter(row, cell, value, dataContext) {
          return value.nome;
        },
        queryFieldFilter: "atendente/nome",
        filterSearchType: FieldType.string,
        filter: {
          collectionAsync: this.usuarioServices.fetch(
            `$select=nome&$orderBy=nome`,
            false
            ),
            model: Filters.autoComplete,
            customStructure: {
              label: "Nome",
              value: "Nome",
            },
          },
        },
        {
        id: "dataInicialAusencia",
        name: "",
        field: "dataInicioAusencia",
        sortable: true,
        type: FieldType.dateTime,
        outputType: FieldType.dateIso,
        formatter: Formatters.dateIso,
        exportWithFormatter: true,
        // filter: {
        //   model: Filters.compoundDate,
        //   filterOptions: { dateFormat: "Y-m-d" },
        // },
      },
      {
        id: "dataFinalAusencia",
        name: "",
        field: "dataFinalAusencia",
        sortable: true,
        type: FieldType.dateTime,
        outputType: FieldType.dateIso,
        formatter: Formatters.dateIso,
        exportWithFormatter: true,
        // filter: {
        //   model: Filters.compoundDate,
        //   filterOptions: { dateFormat: "Y-m-d" },
        // },
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
        message: 'Nenhum dado encontrado'
      },
      enableFiltering: true,
      enablePagination: true,
      enableColumnPicker: true,
      pagination: {
        pageSizes: [5, 10, 15, 20, 25, 30, 40, 50, 75, 100],
        pageSize: 5,
        totalItems: 0
      },
      backendServiceApi: {
        service: new GridOdataService(),
        options: {
          enableCount: this.isCountEnabled,
          version: 4
        },
        preProcess: () => this.displaySpinner(true),
        process: (query) => this.getData(query),
        postProcess: (response) => {
          this.metrics = response.metrics;
          this.displaySpinner(false);
          this.getCallback(response);
        }
      }
    };
    //#endregion

  }

  angularGridReady(angularGrid: AngularGridInstance) {
    this.angularGrid = angularGrid;
    this.dataview = angularGrid.dataView;
    this.grid = angularGrid.slickGrid;
    this.gridService = angularGrid.gridService;
  }

  getCallback(response) {
    this.gridOptions.pagination.totalItems = response['@odata.count'];

    if (this.metrics) {
      this.metrics.totalItemCount = response['@odata.count'];
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
    //Atendente
    if(this.filtro.atendente && !this.filtro.dataInicial && !this.filtro.dataFinal){
      query += `&$filter=(contains(atendente/nome,'${this.filtro.atendente}'))`;
      return this.service.getAll(`$expand=atendente&${query}`)
    }
    if(this.filtro.atendente && this.filtro.dataInicial && !this.filtro.dataFinal){
      query += `&$filter=(contains(atendente/nome,'${this.filtro.atendente}'))and(dataInicioAusencia ge ${this.filtro.dataInicial} and dataInicioAusencia le 2080-10-10)`;
      return this.service.getAll(`$expand=atendente&${query}`)
    }
    if(this.filtro.atendente && !this.filtro.dataInicial && this.filtro.dataFinal){
      query += `&$filter=(contains(atendente/nome,'${this.filtro.atendente}'))and(dataFinalAusencia ge ${this.filtro.dataFinal} and dataFinalAusencia le 2080-10-10)`;
      return this.service.getAll(`$expand=atendente&${query}`)
    }
    if(this.filtro.atendente && this.filtro.dataInicial && this.filtro.dataFinal){
      query += `&$filter=(contains(atendente/nome,'${this.filtro.atendente}'))and(dataInicioAusencia ge ${this.filtro.dataInicial} and dataInicioAusencia le ${this.filtro.dataFinal})`;
      return this.service.getAll(`$expand=atendente&${query}`)
    }

    //Data Inicial
    if(!this.filtro.atendente && this.filtro.dataInicial && !this.filtro.dataFinal){
      query += `&$filter=(dataInicioAusencia ge ${this.filtro.dataInicial} and dataInicioAusencia le 2080-10-10)`;
      return this.service.getAll(`$expand=atendente&${query}`)
    }

    //Data Final
    if(!this.filtro.atendente && !this.filtro.dataInicial && this.filtro.dataFinal){
      query += `&$filter=(dataFinalAusencia ge ${this.filtro.dataFinal} and dataFinalAusencia le 2080-10-10)`;
      return this.service.getAll(`$expand=atendente&${query}`)
    }

    //Ambas datas
    if(!this.filtro.atendente && this.filtro.dataInicial && this.filtro.dataFinal){
      query += `&$filter=(dataInicioAusencia ge ${this.filtro.dataInicial} and dataInicioAusencia le ${this.filtro.dataFinal})`;
      return this.service.getAll(`$expand=atendente&${query}`)
    }

    return this.service.getAll(`$expand=atendente&${query}`)
  }

  reloadGrid() {
    this.angularGrid.gridService.resetGrid();
  }

  handleEventoConcluido(retorno) {
    if (retorno) {
      this.itemSelecionado = null;
      this.display = false;
      this.reloadGrid();

      Swal.fire(
        '',
        `Mapeamento ${retorno.adicionar ? 'adicionado' : 'atualizado'}!`,
        'success'
      );
    }
  }

  openPanel() {
    this.router.navigate(['cadastrar',
  ], { relativeTo: this.route});
  }

  handleClosePanel(fechar) {
    if (fechar) {
      this.itemSelecionado = null;
      this.display = false;
    }
  }


  remover(item) {
    Swal.fire({
      title: 'Atenção!',
      text: `Deseja excluir a ausência de atendente: ${item.dataContext.atendente.nome}?`,
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#5cd65c',
      cancelButtonColor: '#ff3333',
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
    }).then((result) => {
     this.service.deletar(item.dataContext.id).subscribe(response => {
       this.reloadGrid();
     });
    });
  }

  editar(item) {
    this.router.navigate([`editar/${item.id}`,
  ], { relativeTo: this.route});
  }
}

