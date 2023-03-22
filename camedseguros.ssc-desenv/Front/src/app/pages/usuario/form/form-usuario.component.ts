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

import { UsuarioService } from "../service";
import { GrupoAgenciaService } from "../../grupoagencias";
import { GrupoService } from "../../grupo";
import { EmpresaService } from "../../empresas";
import { AreaDeNegocioService } from "../../areadenegocio";

@Component({
  selector: "app-form-usuario",
  templateUrl: "./form-usuario.component.html",
  styleUrls: ["./form-usuario.component.css"],
})
export class FormUsuarioComponent extends BaseComponent implements OnInit {
  displayBasic: boolean = false;

  submitted = false;
  display = false;
  titulo: string;
  agenciaSelecionada: any;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() usuario: any;
  @Input() id: any;

  empresas$: Observable<any>;

  agencias: Observable<any>;
  grupos: SelectItem[] = [];
  areasDeNegocio: SelectItem[] = [];
  gruposAgencias: SelectItem[] = [];

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private service: UsuarioService,
    private grupoAgenciaService: GrupoAgenciaService,
    private grupoService: GrupoService,
    private empresaService: EmpresaService,
    private agenciaService: AgenciaService,
    private areaDeNegocioService: AreaDeNegocioService,
    private messageService: MessageService
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  ngOnInit(): void {
    this.empresas$ = this.empresaService.get("$select=id,nome&$orderby=nome");

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

  newFormEdit(){
    this.service.get(`$filter=id eq ${this.id}&$expand=agencia,gruposAgencias,areasDeNegocio,grupos,empresa`).subscribe(({value})=>{
      this.titulo = 'Editar Usuário';
      this.agenciaSelecionada = (value[0].agencia !== null ? value[0].agencia : null);
      this.form = this.fb.group({
        id: this.id,
        matricula: value[0].matricula,
        login: value[0].login,
        nome: value[0].nome,
        cpf: value[0].cpf,
        email: value[0].email,
        empresa: value[0].empresa.id,
        agencia: (value[0].agencia !== null ? value[0].agencia.id : null),
        ativo: value[0].ativo,
        enviarSLA: value[0].enviarSLA,
        ehCalculista: value[0].ehCalculista,
        gruposAgencias: (value[0].gruposAgencias.length > 0  ?[ _.pluck(value[0].gruposAgencias, 'grupoAgencia_Id')] : null),
        areasDeNegocio: (value[0].areasDeNegocio.length > 0 ? [_.pluck(value[0].areasDeNegocio, 'areaDeNegocio_Id') ]: null),
        grupos: (value[0].grupos.length > 0  ? [_.pluck(value[0].grupos, 'grupo_Id')] : null)
      });
    })
  }

  getForm(){
    this.agenciaSelecionada = null;
    this.alertError = '';

    this.titulo = 'Novo Usuário';
    this.form = this.fb.group({
        id: [0],
        matricula: [null],
        login: [null],
        nome: [null],
        cpf: [null],
        email: "",
        empresa: "",
        agencia: [null],
        ativo: [false],
        enviarSLA: [false],
        gruposAgencias: [null],
        areasDeNegocio: [null],
        grupos: [],
      });

      if (this.id) {
        this.newFormEdit()
      }
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

  removeAgencia() {
    this.form.controls.agencia.setValue(null);
    this.agenciaSelecionada = "";
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
    const cpfSemMascara = this.form.value.cpf?.replace(/\D/g, '');
    this.form.value.cpf = cpfSemMascara;
    this.submitted = true;
    this.loading = true;
    const newValue = this.form.controls.id.value === 0;
    if (this.form.value.grupos == null) this.form.value.grupos = "";
    if (this.form.value.areasDeNegocio == null) this.form.value.areasDeNegocio = "";
    if (this.form.value.gruposAgencias == null) this.form.value.gruposAgencias = "";


    this.service.save(this.form.value).subscribe(response => {
      this.setResult(response);
      if (response.successfully && response.message != "reativarExcluido") {
        if(this.titulo === "Editar Usuário" ){
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Usuário Editado'})
          this.newFormEdit()
        }else{
          this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Usuário Cadastrado'});
          this.getForm()
        }

        this.alertError = ''
        this.eventoConcluido.emit({ adicionar: newValue });
      } else if (response.successfully && response.message == "reativarExcluido") {
        this.displayBasic = true;
      }
    }, (error) => this.showError(error));
  }

  onSubmitRestoreUser() {
    this.form.value.cpf = '00000000001';
    this.submitted = true;
    this.loading = true;
    this.form.value.id = 1;

    this.displayBasic = false;
    this.service.save(this.form.value).subscribe(response => {
      this.setResult(response);
      if(response.successfully) {
        this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Usuário Restaurado' })
      }

      this.alertError = '';
    }, (error) => this.showError(error));

  }
}
