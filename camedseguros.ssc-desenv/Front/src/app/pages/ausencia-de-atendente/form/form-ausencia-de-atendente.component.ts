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

import { AusenciaDeAtendenteServices } from "../service";
import { GrupoAgenciaService } from "../../grupoagencias";
import { TipoSeguroService } from "../../tiposdeseguro";
import { GrupoService } from "../../grupo";
import { EmpresaService } from "../../empresas";
import { AreaDeNegocioService } from "../../areadenegocio";
import { UsuarioService } from "../../usuario";
import { post } from "jquery";
import moment from "moment";

@Component({
  selector: "app-form-ausencia-de-atendente",
  templateUrl: "./form-ausencia-de-atendente.component.html",
  styleUrls: ["./form-ausencia-de-atendente.component.css"],
})
export class FormAusenciaDeAtendenteComponent
  extends BaseComponent
  implements OnInit
{
  submitted = false;
  display = false;
  titulo: string;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() usuario: any;
  @Input() id: any;
  situacaoAtualSelecionada: any;

  empresas$: Observable<any>;

  atendentes: SelectItem[] = [];

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private service: AusenciaDeAtendenteServices,
    private usuarioService: UsuarioService,
    private messageService: MessageService
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe((params) => (this.id = params["id"]));
  }

  ngOnInit(): void {
    this.usuarioService
      .get("$select=id,nome&$orderby=nome", false)
      .subscribe((data) => {
        data.forEach((element) => {
          this.atendentes.push({
            label: element.Nome,
            value: element.Id,
          });
        });
        this.getForm();
      });

    this.getForm();
  }

  newFormEdit() {
    console.log(this.form.value.atendente);
    this.service
      .getAll(`$filter=id eq ${this.id}&$expand=atendente`)
      .subscribe(({ value }) => {
        console.log(value[0].atendente)
        this.titulo = "Editar Ausência de Atendente";
        this.situacaoAtualSelecionada = value[0].atendente;
        this.form = this.fb.group({
          id: this.id,
          atendente: value[0].atendente_Id,
          dataInicioAusencia: moment(value[0].dataInicioAusencia).format(
            "YYYY-MM-DD"
          ),
          dataFinalAusencia: moment(value[0].dataFinalAusencia).format(
            "YYYY-MM-DD"
          ),
        });
      });
  }

  getForm() {
    this.titulo = "Nova Ausência de Atendente";
    this.form = this.fb.group({
      id: 0,
      atendente: null,
      dataInicioAusencia: null,
      dataFinalAusencia: null,
    });

    this.situacaoAtualSelecionada = null;

    if (this.id) {
      this.newFormEdit();
    }
  }

  searchSituacaoAtual(event) {
    this.usuarioService
      .get(
        `$select=id,nome&$filter=(contains(nome,'${event.query}'))&$orderby=nome`
      )
      .subscribe((data) => {
        this.atendentes = data.value;
      });
  }

  searchAtendentes(event) {
    let query = `$filter=contains(nome, '${event.query}') eq true`;
    this.usuarioService.get(query).subscribe((data) => {
      this.atendentes = data.value;
    });
  }

  setSituacaoAtual(atendente) {
    this.form.controls.atendente.setValue(atendente.id);
    console.log(atendente.id)
  }

  onClosePanel() {
    this.setResult({} as Result);
    // this.closePanel.emit(true);
    window.history.back();
  }

  onSubmit() {
    console.log(
      "this.form.value",
      this.form.value,
      this.situacaoAtualSelecionada
    );

    this.submitted = true;
    this.loading = true;
    const newValue = this.form.controls.id.value === 0;
    const post = {
      id: this.form.value.id || 0,
      atendente_id: this.form.value.atendente,
      datainicioausencia: this.form.value.dataInicioAusencia == null || this.form.value.dataInicioAusencia == "" ? "0001-01-01" : this.form.value.dataInicioAusencia,
      datafinalausencia: this.form.value.dataFinalAusencia == null || this.form.value.dataFinalAusencia == "" ? "0001-01-01" : this.form.value.dataFinalAusencia,
    };

    this.service.save(post).subscribe(
      (response) => {
        this.setResult(response);
        if (response.successfully) {
          this.eventoConcluido.emit({ adicionar: newValue });
          if (this.titulo == "Nova Ausência de Atendente") {
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Ausência de Atendente Cadastrada",
            });
            this.getForm();
          } else {
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Ausência de Atendente Editada",
            });
            this.newFormEdit();
          }
        }
      },
      (error) => this.showError(error)
    );
  }
}
