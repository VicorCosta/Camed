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

import { MapeamentoDeatendenteServices } from "../service";
import { GrupoAgenciaService } from "../../grupoagencias";
import { TipoSeguroService } from "../../tiposdeseguro";
import { GrupoService } from "../../grupo";
import { EmpresaService } from "../../empresas";
import { AreaDeNegocioService } from "../../areadenegocio";
import { UsuarioService } from "../../usuario";
import { post } from "jquery";

@Component({
  selector: "app-form-mapeamento-de-atendente",
  templateUrl: "./form-mapeamento-de-atendente.component.html",
  styleUrls: ["./form-mapeamento-de-atendente.component.css"],
})
export class FormMapeamentoDeAtendenteComponent
  extends BaseComponent
  implements OnInit {
  submitted = false;
  display = false;
  titulo: string;
  agenciaSelecionada: any;
  // atendenteSelecionada: any;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() usuario: any;
  @Input() id: any;

  empresas$: Observable<any>;

  agencias: SelectItem[] = [];
  grupos: SelectItem[] = [];
  areasDeNegocio: SelectItem[] = [];
  atendentes: Observable<any>;
  gruposAgencias: SelectItem[] = [];
  tipodeseguro: SelectItem[] = [];

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private service: MapeamentoDeatendenteServices,
    private usuarioService: UsuarioService,
    private grupoAgenciaService: GrupoAgenciaService,
    private tipoSeguroService: TipoSeguroService,
    private grupoService: GrupoService,
    private agenciaService: AgenciaService,
    private areaDeNegocioService: AreaDeNegocioService,
    private messageService: MessageService
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe((params) => (this.id = params["id"]));
  }

  ngOnInit(): void {
    this.agenciaService
      .get(`$select=id,nome&$filter=(contains(nome,''))&$orderby=nome`)
      .subscribe((data) => {
        data.value.forEach((element) => {
          this.agencias.push({
            label: element.nome,
            value: element.id,
          });
        });
        this.getForm();
      });

    this.grupoService
      .get("$select=id,nome&$orderby=nome", false)
      .subscribe((data) => {
        data.forEach((element) => {
          this.grupos.push({
            label: element.Nome,
            value: element.Id,
          });
        });
        this.getForm();
      });

    this.grupoAgenciaService
      .get("$select=id,nome&$orderby=nome", false)
      .subscribe((data) => {
        data.forEach((element) => {
          this.gruposAgencias.push({
            label: element.Nome,
            value: element.Id,
          });
        });
        this.getForm();
      });

    this.tipoSeguroService
      .get("$select=id,nome&$orderby=nome", false)
      .subscribe((data) => {
        data.forEach((element) => {
          this.tipodeseguro.push({
            label: element.Nome,
            value: element.Id,
          });
        });
        this.getForm();
      });

    // this.usuarioService
    //   .get("$select=id,nome&$orderby=nome", false)
    //   .subscribe((data) => {
    //     data.forEach((element) => {
    //       this.atendentes.push({
    //         label: element.Nome,
    //         value: element.Id,
    //       });
    //     });
    //     this.getForm();
    //   });

    this.areaDeNegocioService
      .get("$select=id,nome&$orderby=nome", false)
      .subscribe((data) => {
        data.forEach((element) => {
          this.areasDeNegocio.push({
            label: element.Nome,
            value: element.Id,
          });
        });
        this.getForm();
      });

    this.getForm();
  }

  newFormEdit() {
    this.service
      .get(
        `$filter=id eq ${this.id}&$expand=agencia($select=id,nome),grupoAgencia,tipodeseguro,areaDeNegocio,atendente`
      )
      .subscribe(({ value }) => {
        this.titulo = "Editar Mapeamento de Atendente";
        this.agenciaSelecionada = value[0].agencia !== {} ? value[0].agencia.id : null;
        this.form = this.fb.group({
          id: this.id,
          agencias: value[0].agencia !== {} ? value[0].agencia.id : null,
          gruposAgencias: value[0].grupoAgencia !== {} ? value[0].grupoAgencia.id : null,
          tipodeseguro: this.form.value.tipoDeSeguro !== 0 ? value[0].tipoDeSeguro.id : null,
          areasDeNegocio: value[0].areaDeNegocio !== {} ? value[0].areaDeNegocio.id : null,
          atendente: value[0].atendente !== {} ? value[0].atendente.id : null,
          atendenteSelecionada: value[0].atendente !== {} ? value[0].atendente : null
        });
      });
  }

  getForm() {
    // this.atendenteSelecionada = null;

    this.titulo = "Novo Mapeamento de Atendente";
    this.form = this.fb.group({
      id: 0,
      agencias: null,
      atendente: null,
      gruposAgencias: null,
      tipodeseguro: null,
      areasDeNegocio: null,
      atendenteSelecionada: null
    });

    if (this.id) {
      this.newFormEdit();
    }
  }

  onClosePanel() {
    this.setResult({} as Result);
    // this.closePanel.emit(true);
    window.history.back();
  }

  search(event) {
    this.usuarioService
      .get(
        `$select=id,nome&$filter=(contains(nome,'${event.query}'))`
      )
      .subscribe((data) => {
        this.atendentes = data.value;
      });
  }

  setAtendente(atendente) {
    this.form.controls.atendente.setValue(atendente.id);
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
    console.log(this.form.value)
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.form.value.id || 0,
      Atendente_Id: this.form.value.atendenteSelecionada == null ? null : this.form.value.atendenteSelecionada.id,
      AreaDeNegocio_Id: this.form.value.areasDeNegocio == "null" ? null : this.form.value.areasDeNegocio,
      Agencia_Id: this.form.value.agencias == "null" ? null : this.form.value.agencias,
      GrupoAgencia_Id: this.form.value.gruposAgencias == "null" ? null : this.form.value.gruposAgencias,
      TipoDeSeguro_Id: this.form.value.tipodeseguro == "null" ? null : this.form.value.tipodeseguro,
    };


    this.service.save(post).subscribe(
      (response) => {
        this.setResult(response);
        if (response.successfully) {
          this.eventoConcluido.emit({ adicionar: this.id === 0 });
          if (this.titulo == 'Novo Mapeamento de Atendente') {
            this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: ' Mapeamento de Atendente Cadastrado' });
            this.getForm()
          }
          else {
            this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Mapeamento de Atendente Editado' });
            this.newFormEdit()
          }
        }
      },
      (error) => this.showError(error)
    );
  }
}
