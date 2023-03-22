import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, Situacao } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { TiposAgenciaService } from '../service';
import { GrupoAgenciaService } from '../../grupoagencias';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-form-tipos-agencia',
  templateUrl: './form-tipos-agencia.component.html',
  styleUrls: ['./form-tipos-agencia.component.css']
})

export class FormTiposAgenciaComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: Situacao;
  titulo: string;
  gruposagencia$: any;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() item: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private agenciaService: TiposAgenciaService,
    private grupoService: GrupoAgenciaService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  ngOnInit(): void {
    this.newForm()
  }


  onClosePanel() {
    this.setResult({} as Result);
    window.history.back();
    this.newForm();
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;


      const post = {
        Id: this.f.id.value || 0,
        Nome: this.f.nome.value,
        GrupoAgencia: this.f.grupoagencia.value
      };

      this.agenciaService.save(post).subscribe(response => {
        this.setResult(response);
        if (response.successfully) {
          this.eventoConcluido.emit({ adicionar: post.Id === 0 });
          if(this.id){
            this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Tipo Agência Editada'});
            this.newFormEdit();
          }else{
            this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Tipo Agência Cadastrada'});
            this.newForm();
          }
        }
      }, (error) => this.showError(error));

  }

  newFormEdit(){
    this.titulo = 'Editar Tipo de Agência';
    this.getGrupos();
    this.agenciaService.get(`$filter=id eq ${this.id}&$expand=grupoAgencia`).subscribe(({value})=>{
      this.form = this.fb.group({
        id: value[0].id,
        nome: value[0].nome,
        grupoagencia: value[0].grupoAgencia.id
      });
    })
  }


  newForm() {
    this.titulo = 'Novo Tipo de Agência';
    this.getGrupos();
    this.form = this.fb.group({
      id: [0],
      nome: [null],
      grupoagencia: [0]
    });

    if (this.id) {
     this.newFormEdit()
    }
  }

  getGrupos() {
    this.gruposagencia$ = this.grupoService.getAll();
  }
}

