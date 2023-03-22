import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, CanalDeDistribuicao } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { CampanhaService } from '../service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-form-campanha',
  templateUrl: './form-campanha.component.html',
  styleUrls: ['./form-campanha.component.css']
})
export class FormCampanhaComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: CanalDeDistribuicao;
  titulo: string;
  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() canal: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private distribuicaoService: CampanhaService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  newFormEditar(){
    this.distribuicaoService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
      this.form = this.fb.group({
        id: value[0].id,
        nome: value[0].nome,
        ativo:value[0].ativo
      });
    })
  }

  newForm(){
    this.titulo = 'Nova Campanha';
    this.form = this.fb.group({
      id: [''],
      nome: [''],
      ativo: [false]
    });

    if (this.id) {
      this.titulo = 'Editar Campanha';
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
      nome: this.f.nome.value,
      ativo: this.f.ativo.value
    };

    this.distribuicaoService.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: post.id === 0 });
        if(this.titulo == 'Nova Campanha'){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Campanha Cadastrada'});
          this.newForm()
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Campanha Editada'});
          this.newFormEditar()
        }
      }
    }, (error) => this.showError(error));
  }
}



