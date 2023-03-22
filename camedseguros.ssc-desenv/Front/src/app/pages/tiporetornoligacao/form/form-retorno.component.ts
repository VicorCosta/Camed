import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, MotivoRecusa } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { RetonoligacaoService } from '../service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-form-retorno',
  templateUrl: './form-retorno.component.html',
  styleUrls: ['./form-retorno.component.css']
})
export class FormRetornoComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: MotivoRecusa;
  titulo: string;
  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() tipo: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private retornoligacaoService: RetonoligacaoService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }
  ngOnInit(): void {
    this.newForm()
  }

  newFormEditar(){
    this.titulo = 'Editar Tipo de Retorno';
    this.retornoligacaoService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
      this.form = this.fb.group({
        id: value[0].id,
        descricao: value[0].descricao
      });
    })
  }

  newForm(){
    this.titulo = 'Novo Tipo de Retorno';
    this.form = this.fb.group({
      id: [''],
      descricao:null
    });

    if (this.id) {
      this.newFormEditar()
    }
  }

  onClosePanel() {
    this.setResult({} as Result);
    // this.closePanel.emit(true);
    window.history.back()
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.f.id.value || 0,
      descricao: this.f.descricao.value
    };

    this.retornoligacaoService.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: post.id === 0 });
        if(this.id){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Tipo de Retorno Editado'});
          this.newFormEditar()
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Tipo de Retorno Cadastrado'});
          this.newForm()
        }

      }
    }, (error) => this.showError(error));
  }

}
