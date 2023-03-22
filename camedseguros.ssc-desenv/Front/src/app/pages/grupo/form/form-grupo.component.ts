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
import { GrupoService } from "../service";
import { MessageService, SelectItem } from "primeng/api";
import { MenuService } from "../../menu";
import { AcaoService } from "../../acao";

@Component({
  selector: "app-form-group",
  templateUrl: "form-grupo.component.html",
  styleUrls: ["./form-grupo.component.css"],
})
export class FormGrupoComponent extends BaseComponent implements OnInit {
  submitted = false;
  display = false;
  displaySubGrupo = false;
  post: Grupo;
  titulo: string;
  itensMenu$: SelectItem[] = [];
  itensSubgrupos$: SelectItem[] = [];
  isEditeGrupo = false;
  acoesMenu: any;
  acoesDoMenu: any = [];

  AcaoDoMenu: any = [];
  AcoesPorMenu: Array<any> = [];
  menuEscolhido: any = [];

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() grupo: any;
  @Input() id: any;

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private grupoService: GrupoService,
    private menuService: MenuService,
    private messageService: MessageService,
    private acoesService: AcaoService
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe((params) => (this.id = params["id"]));
  }

  ngOnInit() {
    this.menuService
      .get("$select=id,label&$orderby=label", false)
      .subscribe((data) => {
        data.forEach((element) => {
          this.itensMenu$.push({
            label: element.Label,
            value: element.Id,
          });
        });
      });

    this.grupoService
      .get("$select=id,nome&$orderby=nome", false)
      .subscribe((data) => {
        data.forEach((element) => {
          this.itensSubgrupos$.push({
            label: element.Nome,
            value: element.Id,
          });
        });
      });

    this.newForm();
  }

  newFormEditar() {
    this.titulo = "Editar Grupo";
    this.grupoService
      .get(`$filter=id eq ${this.id}&$expand=subgrupos,menus`)
      .subscribe(({ value }) => {
        value[0].menus !== null &&
          value[0].menus.map((menu) => this.menuSelect([menu.menu_Id]));

        this.form = this.fb.group({
          id: value[0].id,
          nome: value[0].nome,
          menus:
            value[0].menus !== null
              ? [_.pluck(value[0].menus, "menu_Id")]
              : [null],
          subgrupos:
            value[0].subgrupos !== null
              ? [_.pluck(value[0].subGrupos, "subgrupo_Id")]
              : [null],
          ativo: value[0].ativo,
          atribuirAtendente: value[0].atribuirAtendente,
          sempreVisualizarObservacao: value[0].sempreVisualizarObservacao,
          atribuirOperador: value[0].atribuirOperador,
          cancelarSolicitacao: value[0].cancelarSolicitacao,
        });
      });
  }

  newForm() {
    this.titulo = "Novo Grupo";
    this.form = this.fb.group({
      id: [0],
      nome: [null],
      menus: [null],
      subgrupos: [null],
      ativo: [false],
      atribuirAtendente: [false],
      sempreVisualizarObservacao: [false],
      atribuirOperador: [false],
      cancelarSolicitacao: [false],
    });
    this.alertError = null;
    if (this.id) {
      this.newFormEditar();
    }
  }

  getAcoesMenu(id) {
    this.AcoesPorMenu = [];
    this.acoesService.getAll().subscribe((acao) => {
      this.menuService
        .get(`$filter=id eq ${id}&$expand=menuAcao($expand=acao)`)
        .subscribe((menu) => {
          if (menu.value[0].menuAcao.length > 0) {
            //Percorrer todas as ações e todas os menuAcao que ficam em menu e verifica se a acao_id dentro de menuAcao é igual ao id de ação se sim adiciona  ao json de acao um chek true
            this.AcoesPorMenu.pop();
            acao.value.filter((a) =>
              menu.value[0].menuAcao.filter((ma) =>
                ma.acao_id === a.id
                  ? this.AcoesPorMenu.push({ ...a, check: true })
                  : this.AcoesPorMenu.push({ ...a, check: false })
              )
            );
            this.AcaoDoMenu.push(this.AcoesPorMenu);
          } else {
            let menuAcao = acao.value.filter((a) => ({ ...a, check: false }));
            this.AcaoDoMenu.push(menuAcao);
          }
        });
    });
  }

  /* Menu(s) selecionado(s) pelo usuário */
  menuSelect(e) {
    let menu = e;
    let arrayTemp = [];
    this.getAcoesMenu(e[e.length - 1]);
    menu.forEach((element, index) => {
      this.menuService
        .get(`$select=id,label&$filter=id+eq+${element}`, true)
        .subscribe((data) => {
          data.value.forEach((element) => {
            arrayTemp.push({
              label: element.label,
              value: element.id,
              index: index,
            });
          });
        });
    });
    this.menuEscolhido = arrayTemp;
  }

  onClosePanel() {
    this.setResult({} as Result);
    window.history.back();
    this.newForm();
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    this.grupoService.save(this.form.value).subscribe(
      (response) => {
        this.setResult(response);
        if (response.successfully) {
          this.eventoConcluido.emit({ adicionar: this.form.value.id === 0 });
          if(this.id){
            this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Grupo Editada'});
            this.newForm();
          }else{
            this.messageService.add({severity:'success', summary: 'Sucesso', detail:'Grupo Cadastrado'});
            this.newForm();
          }
        }
      },
      (error) => this.showError(error)
    );
  }
}
