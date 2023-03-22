//#region Imports
import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, Frames } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { FramesService } from '../service';
import { AcaoDeAcompanhamentoService } from '../../acaodeacompanhamento';
import _ from 'underscore';
import { MessageService } from 'primeng/api';

//#endregion

@Component({
  selector: 'app-form-frames',
  templateUrl: './form-frames.component.html',
  styleUrls: ['./form-frames.component.css']
})

export class FormFramesComponent extends BaseComponent implements OnInit {

  //#region Variaveis
  submitted = false;
  display = false;
  post: Frames;
  titulo: string;
  acoesAcompanhamento$: any;
  itensSelecionados: any = [];

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() frame: any;
  @Input() id: any;
  //#endregion

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private frameService: FramesService,
    private acaoService: AcaoDeAcompanhamentoService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);

  }

  ngOnInit() {
    this.submitted = false;
    this.loading = false;
    this.itensSelecionados = [];


    this.createForm();
  }

  createFormEdit(){
    this.frameService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
      this.form = this.fb.group({
        id: value[0].id,
        nome: value[0].nome,
        acaoacompanhamento: [this.itensSelecionados]
      });
    })
  }

  createForm(){
    this.getAcoes();
    this.titulo = 'Novo Frame';
    this.form = this.fb.group({
      id: [''],
      nome: [''],
      acaoacompanhamento: ['']
    });
    if (this.id) {
      this.titulo = 'Editar Frame';
      this.frameService.getAcoesByFrame(this.id).subscribe(response => {
        this.filterAcoesID(response);
        this.getAcoes();
      });
      this.createFormEdit()
    }

  }

  //Data
  getAcoes() {
    this.acoesAcompanhamento$ = this.acaoService.getAll();
  }

  //Filter
  filterAcoesID(response) {
    this.itensSelecionados = _.pluck(response.value[0].acoesAcompanhamento, 'acaoDeAcompanhamento_Id');
    this.createFormEdit()
  }

  //Send Data
  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.f.id.value || 0,
      nome: this.f.nome.value,
      AcoesAcompanhamento: this.f.acaoacompanhamento.value
    };

    this.frameService.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: post.id === 0 });
        if(this.titulo == 'Editar Frame'){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Fame Editado'});
          this.createForm()
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Fame Cadastrado'});
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

  checkAcoes(acoesID) {
    return _.contains(this.itensSelecionados, parseInt(acoesID));
  }

  //Layout
  onClosePanel() {
    this.setResult({} as Result);
    window.history.back();
  }

}


