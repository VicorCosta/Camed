import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Result, Situacao, AgenciaService } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { AgenciaTipoService } from '../service';

import { MessageService, SelectItem } from 'primeng/api';
import { TiposAgenciaService } from '../../tiposDeAgencia';

@Component({
  selector: 'app-form-agencia-tipo',
  templateUrl: './form-agencia-tipo.component.html',
  styleUrls: ['./form-agencia-tipo.component.css']
})

export class FormAgenciaTipoComponent extends BaseComponent implements OnInit {

  submitted = false;
  display = false;
  post: Situacao;
  titulo: string;
  agencias$: SelectItem[] = [];
  tipos$: any;
  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() id: any;

  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private service: AgenciaTipoService,
    private agenciaService: AgenciaService,
    private tiposervice: TiposAgenciaService,
    private messageService: MessageService) {
    super(authenticationService, fb, route, router);

    const agenciaTipoAgencia = this.router.getCurrentNavigation()
    this.route.params.subscribe(params => this.id = params['id']);
  }

  ngOnInit() {
    this.getAgencias();
    this.getTipos();

    this.newForm()
  }


  onClosePanel() {
    this.setResult({} as Result);
    // this.closePanel.emit(true);
    window.history.back();
    this.newForm();
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const newValue = this.form.controls.Id.value === 0;

    console.log(this.form.value)
    this.service.save(this.form.value).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        if (this.id) {
          this.newFormEdit()
          this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Agência x Tipo de Agência Editada' });
        } else {
          this.newForm()
          this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Agência x Tipo de Agência Cadastrada' });
        }
        this.eventoConcluido.emit({ adicionar: newValue });
      }
    }, (error) => this.showError(error));
  }

  newForm() {
    this.form = this.fb.group({
      Id: [0],
      AgenciaId: [null],
      TipoDeAgenciaId: [null]
    });

    if (this.id) {
      this.newFormEdit()
    }
  }

  newFormEdit() {
    this.titulo = 'Editar Agência x Tipos de Agência';
    this.service.get(`$filter=id eq ${this.id}&$expand=agencia,tipoDeAgencia`).subscribe(({ value }) => {
      console.log(value)
      this.form = this.fb.group({
        Id: value[0].id,
        AgenciaId: value[0].agencia.id,
        TipoDeAgenciaId: value[0].tipoDeAgencia.id
      });
    })
  }

  getAgencias() {
    this.agenciaService.get('$select=id,nome&$orderby=nome', false).subscribe((data) => {
      data.forEach(element => {
        this.agencias$.push({
          label: element.Nome,
          value: element.Id
        });
      });
    });
  }

  getTipos() {
    this.tipos$ = this.tiposervice.getAll();
  }

}

