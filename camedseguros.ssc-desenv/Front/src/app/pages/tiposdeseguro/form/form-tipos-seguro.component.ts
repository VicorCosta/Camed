
import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, Empresas } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { TipoSeguroService } from '../service';
import _ from 'underscore';

import { GrupoAgenciaService } from '../../grupoagencias';
import { RamosService } from '../../ramosdeseguro';
import { MessageService } from 'primeng/api';


@Component({
  selector: 'app-form-tipos-seguro',
  templateUrl: './form-tipos-seguro.component.html',
  styleUrls: ['./form-tipos-seguro.component.css']
})

export class FormTiposSeguroComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: Empresas;
  titulo: string;
  gruposagencia$: any;
  ramosSeguro$: any;
  itemAgencia: any;
  itensRamo: any;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() id: any;


  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private tiposeguroService: TipoSeguroService,
    private grupoagenciaService: GrupoAgenciaService,
    private ramosService: RamosService,
    private messageService: MessageService
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  ngOnInit() {
    this.submitted = false;
    this.loading = false;
    this.itensRamo = [];

    if (this.id) {
      this.titulo = "Editar seguro"
      this.tiposeguroService.getAgenciaRamoByTipoSeguro(this.id).subscribe(response => {
        this.filterAgenciaID(response);
        this.filterRamosID(response);
      })
    }
    this.setForm();
  }

  setFormEditar() {
    this.getGrupos();
    this.getRamos();
    this.tiposeguroService.get(`$filter=id eq ${this.id}`).subscribe(({ value }) => {
      this.form = this.fb.group({
        id: value[0].id,
        nome: value[0].nome,
        GrupoAgenciaID: this.itemAgencia,
        ramosDeseguro: [this.itensRamo]
      });
    })
  }

  setForm() {
    this.getGrupos();
    this.getRamos();
    this.titulo = 'Novo seguro';
    this.form = this.fb.group({
      id: [0],
      nome: [''],
      GrupoAgenciaID: [0],
      ramosDeseguro: [null]
    });

    if (this.id) {
      this.setFormEditar()
    }
  }


  //Data
  getGrupos() {
    this.gruposagencia$ = this.grupoagenciaService.getAll();
  }

  getRamos() {
    this.ramosSeguro$ = this.ramosService.getAll();
  }

  //Filter
  filterAgenciaID(response) {
    this.itemAgencia = response.value[0].grupoAgencia.id;
    this.setForm();
  }

  filterRamosID(response) {
    this.itensRamo = _.pluck(response.value[0].tiposDeProduto, 'tipoDeProduto_Id');
    this.setForm();
  }

  //Send Data
  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const newValue = this.form.controls.id.value === 0;

    this.tiposeguroService.save(this.form.value).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        if (this.id) {
          this.setFormEditar()
          this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Seguro Editado' });
        } else {
          this.setForm()
          this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Seguro Cadastrado' });
        }
        this.eventoConcluido.emit({ adicionar: newValue });
      }
    }, (error) => this.showError(error));
  }

  //Checkbox
  saveCheckBox(event) {
    if (event.target.checked) {
      this.itensRamo.push(parseInt(event.target.value));
    } else {
      this.itensRamo = _.without(this.itensRamo, parseInt(event.target.value));
    }
  }

  checkRamos(segurosID) {
    return _.contains(this.itensRamo, parseInt(segurosID));
  }

  //Layout
  onClosePanel() {
    this.setResult({} as Result);
    // this.closePanel.emit(true);
    window.history.back();
  }

}


