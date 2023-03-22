import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, AcaoDeAcompanhamento, AreaDeNegocio } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { AreaDeNegocioService } from '../service';
import { MessageService } from 'primeng/api';
@Component({
  selector: 'app-form-areadenegocio',
  templateUrl: './form-areadenegocio.component.html',
  styleUrls: ['./form-areadenegocio.component.css']
})
export class FormAreaDeNegocioComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: AreaDeNegocio;
  titulo: string;
  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() area: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private areadenegocioService: AreaDeNegocioService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  newFormEditar(){
    this.areadenegocioService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
    this.titulo = 'Editar Área de Negócio';
      this.form = this.fb.group({
        id: value[0].id,
        nome: value[0].nome
      });
    })
  }

  newForm(){
    this.titulo = 'Nova Área de Negócio';
    this.form = this.fb.group({
      id: [''],
      nome: ['']
    });
    if (this.id) {
      this.newFormEditar()
    }

  }

  ngOnInit() {
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

    this.areadenegocioService.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: post.id === 0 });
        if(this.titulo == 'Nova Área de Negócio'){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Área de Negócio Cadastrada'});
          this.newForm()
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Área de Negócio Editada'});
          this.newFormEditar()
        }
      }
    }, (error) => this.showError(error));
  }
}
