import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, MotivoCancelamento } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { MotivoCancelamentoService } from '../service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-form-motivo-cancelamento',
  templateUrl: './form-motivo-cancelamento.component.html',
  styleUrls: ['./form-motivo-cancelamento.component.css']
})

export class FormMotivoCancelamentoComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: MotivoCancelamento;
  titulo: string;
  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() motivo: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private motivocancelamentoService: MotivoCancelamentoService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  newFormEditar(){
    this.titulo = 'Editar Motivo de Cancelamento';
    this.motivocancelamentoService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
      this.form = this.fb.group({
        id: value[0].id,
        descricao: value[0].descricao
      });
  })
  }

  newForm(){
    this.titulo = 'Novo Motivo de Cancelamento';
    this.form = this.fb.group({
      id: [0],
      descricao: ['']
    });

    if (this.id) {
      this.newFormEditar()
    }
  }

  ngOnInit(): void {
    this.newForm()
  }



  onClosePanel() {
    this.setResult({} as Result);
    this.closePanel.emit(true);
    window.history.back();
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const newValue = this.form.controls.id.value === 0;
    this.motivocancelamentoService.save(this.form.value).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: newValue});
        if(this.id){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Motivo de Cancelamento Editado'});
          this.newFormEditar()
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Motivo de Cancelamento Cadastrado'});
          this.newForm()
        }

      }
    }, (error) => this.showError(error));
  }

}

