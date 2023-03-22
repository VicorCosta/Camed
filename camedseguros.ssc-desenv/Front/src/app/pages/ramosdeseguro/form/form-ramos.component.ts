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
  TipoDeProduto,
} from "src/app/core";
import { FormBuilder } from "@angular/forms";
import { RamosService } from "../service";
import _ from "underscore";

import { SituacaoService } from "../../situacoes";
import { MessageService } from "primeng/api";

@Component({
  selector: "app-form-ramos",
  templateUrl: "./form-ramos.component.html",
  styleUrls: ["./form-ramos.component.css"],
})
export class FormRamosComponent extends BaseComponent implements OnInit {
  submitted = false;
  display = false;
  post: TipoDeProduto;
  titulo: string;
  situacoes$: any;
  itensSituacao: any = [];
  itensSituacaoRenovacao: any = [];

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() ramo: any;
  @Input() id: any;

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private ramosService: RamosService,
    private situacaoService: SituacaoService,
    private messageService: MessageService
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe((params) => (this.id = params["id"]));
    if (this.id) {
      this.ramosService.getSituacoesByRamo(this.id).subscribe((response) => {
        this.filterItens(response);
      });
    }
  }

  newFormEditar() {
    this.titulo = "Editar Ramo de Seguro";
    this.ramosService.get(`$filter=id eq ${this.id}`).subscribe(({ value }) => {
      this.form = this.fb.group({
        id: value[0].id,
        nome: value[0].nome,
        ativo: value[0].ativo,
        situacao: this.itensSituacao ? this.itensSituacao : 0,
        slamaximo: value[0].slaMaximo,
        usoInterno: value[0].usoInterno,
        situacaorenovacao: this.itensSituacaoRenovacao
          ? this.itensSituacaoRenovacao
          : 0,
        DescricaoSasParaTipoDeProduto: value[0].descricaoSasParaTipoDeProduto,
      });
    });
  }

  newForm() {
    this.titulo = "Novo Ramo de Seguro";
    this.form = this.fb.group({
      id: [""],
      nome: [""],
      ativo: [false],
      situacao: 0,
      slamaximo: 0,
      usoInterno: [false],
      situacaorenovacao: 0,
      DescricaoSasParaTipoDeProduto: "",
    });

    if (this.id) {
      this.newFormEditar();
    }
  }

  ngOnInit(): void {
    this.submitted = false;
    this.loading = false;
    this.itensSituacao, (this.itensSituacaoRenovacao = []);

    this.getSituacoes();

    this.newForm();
  }

  getSituacoes() {
    this.situacoes$ = this.situacaoService.getAll(
      "$select=id,nome&$orderby=nome"
    );
  }

  filterItens(response) {
    this.itensSituacao = response.value[0].situacao?.id;
    this.itensSituacaoRenovacao = response.value[0].situacaoRenovacao?.id;
    this.newForm();
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.f.id.value || 0,
      Nome: this.f.nome.value,
      Ativo: this.f.ativo.value,
      Situacao: this.f.situacao.value,
      SlaMaximo: this.f.slamaximo.value,
      UsoInterno: this.f.usoInterno.value,
      SituacaoRenovacao: this.f.situacaorenovacao.value,
      DescricaoSasParaTipoDeProduto: this.f.DescricaoSasParaTipoDeProduto.value,
    };

    this.ramosService.save(post).subscribe(
      (response) => {
        this.setResult(response);
        if (response.successfully) {
          this.eventoConcluido.emit({ adicionar: this.form.value.id === 0 });
          if (this.id) {
            this.newFormEditar();
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Ramo de Seguro Editado",
            });
          } else {
            this.newForm();
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Ramo de Seguro Cadastrado",
            });
          }
        }
      },
      (error) => this.showError(error)
    );
  }

  saveSituacao(event) {
    if (event.target.checked) {
      this.itensSituacao.push(parseInt(event.target.value));
    } else {
      this.itensSituacao = _.without(
        this.itensSituacao,
        parseInt(event.target.value)
      );
    }
  }

  saveSituacaoRenovacao(event) {
    if (event.target.checked) {
      this.itensSituacaoRenovacao.push(parseInt(event.target.value));
    } else {
      this.itensSituacaoRenovacao = _.without(
        this.itensSituacaoRenovacao,
        parseInt(event.target.value)
      );
    }
  }

  checkSituacao(situacaoID) {
    return _.contains(this.itensSituacao, parseInt(situacaoID));
  }

  checkSituacaoRenovacao(situacaoRenovacaoID) {
    return _.contains(
      this.itensSituacaoRenovacao,
      parseInt(situacaoRenovacaoID)
    );
  }

  //Layout
  onClosePanel() {
    this.setResult({} as Result);
    // this.closePanel.emit(true);
    window.history.back();
  }
}
