import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { FeriadoService } from '../service';
import * as moment from 'moment';
import { MessageService } from 'primeng/api';


@Component({
  selector: 'app-form-feriado',
  templateUrl: './form-feriado.component.html',
  styleUrls: ['./form-feriado.component.css']
})

export class FormFeriadoComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  titulo: string;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() item: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private feriadoService: FeriadoService,
    private messageService: MessageService)
   {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }


  newForm() {
    this.titulo = 'Novo Feriado';

    this.form = this.fb.group({
      id: 0,
      data: [null],
      pais: [null],
      estado: [null],
      municipio: [0],
      descricao: [null]
    });

    if (this.id) {
     this.newFormEditar()
    }
  }

  newFormEditar(){
    this.feriadoService.get(`$filter=id eq ${this.id}&$expand=municipio`).subscribe(({value})=>{
      this.titulo = 'Editar Feriado';
      this.form = this.fb.group({
        id: value[0].id,
        data: moment(value[0].data).format('YYYY-MM-DD'),
        pais: value[0].pais,
        estado: value[0].estado,
        municipio: value[0].municipio.id,
        descricao: value[0].descricao
      });
    })
  }

  ngOnInit(): void {
    this.newForm()
  }

  onClosePanel() {
    this.setResult({} as Result);
    // this.closePanel.emit(true);
    window.history.back();
    this.form.reset();
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    const newValue = this.form.controls.id.value === 0;

      this.feriadoService.save(this.form.value).subscribe(response => {
        this.setResult(response);
        if (response.successfully) {
          this.eventoConcluido.emit({ adicionar: newValue });
          if(this.id){
            this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Feriado Editado'});
            this.newFormEditar();
          }else{
            this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Feriado Cadastrado'});
            this.newForm();
          }
          this.showError("")
        }
      }, (error) => this.showError(error));

    }


}

