import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, Situacao } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { TiposCancelamentoService } from '../service';
import { GrupoAgenciaService } from '../../grupoagencias';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-form-tipos-cancelamento',
  templateUrl: './form-tipos-cancelamento.component.html',
  styleUrls: ['./form-tipos-cancelamento.component.css']
})

export class FormTiposCancelamentoComponent extends BaseComponent implements OnInit {

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
    private cancelamentoService: TiposCancelamentoService,
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
    const newValue = this.form.controls.id.value === 0;

    this.cancelamentoService.save(this.form.value).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: newValue});
        if(this.id){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Tipo de Cancelamento Editado'});
          this.newFormEdit();
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Tipo de Cancelamento Cadastrado'});
          this.newForm();
        }
      }
    }, (error) => this.showError(error));
  }

  newFormEdit(){
    this.titulo = 'Editar Tipo de Cancelamento';
    this.getGrupos();
    this.cancelamentoService.get(`$filter=id eq ${this.id}&$expand=grupoAgencia`).subscribe(({value})=>{
      this.form = this.fb.group({
        id: value[0].id,
        descricao: value[0].descricao,
        grupoagencia: value[0].grupoAgencia.id
      });
  })
  }

  newForm() {
    this.titulo = 'Novo Tipo de Cancelamento';
    this.getGrupos();
    this.form = this.fb.group({
      id: [0],
      descricao: [null],
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

