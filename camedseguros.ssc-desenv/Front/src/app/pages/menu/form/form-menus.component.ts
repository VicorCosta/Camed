import {
  Component,
  Output,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
} from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { BaseComponent, AuthenticationService, Result } from "src/app/core";
import { FormBuilder } from "@angular/forms";
import { MenuService } from "../service";
import _ from "underscore";
import { MessageService } from "primeng/api";
import { AcaoService } from "../../acao";

@Component({
  selector: "app-form-menus",
  templateUrl: "./form-menus.component.html",
  styleUrls: ["./form-menus.component.css"],
})
export class FormMenusComponent extends BaseComponent implements OnInit {
  submitted = false;
  display;
  titulo: string;

  acoesMenu$: any;
  menus$: any;
  menuSuperior$: any;
  itensSelecionados: any = [];
  iconSelected: any = [];
  textoInvalido = false;

  editarForm = false;

  checkPai = false;
  checkFilho = false;

  exibirMenuFilho = false;
  exibirMenuPai = false;

  iconList: any = [
    { label: "Adicionar", icon: "fa-plus" },
    { label: "Download", icon: "fa-download" },
    { label: "Arquivo", icon: "fa-archive" },
    { label: "Envelope", icon: "fa-envelope-o" },
    { label: "Pesquisar", icon: "fa-search-plus" },
    { label: "Relógio", icon: "fa-clock-o" },
    { label: "Livro", icon: "fa-book" },
    { label: "Camera", icon: "fa-camera" },
    { label: "Imagem", icon: "fa-image" },
    { label: "Editar", icon: "fa-edit" },
    { label: "Telefone", icon: "fa-phone" },
    { label: "Certificado", icon: "fa-certificate" },
    { label: "Tabela", icon: "fa-table" },
    { label: "Enviar", icon: "fa-send-o" },
    { label: "Gráfico", icon: "fa-bar-chart-o" },
    { label: "Laptop", icon: "fa-laptop" },
    { label: "Sitemap", icon: "fa-sitemap" },
    { label: "Tabela", icon: "fa-th-large" },
    { label: "Inbox", icon: "fa-inbox" },
    { label: "Vizualizar", icon: "fa-eye" },
    { label: "Solicitação", icon: "far fa-file" },
  ];

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() item: any;
  @Input() id: any;

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private acoesService: AcaoService,
    private service: MenuService,
    private messageService: MessageService
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe((params) => (this.id = params["id"]));
  }

  ngOnInit() {
    this.getMenus();
    this.getMenuSuperior();
    this.getAcoesMenu();

    this.newform();
    this.submitted = false;
    this.loading = false;
  }

  newFormEdit() {
    this.service
      .get(`$filter=id eq ${this.id}&$expand=superior,menuAcao`)
      .subscribe(({ value }) => {
        const acoes = [];
        value.map((e) =>
          e.menuAcao.map((menuAcao) => acoes.push(menuAcao.acao_id))
        );

        this.editarForm = true;
        this.titulo = "Editar Menu";
        if (value[0].superior == null) {
          this.exibirMenuPai = true;
          this.form.setValue({
            id: value[0].id,
            label: value[0].label,
            icone: value[0].icone,
            rota: null,
            menuSuperior: null,
            ajuda: null,
            acoes: [],
          });
        } else {
          this.exibirMenuFilho = true;
          this.form.setValue({
            id: value[0].id,
            label: value[0].label,
            rota: value[0].rota,
            menuSuperior: value[0].superior.id,
            ajuda: value[0].ajudatexto,
            icone: value[0].icone,
            acoes: acoes,
          });
        }
      });
  }

  newform() {
    this.exibirMenuPai = false;
    this.exibirMenuFilho = false;
    this.editarForm = false;
    this.titulo = "Novo Menu";
    this.form = this.fb.group({
      id: [0],
      label: [null],
      rota: [null],
      icone: [null],
      menuSuperior: "",
      ajuda: [null],
      acoes: [null],
    });

    if (this.id) {
      this.newFormEdit();
    }
  }

  getAcoesMenu() {
    this.acoesMenu$ = this.acoesService.getAll();
  }

  getMenus() {
    this.menus$ = this.service.getAll();
  }

  getMenuSuperior() {
    this.menuSuperior$ = this.service.get(
      "?$expand=Superior&$filter=Superior%20eq%20null"
    );
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    // verificar se icone foi adicionado
    Array.isArray(this.iconSelected) && !this.iconSelected.length
      ? null
      : (this.form.value.icone = this.iconSelected[0].icon);
    const newValue = this.form.controls.id.value === 0;

    if (
      this.form.value.ajuda == null ||
      this.textoValido(this.form.value.ajuda)
    )
      this.service.save(this.form.value).subscribe(
        (response) => {
          this.setResult(response);
          if (response.successfully) {
            if (this.titulo === "Editar Menu") {
              this.newFormEdit();
              this.messageService.add({
                severity: "success",
                summary: "Sucesso",
                detail: "Menu Editado",
              });
            } else {
              this.newform();
              this.messageService.add({
                severity: "success",
                summary: "Sucesso",
                detail: "Menu Cadastrado",
              });
            }
            this.eventoConcluido.emit({ adicionar: newValue });
          }
        },
        (error) => this.showError(error)
      );
  }

  onClosePanel() {
    this.setResult({} as Result);
    // this.closePanel.emit(true);
    window.history.back();
  }

  checkedMenuPai(evento) {
    let marcado = evento.checked;
    if (marcado) {
      this.exibirMenuPai = true;
      this.exibirMenuFilho = false;
    } else {
      this.exibirMenuPai = false;
    }
  }

  checkedMenuFilho(evento) {
    let marcado = evento.checked;
    if (marcado) {
      this.exibirMenuFilho = true;
      this.exibirMenuPai = false;
    } else {
      this.exibirMenuFilho = false;
    }
  }

  textoValido(text) {
    let tamanhoTexto = text.replace(/<[^>]*>/g, "")?.length;
    if (tamanhoTexto > 300) {
      this.loading = false;
      this.textoInvalido = true;
      return false;
    }
    this.textoInvalido = false;
    return true;
  }
}
