//#region Imports
import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, AcaoDeAcompanhamento } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { AcaoService } from '../service';
import { MessageService } from 'primeng/api';
//#endregion

@Component({
  selector: 'app-form-acoes',
  templateUrl: 'form-acoes.component.html'
})

export class FormAcoesComponent extends BaseComponent implements OnInit {
  submitted = false;
  display = false;
  post: AcaoDeAcompanhamento;
  titulo: string;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel= new EventEmitter<any>();
  @Input() acao: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
              fb: FormBuilder,
              route: ActivatedRoute,
              router: Router,
              private acaoService: AcaoService,
              private messageService: MessageService,
              ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  getFormEdit(){
    this.acaoService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
      this.titulo = 'Editar Ação';
      this.form = this.fb.group({
      id: this.id,
      nome: value[0].nome,
      descricao: value[0].descricao
    });
    })
  }

  getForm(){
    this.titulo = 'Nova Ação';
    this.form = this.fb.group({
      id: [''],
      nome: [''],
      descricao: ''
    });

    if (this.id) {
     this.getFormEdit()
    }

  }

  ngOnInit(): void {
  this.getForm()
  }

  onClosePanel() {
    this.setResult({} as Result);
    window.history.back()
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.f.id.value || 0,
      nome: this.f.nome.value,
      descricao: this.f.descricao.value,
      papel: 1
    };

    this.acaoService.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: post.id === 0 });
        if(this.titulo == 'Nova Ação'){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Ação Cadastrada'});
          this.getForm()
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Ação Editada'});
          this.getFormEdit()
        }
      }
    }, (error) => this.showError(error));
  }
}
