//#region Imports
import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, ParametrosSistemaService } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { VariaveisDeEmailService } from '../service';
import { MessageService } from 'primeng/api';
import { VariaveisDeEmail } from 'src/app/core/models/variaveisdeemail.model';
//#endregion

@Component({
  selector: 'app-form-variaveis',
  templateUrl: 'form-variaveis-email.component.html'
})

export class FormVariaveisComponent extends BaseComponent implements OnInit {
  submitted = false;
  display = false;
  post: VariaveisDeEmail;
  titulo: string;
  parametros: any = []
  editando: boolean = false;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel= new EventEmitter<any>();
  @Input() acao: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
              fb: FormBuilder,
              route: ActivatedRoute,
              router: Router,
              private acaoService: VariaveisDeEmailService,
              private parametrosService: ParametrosSistemaService,
              private messageService: MessageService,
              ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  getFormEdit(){
    this.acaoService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
      this.titulo = 'Editar Variável de E-mail';
      this.editando = true;
      this.form = this.fb.group({
      id: this.id,
      nome: value[0].nome,
    });
    })
  }

  getForm(){
    this.editando = false;
    this.titulo = 'Nova Variável de E-mail';
    this.form = this.fb.group({
      id: [''],
      nome: [''],
      ParametroId: null
    });

    if (this.id) {
     this.getFormEdit()
    }
  }

  ngOnInit(): void {
    this.parametrosService.get(`$select=id,parametro,VariaveisDeEmail_Id`).subscribe(response => {
      response.value.map(parametro => { this.parametros.push({  id: parametro.id,  nome: parametro.parametro  }) });
    })
    this.getForm();
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
      ParametroId: this.editando ? 0 : this.f.ParametroId.value
    };

    this.acaoService.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: post.id === 0 });
        if(this.titulo == 'Nova Variável de E-mail'){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Variável de E-mail Cadastrada'});
          this.getForm()
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Variável de E-mail Editada'});
          this.getFormEdit()
        }
      }
    }, (error) => this.showError(error));
  }
}
