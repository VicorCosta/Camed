import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, Situacao, TipoProdutoService } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { TiposCategoriaService } from '../service';
import { MessageService } from 'primeng/api';


@Component({
  selector: 'app-form-tipos-categoria',
  templateUrl: './form-tipos-categoria.component.html',
  styleUrls: ['./form-tipos-categoria.component.css']
})

export class FormTiposCategoriaComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: Situacao;
  titulo: string;
  tipoProdutos$: any;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() item: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private categoriaService: TiposCategoriaService,
    private produtoService: TipoProdutoService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }


  ngOnInit(): void {
    this.newForm();
  }

  onClosePanel() {
    this.setResult({} as Result);
    // this.closePanel.emit(true);
    window.history.back()
    this.newForm();
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const newValue = this.form.controls.id.value === 0;

      this.categoriaService.save(this.form.value).subscribe(response => {
        this.setResult(response);
        if (response.successfully) {
          this.eventoConcluido.emit({ adicionar: newValue });
          if(this.titulo == 'Editar Tipo de Categoria'){
            this.newFormEdit();
            this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Tipo de Categoria Editada'});
          }else{
            this.newForm();
            this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Tipo de Categoria Cadastrado'});
          }
        }
      }, (error) => this.showError(error));

  }

  newFormEdit(){
    this.titulo = 'Editar Tipo de Categoria';
    this.getTiposProdutos();
    this.categoriaService.get(`$filter=id eq ${this.id}&$expand=tipoDeProduto`).subscribe(({value})=>{
      this.form = this.fb.group({
        id: value[0].id,
        descricao: value[0].descricao,
        tipodeproduto: value[0].tipoDeProduto.id
      });
    })
  }

  newForm() {
    this.getTiposProdutos();
    this.form = this.fb.group({
      id: [0],
      descricao: [null],
      tipodeproduto: [0]
    });
    if (this.id) {
     this.newFormEdit()
    }
  }

  getTiposProdutos() {
    this.tipoProdutos$ = this.produtoService.get();
  }
}

