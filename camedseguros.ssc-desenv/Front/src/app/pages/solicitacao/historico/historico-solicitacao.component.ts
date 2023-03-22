import { isNumber } from 'underscore';
import {
  Component,
  Output,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
} from "@angular/core";
import { Observable, from } from "rxjs";

import { Router, ActivatedRoute } from "@angular/router";
import {
  BaseComponent,
  AuthenticationService,
  Solicitacao,
  Result,
} from "src/app/core";
import { Form, FormBuilder, FormControl } from "@angular/forms";
import { AcompanhamentoService, SolicitacaoService } from "../service";
import { environment } from "src/environments/environment";
import { MenuItem, SelectItem } from "primeng/api";
import { AgenciaService } from "../../../core/services/agencia.service";
import { TipoSeguroService } from "../../tiposdeseguro/service/tiposeguro.service";
import { SeguradoService } from "../../../core/services/segurado.service";
import { FuncionarioService } from "../../../core/services/funcionario.service";
import { SegmentoService } from "../../../core/services/segmento.service";
import { CanalDeDistribuicaoService } from "../../../core/services/canalDeDistribuicao.service";
import { TipoProdutoService } from "../../../core/services/tipodeproduto.service";
import { VinculoBNBService } from "../../../core/services/vinculoBNB.service";
import { SolicitanteService } from "../../../core/services/solicitante.service";
import * as moment from "moment";
import { SituacaoService } from "../../../core/services/situacao.service";
import { CotacaoSombreroService } from "../../cotacao-sombrero";
import { param } from "jquery";
import { UsuarioGrupo } from "../../auditoria/service";
import { GrupoService } from "../../grupo/service";

export enum PageNames {
  Solicitante,
  Solicitacao,
  Segurado,
  Indicacoes,
  Anexos,
}

@Component({
  selector: "app-historico-solicitacao",
  styleUrls: ["./historico-solicitacao.component.css"],
  templateUrl: "./historico-solicitacao.component.html",
})
export class HistoricoSolicitacaoComponent
  extends BaseComponent
  implements OnInit {
  submitted = false;
  display = false;
  post: Solicitacao;
  titulo: string;
  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() solicitacao: any;
  @Input() idCotacaoSombreroInput: any;
  @Input() btnAcompanhamento: any;
  @Input() acompanhamento: any;


  idCotacaoSombreroOutput: any;
  url: string;
  Origens: SelectItem[];
  Agencias: any[];
  TiposDeSeguro: any[];
  Segmentos: any[];
  Produtores: any[];
  CanaisDeDistribuicao: any[];
  TiposDeProduto: any[];
  VinculosBNB: any[];
  uploadedFiles: any[];
  PageNames = PageNames;
  nomeIndicado: string;
  telefoneIndicado: string;
  indicados: any[] = new Array<any>();
  AgenciasContas: any[];
  anexos: any[];
  viewOnly: boolean = false;
  varejo: any;
  ignoraVarejo: boolean = false;
  soLeituraSolicitante: boolean = false;
  seguradoPreenchido: boolean = false;
  acompanhamentos: any = [];
  situacaoAtualId: any;
  situacoes: any = [];
  usuarioGrupos: any[];
  solicitacaoId: any;

  activeIndex: number = 0;
  EtapasSolicitacao: MenuItem[];
  LoggedUser = this.authenticationService.getLoggedUser();
  apiUrl = environment.api_url;

  tipoSeguroSelecionado: any;

  anexosSolicitacao: any;
  gruposId: number;
  loggedUser: import("c:/Projeto CAMEDSegurosNovo/camedseguros.ssc/Front/src/app/core/index").User;
  editTipoProduto: boolean;
  editSolicitacaoExistente: boolean;
  items = ['Situações'];
  statusFluxo: boolean;


  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private tipoSeguroService: TipoSeguroService,
    private agenciaService: AgenciaService,
    private segmentoService: SegmentoService,
    private funcionarioService: FuncionarioService,
    private canalDeDistribuicaoService: CanalDeDistribuicaoService,
    private tipoProdutoService: TipoProdutoService,
    private seguradoService: SeguradoService,
    private vinculoBNBService: VinculoBNBService,
    private solicitanteService: SolicitanteService,
    private solicitacaoService: SolicitacaoService,
    private acompanhamentoService: AcompanhamentoService,
    private situacaoService: SituacaoService,
    private service: CotacaoSombreroService,
    private grupoService: GrupoService,
    private usuarioGrupoService: UsuarioGrupo,



  ) {
    super(authenticationService, fb, route, router);
    this.solicitacao = this.router.getCurrentNavigation().extras.state;
  }

  receiveMessage($event) {
    this.idCotacaoSombreroOutput = $event;
  }

  ngOnInit() {

    this.titulo = "Histórico da Solicitação";

    this.usuarioGrupoService.getAll().subscribe((data) => {
      this.loggedUser = this.authenticationService.getLoggedUser();
      for (let i = 0; i < data.value.length; i++) {
        let usuarioId = data.value[i].usuario_Id
        let grupoId = data.value[i].grupo_Id
        if (usuarioId == this.loggedUser.id) {
          if (grupoId != 1) {
            document.getElementById("btnAcompanhamento").style.display = "none";
          }
          break;
        }
      }
    });

    if (this.solicitacao) {
      this.solicitacao.origem = this.solicitacao.origem == 0 || this.solicitacao.origem == "telefone" ? "Telefone" : "TEM QUE CONDICIONAR";
      this.solicitacao.mercado = this.solicitacao.mercado == 0 ? "Não" : "TEM QUE CONDICIONAR";
      this.solicitacao.segurado.telefoneAdicional = this.solicitacao.segurado.telefoneAdicional == "" ? "Não há outro" : this.solicitacao.segurado.telefoneAdicional;
      this.solicitacao.indicacoes = this.solicitacao.indicacoes.length == 0 ? "Sem indicações" : this.solicitacao.indicacoes;
      this.acompanhamentos
      this.acompanhamentoService.get(`$select=Id,ordem,solicitacao_Id,dataEHora,observacao,situacao_Id&$expand=atendente($select=nome)&$filter=solicitacao_Id eq ${this.solicitacao.id}`).subscribe((data) => {
        this.acompanhamentos = data.value;
        this.situacaoAtualId = data.value[data.value.length - 1].situacao_Id;
        this.solicitacaoId = this.solicitacao.id;
        if (this.acompanhamentos.length == 1) {
          this.editTipoProduto = true;
        } else if (this.acompanhamentos.length > 1) {
          this.editTipoProduto = false;
        }
        this.acompanhamentos.map(acomp => {
          this.situacaoService.get(`$select=Id,nome,eFimFluxo&$filter=Id eq ${acomp.situacao_Id}`).subscribe(content => {
            acomp.situacao_Id = { id: content.value[0].id, nome: content.value[0].nome, eFimFluxo: content.value[0].eFimFluxo };
            if (acomp.situacao_Id.eFimFluxo == true) {
              document.getElementById("btnAcompanhamento").style.visibility = "hidden";
            }
          });
        });
      });
    }
  }

  onClosePanel() {
    window.history.back();
    this.showError("");
    this.alertError = null;
    this.setResult({} as Result);
  }

  onSubmit() { }
}
