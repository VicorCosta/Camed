import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { GridOdataService } from '@slickgrid-universal/odata';
import { AngularGridInstance, Column, FieldType, Formatters, GridOption, Metrics, OnEventArgs } from 'angular-slickgrid';
import { BaseComponent, AuthenticationService } from 'src/app/core';
import { localePtBR } from 'src/locale/slickgrid.pt';
import Swal from 'sweetalert2';
import { TipomorteService } from '../service/tipomorte.service';


@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent extends BaseComponent implements OnInit {

  display = false;
  acoes$: any;
  acaoSelecionada: any;

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

  constructor(
    private tipoMorteService: TipomorteService,
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
  ) {
    super(authenticationService,fb, route, router);

   }

  ngOnInit(): void {
    
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
        id: 'descricao', name: 'Descrição', field: 'descricao', sortable: true, type: FieldType.string,
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
        process:  (query) => this.getData(query),
        postProcess: (response) => {
          this.metrics = response.metrics;
          this.displaySpinner(false);
          this.getCallback(response);
        }
      }
    };
  }

  private displaySpinner(isProcessing): void {
    this.loadingDataGrid = isProcessing;
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

  getData(query) {
    console.log('query', query)
    this.odataQuery = query;
    return this.tipoMorteService.getAll(query);
  }

  angularGridReady(angularGrid: AngularGridInstance) {
    this.angularGrid = angularGrid;
    this.dataview = angularGrid.dataView;
    this.grid = angularGrid.slickGrid;
    this.gridService = angularGrid.gridService;
  }

  editar(item) {
    this.router.navigate([`editar/${item.id}`,
      ], { relativeTo: this.route,  state: item.id});
  }

  openRegister(){
    this.router.navigate(['cadastrar',
  ], { relativeTo: this.route});
  }

  remover(item) {
    Swal.fire({
      title: 'Atenção!',
      text: `Deseja excluir tipo de morte: ${item.dataContext.descricao}?`,
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#5cd65c',
      cancelButtonColor: '#ff3333',
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      showLoaderOnConfirm: true,
      preConfirm: () => {
        this.tipoMorteService.delete(item.dataContext.id).subscribe(()=>{
          const metadata = this.angularGrid.gridService.getColumnFromEventArguments(item);
          this.angularGrid.gridService.deleteItemById(metadata.dataContext.id);
        })
      }
    }).then((result) => {
      if (result.value) {
        Swal.fire(
          '',
          'Tipo de morte foi deletada.',
          'success'
        );
      }
    });
  }

}
