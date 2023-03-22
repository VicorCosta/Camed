import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, Situacao } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { SituacaoService } from '../service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-form-situacoes',
  templateUrl: './form-situacoes.component.html',
  styleUrls: ['./form-situacoes.component.css']
})

export class FormSituacoesComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: Situacao;
  titulo: string;
  contagensDoSLA: any = [
    { tipo: 'I', nome: "Inicial" },
    { tipo: 'C', nome: "Contar" },
    { tipo: 'P', nome: "Parar" },
    { tipo: 'A', nome: "Avulso" }
  ];

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() situacao: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private messageService: MessageService,
    private situacaoService: SituacaoService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
}

newFormEdit(){
  this.situacaoService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
    this.titulo = 'Editar Situação';
    this.form = this.fb.group({
      id: value[0].id,
      nome: value[0].nome,
      tipo: value[0].tipo,
      temposla: value[0].tempoSLA,
      efimfluxo: value[0].eFimFluxo,
      pendenciacliente: value[0].pendenciaCliente
    })
  });
}

newForm() {
  this.titulo = 'Nova Situação';
  this.form = this.fb.group({
    id: [0],
    nome: [null],
    tipo: "",
    temposla: [0],
    efimfluxo: [false],
    pendenciacliente: [false]
  });

  if (this.id) {
   this.newFormEdit()
  }
}

  ngOnInit(): void {
      this.newForm();
  }

  onClosePanel() {
    this.setResult({} as Result);
    this.closePanel.emit(true);
    this.newForm();
    window.history.back()
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.f.id.value || 0,
      nome: this.f.nome.value,
      tipo: this.f.tipo.value,
      tempoSLA: this.f.temposla.value,
      eFimFluxo: this.f.efimfluxo.value,
      pendenciaCliente: this.f.pendenciacliente.value
    };

    this.situacaoService.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        if( this.titulo == 'Nova Situação'){
          this.newForm();
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Situação Cadastrada'});
        }else{
          this.newFormEdit();
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Situação Editada'});
        }
        this.eventoConcluido.emit({ adicionar: post.id === 0 });
      }
    }, (error) => this.showError(error));
  }
}

