import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { ExpedienteService } from '../service';
import * as moment from 'moment';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-form-expediente',
  templateUrl: './form-expediente.component.html',
  styleUrls: ['./form-expediente.component.css']
})

export class FormExpedienteComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  titulo: string;
  diaSemana: any = [
    { id: 1, nome: "Domingo" },
    { id: 2, nome: "Segunda-Feira" },
    { id: 3, nome: "Terça-Feira" },
    { id: 4, nome: "Quarta-Feira" },
    { id: 5, nome: "Quinta-Feira" },
    { id: 6, nome: "Sexta-Feira" },
    { id: 7, nome: "Sábado" },
  ];

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() item: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private expedienteService: ExpedienteService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);  }

    getFormEditar(){
      this.expedienteService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
        this.titulo = 'Editar Expediente';
        this.form = this.fb.group({
          id: value[0].id,
          dia: value[0].dia,
          horaInicialManha: moment(value[0].horaInicialManha).format("HH:mm"),
          horaFinalManha: moment(value[0].horaFinalManha).format("HH:mm"),
          horaInicialTarde: moment(value[0].horaInicialTarde).format("HH:mm"),
          horaFinalTarde: moment(value[0].horaFinalTarde).format("HH:mm")
        });
      })
    }

    getForm(){
      this.titulo = 'Novo Expediente';
      this.form = this.fb.group({
        id: 0,
        dia: "",
        horaInicialManha:"",
        horaFinalManha:"",
        horaInicialTarde:"",
        horaFinalTarde:""
      });
      if (this.id) {
       this.getFormEditar()
      }
    }

  ngOnInit(): void {
    this.getForm()
  }

  onClosePanel() {
    // this.setResult({} as Result);
    // this.closePanel.emit(true);
    // this.showError(null);
    // this.form.reset();

    window.history.back()
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    const newValue = this.form.controls.id.value === 0;

    if(this.horariosisNotEmpty(this.form.value))
    this.expedienteService.save(this.form.value).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.alertError = ''

      if(this.titulo == 'Novo Expediente'){
        this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Expediente Cadastrado'});
        this.getForm()
      }else{
        this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Expediente Editado'});
        this.getFormEditar()
      }
        this.eventoConcluido.emit({ adicionar: newValue });
      }else{
        this.alertError = response.notifications[0].value
      }
    }, (error) => this.showError(error));
  }

  horariosisNotEmpty(form) {
    if (
      form.id == null ||
      form.dia == "" ||
      form.horaFinalManha == "" ||
      form.horaFinalTarde == "" ||
      form.horaInicialManha == "" ||
      form.horaInicialTarde == "") {
        this.showError('Preencha todos os campos!')
        return false;
    } else {
        return true;
    }

  }

}

