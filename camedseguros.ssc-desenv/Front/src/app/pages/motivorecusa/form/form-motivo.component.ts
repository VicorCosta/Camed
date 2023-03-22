import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, MotivoRecusa } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { MotivoRecusaService } from '../service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-form-motivo',
  templateUrl: './form-motivo.component.html',
  styleUrls: ['./form-motivo.component.css']
})
export class FormMotivoComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: MotivoRecusa;
  titulo: string;
  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() motivo: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private motivorecusaService: MotivoRecusaService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  newFormEdit(){
    this.titulo = 'Editar Motivo de Recusa';
    this.motivorecusaService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
      this.form = this.fb.group({
        id: value[0].id,
        descricao: value[0].descricao,
        ativo: value[0].ativo
      });
    })
  }


  newForm(){
    this.titulo = 'Novo Motivo de Recusa';
    this.form = this.fb.group({
      id: [''],
      descricao: [''],
      ativo: false
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
    window.history.back()
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.f.id.value || 0,
      descricao: this.f.descricao.value,
      ativo: this.f.ativo.value
    };

    this.motivorecusaService.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.newForm()
        this.eventoConcluido.emit({ adicionar: post.id === 0 });
        if(this.id){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Motivo de Recusa Editado'})
          this.newFormEdit()
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Motivo de Recusa Cadastrado'});
          this.newForm()
        }
      }
    }, (error) => this.showError(error));
  }

}
