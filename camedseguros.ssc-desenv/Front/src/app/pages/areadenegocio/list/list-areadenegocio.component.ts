import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BaseComponent, AuthenticationService } from 'src/app/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import Swal from 'sweetalert2';
import {
  AngularGridInstance,
  Column,
  GridOption,
  FieldType,
  Filters,
  // //GridOdataService,
  Metrics,
  Formatters,
  OnEventArgs
} from 'angular-slickgrid';

import { AreaDeNegocioService } from '../service';
import { localePtBR } from 'src/locale/slickgrid.pt';
import { GridOdataService } from '@slickgrid-universal/odata';


@Component({
  selector: 'app-list-areadenegocio',
  templateUrl: './list-areadenegocio.component.html',
  styleUrls: ['./list-areadenegocio.component.css']
})

export class ListAreaDeNegocioComponent extends BaseComponent implements OnInit {
  display = false;
  areaSelecionada: any;

  angularGrid: AngularGridInstance;
  columnDefinitions: Column[];
  gridOptions: GridOption;
  dataset: any[] = [];
  isCountEnabled = true;
  metrics: Metrics;
  odataQuery = '';
  dataview: any;
  grid: any;
  gridService: any;
  loadingDataGrid = false;
  hasInitialLoading = true;
  atualizarGrid = localePtBR.TEXT_REFRESH_DATASET;

  @Output() area = new EventEmitter<any>();

  constructor(private areadenegocioService: AreaDeNegocioService,
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
  ) {
    super(authenticationService, fb, route, router);
  }

  ngOnInit() {

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
        id: 'nome', name: 'Nome', field: 'nome', sortable: true, type: FieldType.string,
        filterable: true,
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

  private displaySpinner(isProcessing): void {
    this.loadingDataGrid = isProcessing;
  }

  getData(query) {
    this.odataQuery = query;
    return this.areadenegocioService.getAll(query);
  }

  reloadGrid() {
this.angularGrid.gridService.resetGrid();
  }

  handleEventoConcluido(retorno) {
    if (retorno) {
      this.areaSelecionada = null;
      this.display = false;
      this.reloadGrid();

      Swal.fire(
        '',
        `Área de negócio ${retorno.adicionar ? 'adicionada' : 'atualizada'}!`,
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
      this.areaSelecionada = null;
      this.display = false;
    }
  }

  remover(item) {
    Swal.fire({
      title: 'Atenção!',
      text: `Deseja excluir a área: ${item.dataContext.nome}?`,
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#5cd65c',
      cancelButtonColor: '#ff3333',
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      showLoaderOnConfirm: true,
      preConfirm: () => {
        this.areadenegocioService.deletar(item.dataContext.id).subscribe(()=>{
          const metadata = this.angularGrid.gridService.getColumnFromEventArguments(item);
          this.angularGrid.gridService.deleteItemById(metadata.dataContext.id);
        })
      }
    }).then((result) => {
      if (result.value) {
        Swal.fire(
          '',
          'Área de negócio foi deletada.',
          'success'
        );
      }
    });
  }

  editar(item) {
    this.router.navigate([`editar/${item.id}`,
      ], { relativeTo: this.route,  state: item});
  }

}


