import {
  Component,
  Output,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
} from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import {
  BaseComponent,
  AuthenticationService,
  Result,
  AgenciaService,
  AppConstants,
} from "src/app/core";
import { FormBuilder } from "@angular/forms";
import { Observable } from "rxjs";
import { MessageService, SelectItem } from "primeng/api";
import _ from "underscore";

import { AgendamentoDeLigacaoServices } from "../service";
import { GrupoAgenciaService } from "../../grupoagencias";
import { TipoSeguroService } from "../../tiposdeseguro";
import { GrupoService } from "../../grupo";
import { EmpresaService } from "../../empresas";
import { AreaDeNegocioService } from "../../areadenegocio";
import { UsuarioService } from "../../usuario";
import { post } from "jquery";
import * as moment from 'moment';


@Component({
  selector: "app-form-agendamento-de-ligacao",
  templateUrl: "./form-agendamento-de-ligacao.component.html",
  styleUrls: ["./form-agendamento-de-ligacao.component.css"],
})
export class FormAgendamentoDeLigacaoComponent
  extends BaseComponent
  implements OnInit
{
  submitted = false;
  display = false;
  titulo: string;
  agenciaSelecionada: any;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() usuario: any;
  @Input() id: any;

  empresas$: Observable<any>;

  agencias: SelectItem[] = [];
  grupos: SelectItem[] = [];
  areasDeNegocio: SelectItem[] = [];
  atendentes: SelectItem[] = [];
  gruposAgencias: SelectItem[] = [];
  tipodeseguro: SelectItem[] = [];

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private service: AgendamentoDeLigacaoServices,
    private agenciaService: AgenciaService,
    private messageService: MessageService
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe((params) => (this.id = params["id"]));
  }

  ngOnInit(): void {
    this.getForm();
  }

    getForm() {
    this.titulo = "Novo Agendamento";
    this.form = this.fb.group({
      id: 0,
      NSolicitacao: null,
      motivo: null,
      dataagendamento: null,
    });

    
  }

  onClosePanel() {
    this.setResult({} as Result);
    // this.closePanel.emit(true);
    window.history.back();
  }

  search(event) {
    this.agenciaService
      .get(
        `$select=id,nome&$filter=(contains(nome,'${event.query}'))&$orderby=nome`
      )
      .subscribe((data) => {
        this.agencias = data.value;
      });
  }

  setAgencia(agencia) {
    this.form.controls.agencia.setValue(agencia.id);
  }

  showAgencia() {
    return _.contains(
      this.form.controls.grupos.value,
      AppConstants.GrupoGerenteDeAgencia
    );
  }

  showGruposDeAgenciaEAreasDeNegocio() {
    return (
      _.contains(this.form.controls.grupos.value, AppConstants.GrupoGerente) ||
      _.contains(this.form.controls.grupos.value, AppConstants.GrupoAtendente)
    );
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.form.value.id || 0,
      dataagendamento: this.form.value.dataagendamento ? moment(this.form.value.dataagendamento).format("DD/MM/yyyy"): null ,
      nsolicitacao: this.form.value.NSolicitacao == null ? 0 : this.form.value.NSolicitacao,
      motivo: this.form.value.motivo,
    };

    this.service.save(post).subscribe(
      (response) => {
        this.setResult(response);
        if (response.successfully) {
          this.eventoConcluido.emit({ adicionar: this.id === 0 });
          if(this.titulo == 'Novo Agendamento'){
            this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Agendamento Cadastrado'});
            this.getForm()
          }
         
        }
      },
      (error) => this.showError(error)
    );
  }
}
