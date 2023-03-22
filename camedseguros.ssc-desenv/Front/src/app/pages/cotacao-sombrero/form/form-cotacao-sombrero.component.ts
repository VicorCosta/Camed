import {
  Component,
  Output,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
} from "@angular/core";
import _ from "underscore";
import Swal from "sweetalert2";

import { Router, ActivatedRoute } from "@angular/router";
import {
  BaseComponent,
  AuthenticationService,
  Grupo,
  Result,
} from "src/app/core";
import { FormBuilder } from "@angular/forms";
import { CotacaoSombreroService } from "../service";
import { MessageService, SelectItem } from "primeng/api";
import { MenuService } from "../../menu";
import { AcaoService } from "../../acao";

@Component({
  selector: "app-form-cotacao-sombrero",
  templateUrl: "form-cotacao-sombrero.component.html",
  styleUrls: ["./form-cotacao-sombrero.component.css"],
})
export class FormCotacaoSombreroComponent
  extends BaseComponent
  implements OnInit
{
  submitted = false;
  display = false;
  titulo: string;

  @Input() item = ""; // decorate the property with @Input()

  @Output() idCotacaoSombreroOutput = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() id: any;

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private service: CotacaoSombreroService,
    private messageService: MessageService
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe((params) => (this.id = params["id"]));
  }

  getFormEdit() {
    this.service.get(`$filter=id eq ${this.id}`).subscribe(({ value }) => {
      this.titulo = "Editar Ação";
      this.form = this.fb.group({
        id: this.id,
      });
    });
  }

  getForm() {
    this.titulo = "Nova Solicitação Sombrero";
    this.form = this.fb.group({
      id: 0,
      cepAreaDeRisco: [null],
      codigoProduto: "",
      codigoCultivo: "",
      nivelCobertura: "",
      areaTotal: "",
      TipoCotacao: "",
      unidadePesoCultivo: "",
      valorCusteio_Preco: "",
      tipoSubvencao: "",
    });

    if (this.id) {
      this.getFormEdit();
    }
  }

  ngOnInit(): void {
    this.getForm();
  }

  onClosePanel() {
    this.setResult({} as Result);
    window.history.back();
  }

  addItem(newItem: any) {
    this.idCotacaoSombreroOutput.emit(newItem);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const post = {
      id: this.f.id.value || 0,
      cepAreaDeRisco: this.f.cepAreaDeRisco.value,
      codigoProduto: this.f.codigoProduto.value,
      codigoCultivo: this.f.codigoCultivo.value,
      nivelCobertura: this.f.nivelCobertura.value,
      areaTotal: this.f.areaTotal.value,
      TipoCotacao: this.f.TipoCotacao.value,
      unidadePesoCultivo: this.f.unidadePesoCultivo.value,
      valorCusteio_Preco: this.f.valorCusteio_Preco.value,
      tipoSubvencao: this.f.tipoSubvencao.value,
    };
    this.service.save(post).subscribe(
      (response) => {
        this.setResult(response);
        if (response.successfully) {
          debugger;
          this.addItem(response.payload);
          if (this.titulo == "Nova Solicitação Sombrero") {
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Solicitação Sombrero Cadastrada",
            });
            this.getForm();
          } else {
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Solicitação Sombrero Editada",
            });
            this.getFormEdit();
          }
        }
      },
      (error) => this.showError(error)
    );
  }
}
