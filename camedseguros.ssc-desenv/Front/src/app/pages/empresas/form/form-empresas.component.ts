//#region Imports
import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, Empresas } from 'src/app/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { EmpresaService } from '../service';
import _, { forEach } from 'underscore';

import { MessageService } from 'primeng/api';
import { FormGrupoAgenciasComponent } from '../../grupoagencias';
import { TipoSeguroService } from '../../tiposdeseguro';
//#endregion

@Component({
  selector: 'app-form-empresas',
  templateUrl: './form-empresas.component.html',
  styleUrls: ['./form-empresas.component.css']
})

export class FormEmpresasComponent extends BaseComponent implements OnInit {

  //#region Variaveis
  submitted = false;
  display = false;
  post: Empresas;
  titulo: string;
  tiposDeSeguroArray: any[];
  itensSelecionados: any = [];
  itensSelecionados2: any = [];

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() empresa: any;
  @Input() id: any;
  //#endregion

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private empresaService: EmpresaService,
    private tipoSeguroService: TipoSeguroService,
    private messageService: MessageService,
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  ngOnInit() {
    this.submitted = false;
    this.loading = false;
    this.itensSelecionados = [];
    this.itensSelecionados2 = [];
    
    if (this.id) {
      this.createFormEditar();
    } else {
      this.createForm();
      
    }
  }

  createFormEditar() {
    this.titulo = 'Editar Empresa';
    this.empresaService.get(`$expand=TiposDeSeguro&$filter=id eq ${this.id}`).subscribe((empresa) => {

      this.tipoSeguroService.getAll().subscribe(tiposDeSeguroResponse => {
        this.tiposDeSeguroArray = tiposDeSeguroResponse.value;
        
        this.form = this.fb.group({
          id: empresa.value[0].id,
          nome: empresa.value[0].nome,
          tiposDeSeguro: new FormArray(this.tiposDeSeguroArray.map((tp) => {
            let tipoDeSeguroVinculado = empresa.value[0].tiposDeSeguro.find(item => item.tipoDeSeguro_id === tp.id);

            return new FormGroup({
              tipoSeguro_Id: new FormControl(tp.id),
              selecionado: new FormControl(tipoDeSeguroVinculado !== undefined),
              permitido_Abrir: new FormControl({ value: (tipoDeSeguroVinculado?.permitido_Abrir || false), disabled: !(tipoDeSeguroVinculado !== undefined) })
            })
          }))
        });
      });
    });
  }

  createForm() {
    this.titulo = 'Nova Empresa';
    this.tipoSeguroService.getAll().subscribe(response => {
      this.tiposDeSeguroArray = response.value;

      this.form = this.fb.group({
        id: new FormControl(0),
        nome: new FormControl(undefined),
        tiposDeSeguro: new FormArray(this.tiposDeSeguroArray.map((tp) => {
          return new FormGroup({
            tipoSeguro_Id: new FormControl(tp.id),
            selecionado: new FormControl(false),
            permitido_Abrir: new FormControl({value: false, disabled: true})
          })
        }))
      });
    });
  }

  public get tiposSeguro(): FormArray {
    return this.form.get('tiposDeSeguro') as FormArray;
  }

  TogglePermitidoAbrir(item: any) {
    if (item.get('selecionado').value) {
      item.get('permitido_Abrir').enable();
    } else {
      item.get('permitido_Abrir').disable();
      item.get('permitido_Abrir').setValue(false);
    }
  }

  //Filter
  filterSeguros(response) {
    this.itensSelecionados = _.pluck(response.this.objetoEdit.tiposDeSeguroArray, 'tipoDeSeguro_id');

    response.this.objetoEdit.tiposDeSeguroArray.forEach(el => {
      if (el.permitido_Abrir) this.itensSelecionados2.push(el.tipoDeSeguro_id);
    });

    this.createFormEditar();
  }

  //Send Data
  onSubmit() {
    this.submitted = true;
    this.loading = true;
    const post = { ...this.form.value, tiposDeSeguro: this.form.value.tiposDeSeguro.filter(item => item.selecionado) };

    this.empresaService.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: post.id === 0 });
        if (this.titulo == 'Editar Empresa') {
          this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Empresa Editada' });
          this.createForm()
        } else {
          this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Empresa Cadastrada' });
          this.createForm()
        }
      }
    }, (error) => this.showError(error));
  }

  //Checkbox
  saveCheckBox(event) {
    if (event.target.checked) {
      this.itensSelecionados.push(parseInt(event.target.value));
    } else {
      this.itensSelecionados = _.without(this.itensSelecionados, parseInt(event.target.value));
    }
  }

  testeMarcado(e) {
    console.log(this.itensSelecionados2);
    console.log(e);
  }

  checkSeguros(segurosID) {
    return _.contains(this.itensSelecionados, parseInt(segurosID));
  }

  //Layout
  onClosePanel() {
    this.setResult({} as Result);
    window.history.back();
  }

}


