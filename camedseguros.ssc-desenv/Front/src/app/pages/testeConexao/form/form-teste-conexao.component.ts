
import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, Empresas } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import _ from 'underscore';

import { MessageService } from 'primeng/api';
import { testeConexao } from '../service/testeConexao';


@Component({
  selector: 'app-form-teste-conexao',
  templateUrl: './form-teste-conexao.component.html',
  styleUrls: ['./form-teste-conexao.component.css']
})

export class FormTesteConexaoComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: Empresas;
  titulo: string;
  gruposagencia$: any;
  ramosSeguro$: any;
  itemAgencia: any;
  itensRamo: any;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() seguro: any;


  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private testeConexao: testeConexao,
    private messageService: MessageService
  ) {
    super(authenticationService, fb, route, router);
  }

  ngOnInit() {
    this.submitted = false;
    this.loading = false;

    this.setForm();
  }


  setForm() {
    this.form = this.fb.group({
      email: ''
    });
  }

  validateEmail(email) {
    var re = /\S+@\S+\.\S+/;
    return re.test(email);
  }

  //Send Data
  onSubmit() {
    this.submitted = true;
    this.loading = true;

    if (!this.validateEmail(this.form.value.email)) {
      this.showError("Digite um email válido")
    } else {
      this.testeConexao.send(this.form.value.email).subscribe(() => {
        this.setForm();
        this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Teste de Conexão Enviado' });
        this.showError("")
      }, (error) => this.showError(error));
    }

  }

}


