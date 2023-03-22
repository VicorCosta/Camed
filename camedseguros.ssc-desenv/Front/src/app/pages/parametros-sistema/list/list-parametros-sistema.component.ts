import { Menu } from './../../../core/models/menu.model';
import { PlaceholderGridModule } from 'src/app/shared';
import { PlaceholderGridComponent } from './../../../shared/components/placeholder-grid/placeholder-grid.component';
import { gridOptions } from 'src/locale/slickgrid.pt';
import { ListTipodeParametrosComponent } from './../../tipos-de-parametros/list/list-tipos-de-parametros.component';
import { VariaveisDeEmailService } from './../../variaveisdeemail/service/variaveisdeemail.service';
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

import { MapeamentoAcaoSituacaoService, ParametrosSistemaService } from '../service';
import { TipodeParametroService } from '../../tipos-de-parametros';
import { VariaveisDeEmail } from 'src/app/core/models/variaveisdeemail.model';

@Component({
  selector: 'app-list-parametros-sistema',
  templateUrl: './list-parametros-sistema.component.html',
  styleUrls: ['./list-parametros-sistema.component.css']
})

export class ListParametrosSistemaComponent extends BaseComponent implements OnInit {
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

  podeSerDeletado: boolean = true;

  @Output() vinculo = new EventEmitter<any>();

  constructor(private service: ParametrosSistemaService,
    private acaoSituacaoService: MapeamentoAcaoSituacaoService,
    private tipodeParametroService: TipodeParametroService,
    private variaveisDeEmailService: VariaveisDeEmailService,
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
        id: 'parametro', name: 'Parâmetro', field: 'parametro', sortable: true, type: FieldType.string,
        filterable: true,
      },
      {
        id: 'tipo', name: 'Tipo', field: 'tipo', sortable: true, type: FieldType.string,
        filterable: true,
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

    const fields = response.value.map(e => ({
      ...e,
      fields: e.tipo === "EMAIL" ? e.variaveisDeEmail : e.tipoDeParametro
    }))

    this.gridOptions = Object.assign({}, this.gridOptions);
    this.dataset = fields;
    this.hasInitialLoading = false;
  }

  private displaySpinner(isProcessing) {
    this.loadingDataGrid = isProcessing;
  }

  getData(query) {
    this.odataQuery = query;
    return this.service.getAll(query);
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
        `Parâmetro ${retorno.adicionar ? 'adicionado' : 'atualizado'}!`,
        'success'
      );
    }
  }

  openPanel() {
    this.router.navigate(['cadastrar',
    ], { relativeTo: this.route });
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
      text: `Deseja excluir o parâmetro: ${item.dataContext.parametro}?`,
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#5cd65c',
      cancelButtonColor: '#ff3333',
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      showLoaderOnConfirm: true,
      preConfirm: () => {
        this.acaoSituacaoService.get(``).subscribe((data) => {
          for (let i = 0; i < data.value.length; i++) {
            let parametrosId = data.value[i].parametrosSistema_Id;
            if (parametrosId == item.dataContext.id) {
              this.podeSerDeletado = false;
              console.log("Bateu!!", parametrosId, item.dataContext.id, this.podeSerDeletado)
              break;
            }
          }

          if (this.podeSerDeletado) {
            this.service.deletar(item.dataContext.id).subscribe(() => {
              const metadata = this.angularGrid.gridService.getColumnFromEventArguments(item);
              this.angularGrid.gridService.deleteItemById(metadata.dataContext.id);
              Swal.fire("", "O Parâmetro foi deletado.", "success");
            },);
          } else {
            Swal.fire("", "O Parâmetro não pode ser deletado.", "warning");
            return;
          }

        });
      }
    })
  }

  editar(item) {
    this.router.navigate([`editar/${item.id}`,
    ], { relativeTo: this.route });
  }
}

