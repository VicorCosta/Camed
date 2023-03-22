import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, User, Grupo, Result } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { GrupoAgenciaService } from '../service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-form-grupoagencias',
  templateUrl: './form-grupoagencias.component.html'
})
export class FormGrupoAgenciasComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: Grupo;
  titulo: string;
  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() grupo: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private grupoAgenciaService: GrupoAgenciaService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  newFormEdit(){
    this.grupoAgenciaService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
      this.titulo = 'Editar Grupo de Agência';
      this.form = this.fb.group({
        id: value[0].id,
        nome: value[0].nome
      });
    })
  }

  newForm(){
    this.titulo = 'Novo Grupo de Agência';
    this.form = this.fb.group({
      id: [''],
      nome: ['']
    });
    if (this.id) {
      this.newFormEdit()
    }
  }

  ngOnInit(): void {
    this.newForm()
  }

  onClosePanel() {
    this.setResult({} as Result);
    window.history.back();
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.f.id.value || 0,
      nome: this.f.nome.value
    };

    this.grupoAgenciaService.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: post.id === 0 });
        if(this.id){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Grupo de Agência Editada'});
          this.newFormEdit()
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Grupo de Agência Cadastrada'});
          this.newForm()
        }
      }
    }, (error) => this.showError(error));
  }

}
