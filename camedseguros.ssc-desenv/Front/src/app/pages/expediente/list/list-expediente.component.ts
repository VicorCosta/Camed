import { Component, OnInit, Output, EventEmitter } from '@angular/core';

import { BaseComponent, AuthenticationService } from 'src/app/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import * as moment from 'moment';
import Swal from 'sweetalert2';
import {
  AngularGridInstance,
  Column,
  GridOption,
  FieldType,
  Filters,
  //GridOdataService,
  Metrics,
  Formatters,
  OnEventArgs
} from 'angular-slickgrid';
import { GridOdataService } from '@slickgrid-universal/odata';

import { ExpedienteService } from '../service';
import { localePtBR } from 'src/locale/slickgrid.pt';


@Component({
  selector: 'app-list-expediente',
  templateUrl: './list-expediente.component.html',
  styleUrls: ['./list-expediente.component.css']
})

export class ListExpedienteComponent extends BaseComponent implements OnInit {
  display = false;
  itemSelecionado: any;

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


  @Output() item = new EventEmitter<any>();

  constructor(private expedienteService: ExpedienteService,
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
        id: 'dia', name: 'Dia', field: 'dia', sortable: true, type: FieldType.string,
        formatter: function (row, cell, value, dataContext) {
          switch (value) {
            case 1: {
              return 'Domingo';
            }
            case 2: {
              return 'Segunda-Feira';
            }
            case 3: {
              return 'Terça-Feira';
            }
            case 4: {
              return 'Quarta-Feira';
            }
            case 5: {
              return 'Quinta-Feira';
            }
            case 6: {
              return 'Sexta-Feira';
            }
            case 7: {
              return 'Sábado';
            }
          }
        },
      },
      {
        id: 'hInicialM', name: 'Hora inicial manhã', field: 'horaInicialManha', sortable: true, type: FieldType.string,
        formatter: (row, cell, value, dataContext) => { return moment(value).format("HH:mm"); }
      },
      {
        id: 'hFinalM', name: 'Hora final manhã', field: 'horaFinalManha', sortable: true, type: FieldType.string,
        formatter: (row, cell, value, dataContext) => { return moment(value).format("HH:mm"); }
      },
      {
        id: 'hInicialT', name: 'Hora inicial tarde', field: 'horaInicialTarde', sortable: true, type: FieldType.string,
        formatter: (row, cell, value, dataContext) => { return moment(value).format("HH:mm"); }
      },
      {
        id: 'hFinalT', name: 'Hora final tarde', field: 'horaFinalTarde', sortable: true, type: FieldType.string,
        formatter: (row, cell, value, dataContext) => { return moment(value).format("HH:mm"); }
      }
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

  private displaySpinner(isProcessing) {
    this.loadingDataGrid = isProcessing;
  }

  getData(query) {
    this.odataQuery = query;
    return this.expedienteService.getAll(query);
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
        `Expediente ${retorno.adicionar ? 'adicionado' : 'atualizado'}!`,
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
      text: `Deseja excluir o expediente ?`,
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#5cd65c',
      cancelButtonColor: '#ff3333',
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      showLoaderOnConfirm: true,
      preConfirm: () => {
        this.expedienteService.deletar(item.dataContext.id)
        .subscribe(response => {
          if (response.successfully) {
            const metadata = this.angularGrid.gridService.getColumnFromEventArguments(item);
            this.angularGrid.gridService.deleteItemById(metadata.dataContext.id);
            }
          });
      }
    }).then((result) => {
      if (result.value) {
        Swal.fire(
          '',
          'O expediente foi deletado.',
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

