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

import { CampanhaService } from '../service';
import { localePtBR } from 'src/locale/slickgrid.pt';
import { GridOdataService } from '@slickgrid-universal/odata';


@Component({
  selector: 'app-list-campanha',
  templateUrl: './list-campanha.component.html',
  styleUrls: ['./list-campanha.component.css']
})

export class ListCampanhaComponent extends BaseComponent implements OnInit {
  display = false;
  canalSelecionado: any;

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


  @Output() canal = new EventEmitter<any>();

  constructor(private distribuicaoService: CampanhaService,
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
      {
        id: 'ativo', name: 'Ativo', field: 'ativo', sortable: true, type: FieldType.boolean,
        formatter: Formatters.checkmark,
        filterable: true,
        filter: {
          model: Filters.singleSelect,
          collection: [{ value: '', label: 'Todos' }, { value: true, label: 'Sim' }, { value: false, label: 'Não' }]
        }
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
    return this.distribuicaoService.getAll(query);
  }

  reloadGrid() {
this.angularGrid.gridService.resetGrid();
  }

  handleEventoConcluido(retorno) {
    if (retorno) {
      this.canalSelecionado = null;
      this.display = false;
      this.reloadGrid();

      Swal.fire(
        '',
        `Campanha ${retorno.adicionar ? 'adicionada' : 'atualizada'}!`,
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
      this.canalSelecionado = null;
      this.display = false;
    }
  }

  remover(item) {
    Swal.fire({
      title: 'Atenção!',
      text: `Deseja excluir a campanha: ${item.dataContext.nome}?`,
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#5cd65c',
      cancelButtonColor: '#ff3333',
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      showLoaderOnConfirm: true,
      preConfirm: () => {
        this.distribuicaoService.deletar(item.dataContext.id)
          .subscribe(response => {
            const metadata = this.angularGrid.gridService.getColumnFromEventArguments(item);
            this.angularGrid.gridService.deleteItemById(metadata.dataContext.id);
          });
      }
    }).then((result) => {
      if (result.value) {
        Swal.fire(
          '',
          'Campanha foi deletada.',
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
