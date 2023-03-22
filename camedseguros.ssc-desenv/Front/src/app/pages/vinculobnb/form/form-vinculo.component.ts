import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, MotivoRecusa } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { VinculoService } from '../service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-form-vinculo',
  templateUrl: './form-vinculo.component.html',
  styleUrls: ['./form-vinculo.component.css']
})
export class FormVinculoComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: MotivoRecusa;
  titulo: string;
  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() vinculo: any;
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private vinculoService: VinculoService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  newFormEdit(){
    this.titulo = 'Editar Vínculo';
    this.vinculoService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
      this.form = this.fb.group({
        id: value[0].id,
        nome: value[0].nome
      });
    })
  }

  newForm(){
    this.titulo = 'Novo Vínculo';
    this.form = this.fb.group({
      id: [0],
      nome: [null]
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
    // this.closePanel.emit(true);
    window.history.back();
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    const newValue = this.form.controls.id.value === 0;

    this.vinculoService.save(this.form.value).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: newValue });
        if(this.id){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Vínculo Editado'});
          this.newFormEdit()
        }else{
          this.newForm();
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Vínculo Cadastrado'});
        }

      }
    }, (error) => this.showError(error));
  }

}

