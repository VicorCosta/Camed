import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { TextoPersonalizadoSeguradora } from '../service';
import { VW_SEGURADORA } from '../service';
import * as moment from 'moment';
import { MessageService, SelectItem } from 'primeng/api';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-form-textopersonalizadoseguro',
  templateUrl: './form-textopersonalizadoseguro.component.html',
  styleUrls: ['./form-textopersonalizadoseguro.component.css']
})
// 
export class FormTextoPersonalizadoSeguroComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  titulo: string;
  seguradoras$: Observable<any>;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() item: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private VW_SEGURADORA: VW_SEGURADORA,
    private tpsService: TextoPersonalizadoSeguradora,
    private messageService: MessageService)
   {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }


  newForm() {
    this.titulo = 'Novo Texto Personalizado Seguradora';
    this.form = this.fb.group({
      id: 0,
      Seguradora_Id: 0,
      Texto: [null]
    });

    if (this.id) {
     this.newFormEditar()
    }
  }

  newFormEditar(){
    this.tpsService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
      this.titulo = 'Editar Texto Personalizado Seguradora';
      this.form = this.fb.group({
        id: value[0].id,
        Seguradora_Id: value[0].seguradora_Id,
        Texto: value[0].texto
      });
    })
  }

  ngOnInit(): void {
    this.seguradoras$ = this.VW_SEGURADORA.get("$select=id,nm_Seguradora");
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

      this.tpsService.save(this.form.value).subscribe(response => {
        this.setResult(response);
        if (response.successfully) {
          this.eventoConcluido.emit({ adicionar: newValue });
          if(this.id){
            this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Texto Personalizado Seguradora Editado'});
            this.newFormEditar();
          }else{
            this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Texto Personalizado Seguradora Cadastrado'});
            this.newForm();
          }
          this.showError("")
        }
      }, (error) => this.showError(error));

    }


}

