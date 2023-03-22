import { Component, Input, OnInit, Output } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { AuthenticationService, BaseComponent } from 'src/app/core';
import { EventEmitter } from 'stream';
import { TipomorteService } from '../service/tipomorte.service';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class FormComponent extends BaseComponent implements OnInit {
  submitted = false;
  display = false;
  titulo: string;

  // @Output() eventoConcluido = new EventEmitter();
  // @Output() closePanel= new EventEmitter();
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
              fb: FormBuilder,
              route: ActivatedRoute,
              router: Router,
              private messageService: MessageService,
              private tipomorteService: TipomorteService
            ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  newFormEdit(){
    this.titulo = 'Editar Tipo de Morte';
    this.tipomorteService.get(`$filter=id eq ${this.id}`).subscribe(({value})=>{
      console.log('response', value)
      this.form = this.fb.group({
        id: this.id,
        descricao: value[0].descricao
      });
    })
  }

  newForm(){
    this.titulo = 'Novo Tipo de Morte';
    this.form = this.fb.group({
      id: [''],
      descricao: ['']
    });

    if(this.id){
     this.newFormEdit()
    }
  }


  ngOnInit(): void {
    this.newForm();
  }

  goback(){
    window.history.back()
  }

  onSubmit(){
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.f.id.value || 0,
      descricao: this.f.descricao.value
    };

    this.tipomorteService.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
       if(  this.titulo == 'Novo Tipo de Morte'){
         this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Tipo de Morte Cadastrada'});
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Tipo de Morte Editada'});
        }
        this.newForm()
      }
    }, (error) => this.showError(error));
  }
}
