import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';

import Swal from 'sweetalert2';

import { BaseComponent, AuthenticationService } from 'src/app/core';
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

import { UsuarioService } from '../service';
import { EmpresaService } from '../../empresas';


@Component({
  selector: 'app-list-usuario',
  templateUrl: './list-usuario.component.html',
  styleUrls: ['./list-usuario.component.css']
})

export class ListUsuarioComponent extends BaseComponent implements OnInit {
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

  @Output() vinculo = new EventEmitter<any>();

  constructor(private service: UsuarioService,
    private empresaService: EmpresaService,
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
  ) {
    super(authenticationService, fb, route, router);
  }

  ngOnInit() {

    //#region Definição de colunas
    this.columnDefinitions = [
      {
        id: 'edit',
        field: '',
        toolTip: 'Editar',
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        formatter: Formatters.editIcon,
        minWidth: 30,
        maxWidth: 30,

        onCellClick: (e: Event, args: OnEventArgs) => {
          this.editar(args.dataContext);
        }
      },
      {
        id: 'delete',
        field: '',
        toolTip: 'Excluir',
        excludeFromExport: true,
        excludeFromHeaderMenu: true,
        excludeFromGridMenu: true,
        excludeFromColumnPicker: true,
        formatter: Formatters.deleteIcon,
        minWidth: 30,
        maxWidth: 30,

        onCellClick: (e: Event, args: OnEventArgs) => {
          this.remover(args);
        }
      },
      {
        id: 'matricula', name: 'Matrícula', field: 'matricula', sortable: true, type: FieldType.string,
        filterable: true,
      },
      {
        id: 'login', name: 'Login', field: 'login', sortable: true, type: FieldType.string,
        filterable: true,
      },
      {
        id: 'nome', name: 'Nome', field: 'nome', sortable: true, type: FieldType.string,
        filterable: true,
      },
      {
        id: 'email', name: 'Email', field: 'email', sortable: true, type: FieldType.string,
        filterable: true,
      },
      {
        id: 'empresa',
        name: 'Empresa',
        field: 'empresa',
        sortable: true,
        queryFieldSorter: 'empresa/nome',
        formatter(row, cell, value, dataContext) {
          return value.nome;
        },
        filterable: true,
        queryFieldFilter: 'empresa/nome',
        filterSearchType: FieldType.number,
        filter: {
          collectionAsync: this.empresaService.fetch(`$select=nome&$orderBy=nome`, false),
          model: Filters.multipleSelect,
          customStructure: {
            label: 'Nome',
            value: 'Nome'
          },
        }
      },
      {
        id: 'ativo', name: 'Ativo', field: 'ativo', sortable: true, type: FieldType.boolean,
        formatter: Formatters.checkmark,
        filterable: true,
        filter: {
          model: Filters.singleSelect,
          collection: [{ value: '', label: '' }, { value: true, label: 'Sim' }, { value: false, label: 'Não' }]
        }
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
    if (this.odataQuery.includes("&$filter")) {
      return this.service.getAll(`$expand=empresa,agencia,areasDeNegocio,grupos,gruposAgencias&${query} and Excluido eq false`)
    } else {
      return this.service.getAll(`$expand=empresa,agencia,areasDeNegocio,grupos,gruposAgencias&${query}`);
    }
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
        `Usuário ${retorno.adicionar ? 'adicionado' : 'atualizado'}!`,
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
      text: `Deseja excluir o usuário: ${item.dataContext.nome}?`,
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#5cd65c',
      cancelButtonColor: '#ff3333',
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      showLoaderOnConfirm: true,
      preConfirm: () => {
        this.service.deletar(item.dataContext.id).subscribe(()=>{
          const metadata = this.angularGrid.gridService.getColumnFromEventArguments(item);
          this.angularGrid.gridService.deleteItemById(metadata.dataContext.id
        )})
      }
    }).then((result) => {
      if (result.value) {
        const metadata = this.angularGrid.gridService.getColumnFromEventArguments(item);
        this.angularGrid.gridService.deleteItemById(metadata.dataContext.id)
        Swal.fire(
          '',
          'O Usuário foi deletado.',
          'success'
        );
      }
    });
  }

  editar(item) {
    this.router.navigate([`editar/${item.id}`,
  ], { relativeTo: this.route});
  }
}

