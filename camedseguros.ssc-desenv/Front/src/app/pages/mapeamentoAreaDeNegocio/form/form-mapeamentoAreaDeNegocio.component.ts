import { Component, Output, EventEmitter, Input, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { BaseComponent, AuthenticationService, Result } from "src/app/core";
import { FormBuilder } from "@angular/forms";

import _, { isNumber } from "underscore";

import { MessageService, SelectItem } from "primeng/api";
import { TipoDeProdutoService } from "../service";
import { TiposAgenciaService } from "./../../tiposDeAgencia/service/tiposagencia.service";
import { TipoSeguroService } from "./../../tiposdeseguro/service/tiposeguro.service";
import { VinculoService } from "../../vinculobnb/service";
import { AreaDeNegocioService } from "../../areadenegocio";
import { MapeamentoAreaDeNegocioService } from "./../service/mapeamentoAreaDeNegocio.service";
import { Observable } from "rxjs";

@Component({
  templateUrl: "form-mapeamentoAreaDeNegocio.component.html",
  styleUrls: ["./form-mapeamentoAreaDeNegocio.component.css"],
})
export class FormMapeamentoAreaDeNegocioComponent
  extends BaseComponent
  implements OnInit
{
  submitted = false;
  display = false;
  titulo: string;
  optionsTipoDeAgencia$: Observable<any>;
  optionsTipoDeSeguro$: Observable<any>;
  optionsOperacaoDeFinanciamento$: SelectItem[] = [];
  optionsTipoDeProduto$: Observable<any>;
  optionsVinculoBNB$: Observable<any>;
  optionsAreaDeNegocio$: Observable<any>;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() id: any;

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private mapeamentoAreaDeNegocioService: MapeamentoAreaDeNegocioService,
    private tipoDeAgenciaService: TiposAgenciaService,
    private tipoDeSeguroService: TipoSeguroService,
    private tipoDeProdutoService: TipoDeProdutoService,
    private vinculoBNBService: VinculoService,
    private areaDeNegocioService: AreaDeNegocioService,
    private messageService: MessageService
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe((params) => (this.id = params["id"]));
  }

  ngOnInit() {
    this.execOptions();
    this.delay(500).then((_) => {
      this.newForm();
      this.appReady = true;
    });
  }

  newFormEditar() {
    this.titulo = "Editar Mapeamento Área de Negócio";
    this.mapeamentoAreaDeNegocioService
      .getMapeamentoSelecionado(this.id)
      .subscribe(({ value }) => {
        // console.log(value[0], this.optionsOperacaoDeFinanciamento$)
        this.form = this.fb.group({
          id: value[0].id,
          tipoDeAgencia_Id: value[0].tipoDeAgencia.id,
          tipoDeSeguro_Id: value[0].tipoDeSeguro.id,
          operacaoDeFinanciamento:
            value[0].operacaoDeFinanciamento >= 0
              ? [value[0].operacaoDeFinanciamento]
              : 0,
          tipoDeProduto_Id: value[0].tipoDeProduto.id,
          vinculoBNB_Id: value[0].vinculoBNB?.id
            ? value[0].vinculoBNB.id
            : null,
          areaDeNegocio_Id: value[0].areaDeNegocio.id,
        });
      });
  }

  newForm() {
    this.titulo = "Novo Mapeamento Área de Negócio";
    this.form = this.fb.group({
      id: [0],
      tipoDeAgencia_Id: [null],
      tipoDeSeguro_Id: [null],
      operacaoDeFinanciamento: [null],
      tipoDeProduto_Id: [null],
      vinculoBNB_Id: [null],
      areaDeNegocio_Id: [null],
    });

    if (this.id) {
      this.newFormEditar();
    }
  }



  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // console.log(this.form.value.operacaoDeFinanciamento)
    const post = {
      id: this.form.value.id,
      tipoDeAgencia_Id: this.form.value.tipoDeAgencia_Id !== null ? this.form.value.tipoDeAgencia_Id : null,
      tipoDeSeguro_Id: this.form.value.tipoDeSeguro_Id !== null ? this.form.value.tipoDeSeguro_Id : null,
      operacaoDeFinanciamento: isNumber(this.form.value.operacaoDeFinanciamento) ? this.form.value.operacaoDeFinanciamento : null,
      tipoDeProduto_Id: this.form.value.tipoDeProduto_Id !== null ? this.form.value.tipoDeProduto_Id : null,
      vinculoBNB_Id: this.form.value.vinculoBNB_Id !== null ? this.form.value.vinculoBNB_Id : null,
      areaDeNegocio_Id: this.form.value.areaDeNegocio_Id !== null ? this.form.value.areaDeNegocio_Id : null,
    };
    // console.log(post.operacaoDeFinanciamento)
    
    this.mapeamentoAreaDeNegocioService.save(post).subscribe(
      (response) => {
        this.setResult(response);
        if (response.successfully) {
          this.eventoConcluido.emit({ adicionar: this.form.value.id === 0 });
          if (this.id) {
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Mapeamento Área de Negócio Editada",
            });
            this.newForm();
          } else {
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Mapeamento Área de Negócio Cadastrado",
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
    //this.closePanel.emit(true);
    window.history.back();
  }

  public execOptions() {
    this.getAllTipoAgencia();
    this.getAllTipoSeguro();
    this.getAllOperacaoFinanc();
    this.getAllProdutos();
    this.getAllVinculosBNB();
    this.getAllAreaDeNegocio();

    return new Promise((resolve) => {
      setTimeout(resolve, 500);
    });
  }

  getAllTipoAgencia() {
    this.optionsTipoDeAgencia$ = this.tipoDeAgenciaService.get("$select=id,nome")
  }

  getAllTipoSeguro() {
    this.optionsTipoDeSeguro$ = this.tipoDeSeguroService.get("$select=id,nome");
  }

  getAllOperacaoFinanc() {
    let temp = [
      {
        label: "",
        value: null,
      },
      {
        label: "Sim",
        value: 1,
      },
      {
        label: "Não",
        value: 0,
      },
    ];

    temp.forEach((element) => {
      this.optionsOperacaoDeFinanciamento$.push({
        label: element.label,
        value: element.value,
      });
    });

  }

  getAllProdutos() {
    this.optionsTipoDeProduto$ = this.tipoDeProdutoService.get("$select=id,nome");
  }

  getAllVinculosBNB() {
    this.optionsVinculoBNB$ = this.vinculoBNBService.get("$select=id,nome");
    // this.optionsVinculoBNB$.push({
    //   label: "Selecione",
    //   value: null,
    // });
    // this.vinculoBNBService.getAll("").subscribe((data) => {
    //   data.value.forEach((element) => {
    //     this.optionsVinculoBNB$.push({
    //       label: element.nome,
    //       value: element.id,
    //     });
    //   });
    // });
  }

  getAllAreaDeNegocio() {
   this.optionsAreaDeNegocio$ = this.areaDeNegocioService.get("$select=id,nome");
  }
}
