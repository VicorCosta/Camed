import { Component, OnInit, Output, EventEmitter } from '@angular/core';

import { BaseComponent, AuthenticationService, ParametrosSistemaService } from 'src/app/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';

import Swal from 'sweetalert2';
import {
  AngularGridInstance,
  Column,
  GridOption,
  FieldType,
  // //GridOdataService,
  Metrics,
  Formatters,
  OnEventArgs
} from 'angular-slickgrid';

import { GridOdataService } from '@slickgrid-universal/odata';
import { VariaveisDeEmailService } from '../service';
import { localePtBR } from 'src/locale/slickgrid.pt';
import { subscribe } from 'graphql';

@Component({
  templateUrl: 'list-variaveis-email.component.html'
})

export class ListVariaveisComponent extends BaseComponent implements OnInit {
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

  parametros: any = [];

  variaveisDeEmailData: any;

  @Output() acao = new EventEmitter<any>();

  constructor(private acaoservice: VariaveisDeEmailService,
    private parametroService: ParametrosSistemaService,
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
  ) {
    super(authenticationService, fb, route, router);
  }


  ngOnInit() {
    this.parametroService.getAll().subscribe(content => {
      content.value.map(resultado => { this.parametros.push( { id: resultado.id, parametro: resultado.parametro, variavelDeEmail_Id: resultado.variaveisDeEmail_Id } ); })
    });

    this.columnDefinitions = [
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
          this.enviarParaParametros();
        }
      
      },
      {
        id: 'parametro', name: 'Parâmetro', field: 'parametro', sortable: true, type: FieldType.string,
        filterable: true, 
        
        onCellChange: (e: Event, args: OnEventArgs) => {
          console.log(args);
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

    let odataValue = this.odataQuery.substring(this.odataQuery.indexOf("'") + 1);
    odataValue = odataValue.substring(0, odataValue.indexOf("'"))

    if(this.parametros.filter(elemento => elemento.parametro === odataValue).length > 0 && odataValue != ""){
      let idDoParametroDoFiltro = this.parametros.filter(elemento => elemento.parametro === odataValue)[0].id
      this.acaoservice.getAll(`$filter=parametro_Id eq ${idDoParametroDoFiltro}`).subscribe(content => {
        this.dataset = content.value;
      });
    } else if(odataValue != ""){
      this.dataset = null;
    } else {
      this.dataset = response.value;
    }

    this.gridOptions = Object.assign({}, this.gridOptions);
    this.hasInitialLoading = false;
  }

  private displaySpinner(isProcessing): void {
    this.loadingDataGrid = isProcessing;
  }

  getData(query) {
    this.odataQuery = query;
    let odataValue = this.odataQuery.substring(this.odataQuery.indexOf("'") + 1);
    odataValue = odataValue.substring(0, odataValue.indexOf("'"));
    
    console.log(this.parametros.filter(elemento => elemento.parametro === odataValue), this.parametros.filter(elemento => elemento.parametro === odataValue).length)
    if (this.parametros.filter(elemento => elemento.parametro === odataValue).length > 0 && odataValue != ""){
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
          id: 'parametro', name: 'Parâmetro', field: 'nome', sortable: true, type: FieldType.string,
          filterable: true, 
          
          onCellChange: (e: Event, args: OnEventArgs) => {
            console.log(args);
          }
        },
  
      ];
    } else {
      this.columnDefinitions = [
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
            this.enviarParaParametros();
          }
        
        },
        {
          id: 'parametro', name: 'Parâmetro', field: 'parametro', sortable: true, type: FieldType.string,
          filterable: true, 
          
          onCellChange: (e: Event, args: OnEventArgs) => {
            console.log(args);
          }
        },
  
      ];
      query = query.replace("Nome", "Parametro")
    }
    return this.parametroService.getAll(query);
  }

  reloadGrid() {
    this.angularGrid.gridService.resetGrid();
  }

  handleEventoConcluido(retorno) {
    if (retorno) {
      this.acaoSelecionada = null;
      this.display = false;
      this.reloadGrid();

      Swal.fire(
        '',
        `Ação ${retorno.adicionar ? 'adicionada' : 'atualizada'}!`,
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
      this.acaoSelecionada = null;
      this.display = false;
    }
  }

  remover(item) {
    Swal.fire({
      title: 'Atenção!',
      text: `Deseja excluir a variável de e-mail: ${item.dataContext.nome}?`,
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#5cd65c',
      cancelButtonColor: '#ff3333',
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      showLoaderOnConfirm: true,
      preConfirm: () => {
        this.acaoservice.deletar(item.dataContext.id).subscribe(() => {
          const metadata = this.angularGrid.gridService.getColumnFromEventArguments(item);
          this.angularGrid.gridService.deleteItemById(metadata.dataContext.id);
        })
      }
    }).then((result) => {
      if (result.value) {
        Swal.fire(
          '',
          'Variável de E-mail foi deletada.',
          'success'
        );
      }
    });
  }

  editar(item) {
    this.router.navigate([`editar/${item.id}`,
    ], { relativeTo: this.route });
  }
  
  enviarParaParametros() {
    this.router.navigate([`/pages/parametros-do-sistema`,
    ], { relativeTo: this.route });
  }
}


