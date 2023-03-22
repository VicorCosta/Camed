//#region Imports

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
  Frames,
} from "src/app/core";
import { FormBuilder } from "@angular/forms";
import _ from "underscore";

import { TipoDeDocumentoService } from "../service";
import { RamosService } from "../../ramosdeseguro";
import { MessageService, SelectItem } from "primeng/api";
import { TipoMorteService } from "../service";

//#endregion

@Component({
  selector: "app-form-tipo-de-documento",
  templateUrl: "./form-tipoDeDocumento.component.html",
  styleUrls: ["./form-tipoDeDocumento.component.css"],
})
export class FormTipoDeDocumentoComponent
  extends BaseComponent
  implements OnInit
{
  //#region Variaveis

  submitted = false;
  display = false;
  post: Frames;
  titulo: string;
  ramosDeSeguro$: any;
  itensSelecionados: any = [];
  itensTipoMorte$: SelectItem[] = [];
  isEditeTipoDeDoc: boolean = false;
  tipoMorteId: number;
  itensSubgrupos$: SelectItem[] = [];

  @Input() id: any;
  @Input() frame: any;
  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();

  //#endregion

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private messageService: MessageService,
    private ramoDeSeguroService: RamosService,
    private tipoMorteService: TipoMorteService,
    private tipoDeDocumentoService: TipoDeDocumentoService
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe((params) => (this.id = params["id"]));
  }

  ngOnInit(): void {
    this.loading = false;
    this.submitted = false;
    this.getRamosDeSeguro();
    this.getAllTipoMorte();
    if (this.id) {
      this.editeForm();
    } else {
      this.newForm();
    }
  }

  newForm() {
    this.titulo = "Novo Tipo de Documento";
    this.form = this.fb.group({
      id: [""],
      nome: [null],
      ramosDeSeguro: [null],
      ordem: [null],
      tipoMorte_Id: [null],
      ativo: [false],
      obrigatorio: [false],
    });
  }

  editeForm() {
    this.titulo = "Editar Tipo de Documento";
    this.tipoDeDocumentoService
      .getTiposDeProdutoByDocumento(this.id)
      .subscribe(({ value }) => {
        // remover repetições para exebição das mortes
        let tipoMortePorProduto = [];
        value[0].tiposDeProdutoMorte.forEach((item) => {
          var duplicated =
            tipoMortePorProduto.findIndex((redItem) => {
              return item.a == redItem.a;
            }) > -1;

          if (!duplicated) {
            tipoMortePorProduto.push(item);
          }
        });

        this.form = this.fb.group({
          id: value[0].id,
          nome: value[0].nome,
          ramosDeSeguro: value[0].tiposDeProdutoMorte
            ? [_.pluck(value[0].tiposDeProdutoMorte, "tipoDeProduto_Id")]
            : [null],
          ordem:
            value[0].tiposDeProdutoMorte.length > 0
              ? value[0].tiposDeProdutoMorte[0].ordem
              : [null],
          tipoMorte_Id:
            value[0].tiposDeProdutoMorte.length > 0
              ? [_.pluck(tipoMortePorProduto, "tipoMorte_Id")]
              : [null],
          ativo:
            value[0].tiposDeProdutoMorte.length > 0
              ? [value[0].tiposDeProdutoMorte[0].ativo]
              : [false],
          obrigatorio:
            value[0].tiposDeProdutoMorte.length > 0
              ? [value[0].tiposDeProdutoMorte[0].obrigatorio]
              : [false],
        });
      });
  }

  //Tipos de produtos
  getRamosDeSeguro() {
    this.ramosDeSeguro$ = this.ramoDeSeguroService.getAll(
      "$select=id,nome&$orderby=nome"
    );
  }

  getAllTipoMorte() {
    this.tipoMorteService.get().subscribe((data) => {
      data.value.forEach((element) => {
        this.itensTipoMorte$.push({
          label: element.descricao,
          value: element.id,
        });
      });
    });
  }

  //Send Data
  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.f.id.value || 0,
      nome: this.f.nome.value,
      ramosDeSeguro:
        Array.isArray(this.f.ramosDeSeguro.value) &&
          this.f.ramosDeSeguro.value.length
          ? this.f.ramosDeSeguro.value
          : null,
      ordem: this.f.ordem.value,
      tipoMorte_Id:
        Array.isArray(this.f.tipoMorte_Id.value) &&
        this.f.tipoMorte_Id.value.length
          ? this.f.tipoMorte_Id.value[0]
          : null,
      ativo: this.f.ativo.value,
      obrigatorio: this.f.obrigatorio.value,
    };

    this.tipoDeDocumentoService.save(post).subscribe(
      (response) => {
        this.setResult(response);
        if (response.successfully) {
          this.eventoConcluido.emit({ adicionar: this.form.value.id === 0 });
          if (this.id) {
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Documento Editado",
            });
            this.editeForm();
          } else {
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Documento Cadastrado",
            });
            this.newForm();
          }
        }
      },
      (error) => {this.showError(error); console.log(error)}
    );
  }

  //Checkbox
  saveCheckBox(event) {
    if (event.target.checked) {
      this.itensSelecionados.push(parseInt(event.target.value));
    } else {
      this.itensSelecionados = _.without(
        this.itensSelecionados,
        parseInt(event.target.value)
      );
    }
  }

  checkAcoes(acoesID) {
    return _.contains(this.itensSelecionados, parseInt(acoesID));
  }

  //Layout
  onClosePanel() {
    this.setResult({} as Result);
    window.history.back();
  }

  onSelected(value: number): void {
    this.tipoMorteId = value;
  }
}
