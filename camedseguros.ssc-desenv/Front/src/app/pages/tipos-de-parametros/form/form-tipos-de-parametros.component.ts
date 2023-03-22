//#region Imports
import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, Empresas } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { TipodeParametroService } from '../service';
import _ from 'underscore';

import { TipoSeguroService } from '../../tiposdeseguro';
import { MessageService } from 'primeng/api';
//#endregion

@Component({
  selector: 'app-form-tipo-de-parametros',
  templateUrl: './form-tipos-de-parametros.component.html',
  styleUrls: ['./form-tipos-de-parametros.component.css']
})

export class FormTipodeParametrosComponent extends BaseComponent implements OnInit {

  //#region Variaveis
  submitted = false;
  display = false;
  post: Empresas;
  titulo: string;
  tiposDeSeguros$: any;
  itensSelecionados: any = [];

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() empresa: any;
  @Input() id: any;
  //#endregion

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private tipodeParametroService: TipodeParametroService,
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
    
    if (this.id) {
      
    }
    this.createForm()
  }
  
  createFormEditar(){
    
    this.titulo = 'Editar Parâmetro';
    this.tipodeParametroService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
      this.form = this.fb.group({
      id: value[0].id,
      nome: value[0].nome,
      tipodeseguros: [this.itensSelecionados]
    });
  })
  }

  createForm(){
    this.getSeguros();
    this.titulo = 'Novo Parâmetro';
    this.form = this.fb.group({
      id: [''],
      nome: [''],
      tipodeseguros: ['']
    });

    if(this.id){
      this.createFormEditar()
      this.createFormEditar()
      this.tipodeParametroService.getSegurobyEmpresa(this.id).subscribe(response => {
        this.filterSeguros(response);
        this.getSeguros();
      });
    }    
  }

  //Data
  getSeguros() {
    this.tiposDeSeguros$ = this.tipoSeguroService.getAll();
  }

  //Filter
  filterSeguros(response) {
    this.itensSelecionados = _.pluck(response.value[0].tiposDeSeguro, 'tipoDeSeguro_id');
    this.createFormEditar();
  }

  //Send Data
  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.f.id.value || 0,
      Nome: this.f.nome.value,
      TiposDeSeguro: this.f.tipodeseguros.value
    };

    this.tipodeParametroService.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: post.id === 0 });
        if(this.titulo == 'Editar Parâmetro'){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Parâmetro editado'});
          this.createForm()
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Parâmetro cadastrado'});
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

  checkSeguros(segurosID) {
    return _.contains(this.itensSelecionados, parseInt(segurosID));
  }

  //Layout
  onClosePanel() {
    this.setResult({} as Result);
    window.history.back();
  }

}


