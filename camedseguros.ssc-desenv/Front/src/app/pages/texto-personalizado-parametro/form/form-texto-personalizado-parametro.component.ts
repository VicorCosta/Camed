import { Component, Output, EventEmitter, Input, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { BaseComponent, AuthenticationService, Result } from "src/app/core";
import { FormBuilder } from "@angular/forms";

import _ from "underscore";

import { MessageService, SelectItem } from "primeng/api";
import { TipoDeProdutoService } from "../service";
import { TiposAgenciaService } from "../../tiposDeAgencia/service/tiposagencia.service";
import { TipoSeguroService } from "../../tiposdeseguro/service/tiposeguro.service";
import { VinculoService } from "../../vinculobnb/service";
import { AreaDeNegocioService } from "../../areadenegocio";
import { TextoPersonalizadoParametroService } from "../service/texto-personalizado-parametro.service";
import { FormGroup } from "@angular/forms";
import { FormControl } from "@angular/forms";
import { Observable } from "rxjs";

@Component({
  templateUrl: "form-texto-personalizado-parametro.component.html",
  styleUrls: ["./form-texto-personalizado-parametro.component.css"],
})
export class FormTextoPersonalizadoParametroComponent
  extends BaseComponent
  implements OnInit
{
  submitted = false;
  display = false;
  titulo: string;
  optionsTipoDeAgencia$: SelectItem[] = [];
  optionsTipoDeSeguro$: Observable<any>;
  optionsOperacaoDeFinanciamento$: SelectItem[] = [];
  optionsTipoDeProduto$: Observable<any>;
  optionsVinculoBNB$: SelectItem[] = [];
  optionsAreaDeNegocio$: SelectItem[] = [];

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() id: any;

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private textoPersonalizadoParametroService: TextoPersonalizadoParametroService,
    private tipoDeSeguroService: TipoSeguroService,
    private tipoDeProdutoService: TipoDeProdutoService,
    private messageService: MessageService
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe((params) => (this.id = params["id"]));
  }

  editorForm: FormGroup
  ngOnInit() {
    this.editorForm = new FormGroup({
      'editor': new FormControl(null)
    })

    this.execOptions();
    this.delay(500).then((_) => {
      this.newForm();
      this.appReady = true;
    });
  }

  newFormEditar() {
    this.titulo = "Editar Texto Personalizado";
    this.textoPersonalizadoParametroService
    .get(
      `$filter=id eq ${this.id}&$expand=tipodeseguro,tipodeproduto`
    )
      .subscribe(({ value }) => {
        this.form = this.fb.group({
          id: value[0].id,
          texto: value[0].texto,
          tipoDeSeguro_Id: [value[0].tipoDeSeguro_Id],
          tipoDeProduto_Id: [value[0].tipoDeProduto_Id],
          });
      });
  }

  newForm() {
    this.titulo = "Novo Texto Personalizado";
    this.form = this.fb.group({
      id: [0],
      texto: [null],
      tipoDeSeguro_Id: [null],
      tipoDeProduto_Id: [null],
    });

    if (this.id) {
      this.newFormEditar();
    }
  }

  onSubmit() {
    const post = {
      id: this.form.value.id,
      texto: this.form.value.texto,
      tipoDeSeguro_Id: this.form.value.tipoDeSeguro_Id,
      tipoDeProduto_Id: this.form.value.tipoDeProduto_Id,
    };
    this.loading = true;
    this.submitted = true;

    this.textoPersonalizadoParametroService.save(post).subscribe(
      (response) => {
        this.setResult(response);
        if (response.successfully) {
          this.eventoConcluido.emit({ adicionar: this.form.value.id === 0 });
          if (this.id) {
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Texto Personalizado Editado",
            });
            this.newForm();
          } else {
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Texto Personalizado Cadastrado",
            });
            this.newForm();
          }
        }
      },
      (error) => this.showError(error)
    );
  }

  //Layout
  onClosePanel() {
    this.setResult({} as Result);
    this.closePanel.emit(true);
    window.history.back();
  }

  public execOptions() {
    this.getAllTipoSeguro();
    this.getAllProdutos();

    return new Promise((resolve) => {
      setTimeout(resolve, 500);
    });
  }

  getAllTipoSeguro() {
    this.optionsTipoDeSeguro$ = this.tipoDeSeguroService.get("$select=id,nome");
  }


  getAllProdutos() {
    this.optionsTipoDeProduto$ = this.tipoDeProdutoService.get("$select=id,nome");
  }

}
