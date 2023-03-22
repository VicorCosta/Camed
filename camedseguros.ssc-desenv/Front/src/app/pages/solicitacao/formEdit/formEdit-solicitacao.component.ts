import {
  Component,
  Output,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
} from "@angular/core";
import { Observable, from } from "rxjs";
import { OnEventArgs } from "angular-slickgrid";
import {DialogModule} from 'primeng/dialog';

import { Router, ActivatedRoute } from "@angular/router";
import {
  BaseComponent,
  AuthenticationService,
  Solicitacao,
  Result,
} from "src/app/core";
import { Form, FormBuilder, FormControl } from "@angular/forms";
import { SolicitacaoService } from "../service";
import { environment } from "src/environments/environment";
import { MenuItem, MessageService, SelectItem } from "primeng/api";
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
import Swal from "sweetalert2";
import { CotacaoSombreroService } from "../../cotacao-sombrero";

export enum PageNames {
  Solicitante,
  Solicitacao,
  Segurado,
  Indicacoes,
  Anexos,
}

@Component({
  selector: "app-formEdit-solicitacao",
  styleUrls: ["./formEdit-solicitacao.component.css"],
  templateUrl: "./formEdit-solicitacao.component.html",
})
export class FormEditSolicitacaoComponent extends BaseComponent implements OnInit {
  submitted = false;
  display = false;
  post: Solicitacao;
  titulo: string;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() solicitacao: any;
  @Input() idCotacaoSombreroInput: any;

  idCotacaoSombreroOutput: any;

  receiveMessage($event) {
    this.idCotacaoSombreroOutput = $event;
  }

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
  teste: any[];
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

  activeIndex: number = 0;
  EtapasSolicitacao: MenuItem[];
  LoggedUser = this.authenticationService.getLoggedUser();
  apiUrl = environment.api_url;

  tipoSeguroSelecionado: any;

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private service: CotacaoSombreroService,
    private tipoSeguroService: TipoSeguroService,
    private agenciaService: AgenciaService,
    private segmentoService: SegmentoService,
    private funcionarioService: FuncionarioService,
    private canalDeDistribuicaoService: CanalDeDistribuicaoService,
    private tipoProdutoService: TipoProdutoService,
    private seguradoService: SeguradoService,
    private vinculoBNBService: VinculoBNBService,
    private solicitanteService: SolicitanteService,
    private messageService: MessageService,
    private solicitacaoService: SolicitacaoService
  ) {
    super(authenticationService, fb, route, router);
    const solicitacaoEditar = this.router.getCurrentNavigation();
    this.solicitacao = solicitacaoEditar.extras.state;
  }

  ngOnInit() {
    this.Origens = [
      { label: "Email", value: 1 },
      { label: "Telefone", value: 2 },
    ];
    this.EtapasSolicitacao = [
      {
        label: "Solicitante",
        command: () => {
          this.activeIndex = PageNames.Solicitante;
        },
      },
      {
        label: "Solicitação",
        command: () => {
          this.activeIndex = PageNames.Solicitacao;
        },
      },
      {
        label: "Segurado",
        command: () => {
          this.activeIndex = PageNames.Segurado;
        },
      },
      {
        label: "Indicações",
        command: () => {
          this.activeIndex = PageNames.Indicacoes;
        },
      },
      {
        label: "Anexos",
        command: () => {
          this.activeIndex = PageNames.Anexos;
        },
      },
    ];

    this.activeIndex = 0;
    this.indicados = new Array<any>();
    this.anexos = new Array<any>();
    this.uploadedFiles = new Array<any>();

    if (this.solicitacao) {
      this.viewOnly = true;
      this.seguradoPreenchido = false;
      this.titulo = "Editar Solicitação";
      this.indicados = this.solicitacao.indicacoes;
      this.anexos = this.solicitacao.anexos.map((item) => item);
      this.form = this.fb.group({
        id: this.solicitacao.id,
        numero: this.solicitacao.numero,
        dataDeIngresso: this.solicitacao.dataDeIngresso,
        solicitante_id: this.solicitacao.solicitante.id,
        solicitante_nome: this.solicitacao.solicitante.nome,
        solicitante_email: this.solicitacao.solicitante.email,
        solicitante_telefonePrincipal:
          this.solicitacao.solicitante.telefonePrincipal,
        solicitante_telefoneCelular:
          this.solicitacao.solicitante.telefoneCelular,
        solicitante_telefoneAdicional:
          this.solicitacao.solicitante.telefoneAdicional,
        agencia: this.solicitacao.agencia.nome,
        produtor: this.solicitacao.produtor
          ? this.solicitacao.produtor.nome
          : "",
        tipoDeProduto: this.solicitacao.tipoDeProduto.nome,
        canalDeDistribuicao: this.solicitacao.canalDeDistribuicao.nome,
        tipoDeSeguro: this.solicitacao.tipoDeSeguro.nome,
        dadosAdicionais: this.solicitacao.dadosAdicionais,
        segurado_cpfCnpj: this.solicitacao.segurado.cpfCnpj,
        segurado_id: this.solicitacao.segurado.id,
        segurado_nome: this.solicitacao.segurado.nome,
        segurado_email: this.solicitacao.segurado.email,
        segurado_vinculoBNB: this.solicitacao.segurado.vinculoBNB?.nome,
        segurado_emailSecundario: this.solicitacao.segurado.emailSecundario
          ? this.solicitacao.segurado.emailSecundario
          : "",
        segurado_telefonePrincipal: this.solicitacao.segurado.telefonePrincipal,
        segurado_telefoneCelular: this.solicitacao.segurado.telefoneCelular,
        segurado_telefoneAdicional: this.solicitacao.segurado.telefoneAdicional,
        segurado_conta: this.solicitacao.segurado.contato,
        segmento: this.solicitacao.segmento?.nome,
        origem:
          this.solicitacao.origem == "0" || this.solicitacao.origem == 0
            ? ""
            : this.solicitacao.origem == "1" || this.solicitacao.origem == 1
            ? "Email"
            : "Telefone",
        indicacoes: this.solicitacao.indicacoes.map((item) => item),
        dataFimVigencia: moment(this.solicitacao.dataFimVigencia).format(
          "DD/MM/YYYY"
        ),
        orcamentoPrevio: this.solicitacao.orcamentoPrevio,
        mercado: this.solicitacao.mercado,
        agenciaConta: this.solicitacao.agenciaConta.nome,
        nomeIndicado: "",
        telefoneIndicado: "",
        operacaoDeFinanciamento: this.solicitacao.operacaoDeFinanciamento,
      });
      this.tipoSeguroSelecionado = this.solicitacao.tipoDeSeguro;
    } else {
      this.viewOnly = false;
      this.seguradoPreenchido = false;
      this.titulo = "Nova Solicitação";
      this.anexos = new Array<any>();
      this.form = this.fb.group({
        id: 0,
        numero: 0,
        dataDeIngresso: null,
        solicitante_id: this.LoggedUser.solicitante_id,
        solicitante_nome: this.LoggedUser.nome,
        solicitante_email: this.LoggedUser.email,
        solicitante_telefonePrincipal: this.LoggedUser.telefonePrincipal,
        solicitante_telefoneCelular: this.LoggedUser.telefoneCelular,
        solicitante_telefoneAdicional: this.LoggedUser.telefoneAdicional,
        agencia: null,
        produtor: null,
        tipoDeProduto: null,
        canalDeDistribuicao: null,
        tipoDeSeguro: null,
        dadosAdicionais: null,
        segurado_id: 0,
        segurado_cpfCnpj: "",
        segurado_nome: "",
        segurado_email: "",
        segurado_vinculoBNB: null,
        segurado_emailSecundario: "",
        segurado_telefonePrincipal: "",
        segurado_telefoneCelular: "",
        segurado_telefoneAdicional: "",
        segurado_conta: "",
        segmento: null,
        origem: 0,
        indicacoes: [[]],
        dataFimVigencia: null,
        orcamentoPrevio: null,
        mercado: null,
        agenciaConta: null,
        nomeIndicado: "",
        telefoneIndicado: "",
        operacaoDeFinanciamento: null,
      });
      this.vinculoBNBService
        .get(`$filter=(nome eq 'varejo')`)
        .subscribe((data) => {
          this.varejo = data.value;
        });
      if (this.LoggedUser.ehAtendente) {
        this.f.segurado_vinculoBNB.setValue(this.varejo);
      }
      if (this.LoggedUser.ehSolicitante) {
        this.ignoraVarejo = true;
      }
    }
  }

  onClosePanel() {
    window.history.back();
    this.activeIndex = 0;
    this.indicados = new Array<any>();
    this.anexos = new Array<any>();
    this.solicitacao = null;
    this.showError("");
    this.form.reset();
    this.alertError = null;
    this.uploadedFiles = new Array<any>();
    this.setResult({} as Result);
  }

  addItem(newItem: any) {
    this.idCotacaoSombreroInput = newItem;
  }

  onSubmit() {
    if (this.validarSolicitacao()) {
      this.submitted = true;
      this.loading = true;
      const post = {
        id: this.f.id.value || 0,
        Numero: this.f.numero.value || 0,
        Solicitante_Id: this.LoggedUser.solicitante_id,
        Solicitante: {
          Id: this.f.solicitante_id.value,
          nome: this.f.solicitante_nome.value,
          Email: this.f.solicitante_email.value,
          TelefonePrincipal: this.f.solicitante_telefonePrincipal.value,
          TelefoneCelular: this.f.solicitante_telefoneCelular.value,
          TelefoneAdicional: this.f.solicitante_telefoneAdicional.value,
        },
        Agencia_Id: this.f.agencia.value != null ? this.f.agencia.value.id : 0,
        Produtor_Id:
          this.f.produtor.value != null ? this.f.produtor.value.id : 0,
        CotacaoSombrero_Id: this.idCotacaoSombreroOutput,
        TipoDeProduto_Id:
          this.f.tipoDeProduto.value != null
            ? this.f.tipoDeProduto.value.id
            : 0,
        CanalDeDistribuicao_Id:
          this.f.canalDeDistribuicao.value != null
            ? this.f.canalDeDistribuicao.value.id
            : 0,
        TipoDeSeguro_Id:
          this.f.tipoDeSeguro.value != null ? this.f.tipoDeSeguro.value.id : 0,
        OperacaoDeFinanciamento: this.MostrarSeguroObrigatorio()
          ? this.f.operacaoDeFinanciamento.value == true
            ? 1
            : 0
          : null,
        DadosAdicionais: this.f.dadosAdicionais.value,
        Segurado_Id: this.f.segurado_id.value,
        Segurado: {
          Id: this.f.segurado_id.value,
          CpfCnpj: this.f.segurado_cpfCnpj.value,
          Nome: this.f.segurado_nome.value,
          Email: this.f.segurado_email.value,
          VinculoBNB: this.f.segurado_vinculoBNB.value,
          EmailSecundario: this.f.segurado_emailSecundario.value,
          EmailPrincipal: this.f.segurado_email.value,
          TelefonePrincipal: this.f.segurado_telefonePrincipal.value,
          TelefoneAdicional: this.f.segurado_telefoneAdicional.value,
          TelefoneCelular: this.f.segurado_telefoneCelular.value,
          Contato: this.f.segurado_conta.value,
        },
        Segmento_Id:
          this.f.segmento.value != null ? this.f.segmento.value.id : 0,
        Anexos: this.uploadedFiles ? this.uploadedFiles : null,
        Origem: this.f.origem.value,
        Indicacoes: this.indicados,
        DataFimVigencia: this.f.dataFimVigencia.value,
        OrcamentoPrevio: this.f.orcamentoPrevio.value == true ? 1 : 0,
        Mercado: this.f.mercado.value == true ? 1 : 0,
        AgenciaConta_Id:
          this.f.agenciaConta.value != null ? this.f.agenciaConta.value.id : 0,
      };
      this.solicitacaoService.save(post).subscribe(
        (response) => {
          this.setResult(response);
          if (response.successfully) {
            this.form.reset();
            this.eventoConcluido.emit({ adicionar: post.id === 0 });
          }
        },
        (error) => this.showError(error)
      );
    }
  }

  handleOnSelectTipoSeguro(event) {
    this.tipoSeguroSelecionado = event;
  }

  NaoEhSinistro() {
    if (this.tipoSeguroSelecionado != undefined) {
      return this.tipoSeguroSelecionado.nome.toLowerCase() !== "sinistro";
    }
    return true;
  }

  searchAgencias(event) {
    let query = `$filter=contains(nome, '${event.query}') eq true`;
    this.agenciaService.get(query).subscribe((data) => {
      this.Agencias = data.value;
    });
  }

  searchTS(event) {
    let query = `$filter=startswith(nome, '${event.query}') eq true`;
    this.tipoSeguroService.get(query).subscribe((data) => {
      this.TiposDeSeguro = data.value;
    });
  }

  searchSegmentos(event) {
    let query = `$filter=startswith(nOME, '${event.query}') eq true`;
    this.segmentoService.get(query).subscribe((data) => {
      this.Segmentos = data.value;
    });
  }

  searchProdutores(event) {
    let query = `$filter=startswith(nome, '${event.query}') eq true or startswith(matricula, '${event.query}') eq true`;
    this.funcionarioService.get(query).subscribe((data) => {
      this.Produtores = data.value;
    });
  }

  searchCanaisDeDistribuicao(event) {
    let query = `$filter=startswith(nome, '${event.query}') eq true`;
    this.canalDeDistribuicaoService.get(query).subscribe((data) => {
      if (this.LoggedUser.ehAgrosul) {
        this.CanaisDeDistribuicao = data.value.filter((value) => {
          if (value.nome.toLowerCase() == "agrosul") {
            return value;
          }
        });
      } else if (this.LoggedUser.ehAtendente) {
        this.CanaisDeDistribuicao = data.value.filter((value) => {
          if (value.nome.toLowerCase() == "camed") {
            return value;
          }
        });
      } else if (this.LoggedUser.ehSolicitante) {
        this.CanaisDeDistribuicao = data.value.filter((value) => {
          if (
            value.nome.toLowerCase() == "bnb" ||
            value.nome.toLowerCase() == "agrosul"
          ) {
            return value;
          }
        });
      } else {
        this.CanaisDeDistribuicao = data.value;
      }
    });
  }

  searchTipoDeProduto(event) {
    let query = `$filter=startswith(nome, '${event.query}') eq true`;
    this.tipoProdutoService.get(query).subscribe((data) => {
      this.TiposDeProduto = data.value;
    });
  }

  searchVinculoBNB(event) {
    let query = this.ignoraVarejo
      ? `$filter=startswith(nome, '${event.query}') eq true and (id eq ${this.varejo.id}) eq false`
      : `$filter=startswith(nome, '${event.query}') eq true`;
    this.vinculoBNBService.get(query).subscribe((data) => {
      this.VinculosBNB = data.value;
    });
  }

  searchAgenciasContas(event) {
    let query = `$filter=startswith(nome, '${event.query}') eq true`;
    this.agenciaService.get(query).subscribe((data) => {
      this.AgenciasContas = data.value;
    });
  }

  PreencheSegurado() {
    this.loading = true;
    let query = `$expand=vinculoBNB&$filter=(cpfCnpj eq '${this.f.segurado_cpfCnpj.value}') eq true`;
    this.seguradoService.get(query).subscribe((data) => {
      if (data != null && data.value.length > 0) {
        this.f.segurado_id.setValue(data.value[0].id);
        this.f.segurado_nome.setValue(
          data.value[0].nome == null ? "" : data.value[0].nome
        );
        this.f.segurado_email.setValue(
          data.value[0].email == null ? "" : data.value[0].email
        );
        this.f.segurado_emailSecundario.setValue(
          data.value[0].emailSecundario == null
            ? ""
            : data.value[0].emailSecundario
        );
        this.f.segurado_telefonePrincipal.setValue(
          data.value[0].telefonePrincipal == null
            ? ""
            : data.value[0].telefonePrincipal
        );
        this.f.segurado_telefoneCelular.setValue(
          data.value[0].telefoneCelular == null
            ? ""
            : data.value[0].telefoneCelular
        );
        this.f.segurado_telefoneAdicional.setValue(
          data.value[0].telefoneAdicional == null
            ? ""
            : data.value[0].telefoneAdicional
        );
        this.f.segurado_vinculoBNB.setValue(
          data.value[0].vinculoBNB == null ? 0 : data.value[0].vinculoBNB
        );
        this.seguradoPreenchido = true;
      } else {
        this.seguradoPreenchido = false;
      }
      this.loading = false;
    });
  }

  UploadFiles(event: any) {
    if (event.files[0].size <= 1000000) {
      this.solicitacaoService
        .formatFile(0,<File[]>event.files)
        .subscribe((response) => {
          this.uploadedFiles.push(response.payload[0]);
        });
    }
  }

  onClear() {
    this.uploadedFiles = new Array<any>();
  }

  onRemove(nome: string) {
    this.uploadedFiles.splice(
      this.uploadedFiles.indexOf(
        this.uploadedFiles.find((w) => `${w.nome}${w.extensao}` == nome)
      ),
      1
    );
  }

  salvaIndicado() {
    this.indicados.push({
      nome: this.f.nomeIndicado.value,
      telefone: this.f.telefoneIndicado.value,
    });
    this.f.nomeIndicado.setValue("");
    this.f.telefoneIndicado.setValue("");
  }
  excluirIndicado(ind: any) {
    this.indicados.splice(
      this.indicados.indexOf(this.indicados.find((w) => w.nome == ind.nome)),
      1
    );
  }

  MostrarOrigem() {
    return this.LoggedUser.ehAtendente;
  }

  MostrarDadosTalentoPremiado() {
    if (this.f.agencia.value && this.f.agencia.value != undefined) {
      let ehCamed: any;
      if (this.viewOnly) {
        this.f.agencia.value.toLowerCase().includes("camed");
      } else {
        this.f.agencia.value.nome.toLowerCase().includes("camed");
      }

      if (!ehCamed && this.tipoSeguroSelecionado) {
        return true;
      }
    }
    return false;
  }

  MostrarDataFimDeVigencia() {
    if (this.tipoSeguroSelecionado != undefined) {
      return (
        this.tipoSeguroSelecionado.nome.toLowerCase() === "renovação" ||
        this.tipoSeguroSelecionado.nome.toLowerCase() === "prospecção"
      );
    }
    false;
  }
  MostrarAgenciaConta() {
    if (this.f.agencia != undefined && this.f.agencia.value != null) {
      return !this.viewOnly
        ? this.f.agencia.value.nome.toLowerCase().includes("camed")
        : this.f.agencia.value.toLowerCase().includes("camed");
    }
    return false;
  }

  MostrarSeguroObrigatorio() {
    return this.NaoEhSinistro();
  }

  MostrarSegmento() {
    return this.NaoEhSinistro();
  }

  MostrarOrcamentoPrevio() {
    return this.NaoEhSinistro();
  }

  MostrarMercado() {
    return this.NaoEhSinistro();
  }

  MostrarVinculoBNB() {
    return this.NaoEhSinistro();
  }

  downloadFile(fileName: string) {
    const post = {
      fileName: fileName,
      entidade_id: this.solicitacao.id,
    };
    this.solicitacaoService.downloadFile(post).subscribe(
      (Response) => {},
      (error) => this.showError(error)
    );
  }

  DesabilitaVinculoBNB() {
    return this.authenticationService.getLoggedUser().ehAtendente;
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
  }

  modelChangeFn(e) {
    if (e.nome === "AGRICOLA SOMBRERO") {
      debugger;
      this.display = true;
    }
  }
  validarSolicitacao() {
    if (
      !this.f.solicitante_email ||
      this.f.solicitante_email.value == "" ||
      this.f.solicitante_email.value == null ||
      this.f.solicitante_email == undefined
    ) {
      this.showError("É necessário informar qual é o email do solicitante");
      return false;
    }
    if (
      !this.f.solicitante_telefonePrincipal ||
      this.f.solicitante_telefonePrincipal.value == "" ||
      this.f.solicitante_telefonePrincipal.value == null ||
      this.f.solicitante_telefonePrincipal == undefined
    ) {
      this.showError("É necessário informar qual é o telefone do solicitante");
      return false;
    }
    if (
      !this.f.agencia ||
      this.f.agencia.value == null ||
      this.f.agencia == undefined
    ) {
      this.showError("É necessário informar qual é a agência");
      return false;
    }
    if (
      !this.f.tipoDeSeguro ||
      this.f.tipoDeSeguro.value == null ||
      this.f.tipoDeSeguro == undefined
    ) {
      this.showError("É necessário informar qual é o tipo de seguro");
      return false;
    }
    if (
      (!this.f.segmento ||
        this.f.segmento.value == null ||
        this.f.segmento == undefined) &&
      this.MostrarSegmento()
    ) {
      this.showError("É necessário informar qual é o segmento");
      return false;
    }
    if (
      !this.f.dadosAdicionais ||
      this.f.dadosAdicionais.value == "" ||
      !this.f.dadosAdicionais.value ||
      this.f.dadosAdicionais.value == null ||
      this.f.dadosAdicionais == undefined
    ) {
      this.showError("É necessário informar dados adicionais");
      return false;
    }
    if (
      (!this.f.produtor ||
        this.f.produtor.value == null ||
        this.f.produtor == undefined) &&
      this.MostrarDadosTalentoPremiado()
    ) {
      this.showError("É necessário informar qual é o produtor BNB");
      return false;
    }
    if (
      !this.f.canalDeDistribuicao ||
      this.f.canalDeDistribuicao.value == null ||
      this.f.canalDeDistribuicao == undefined
    ) {
      this.showError("É necessário informar qual é o canal de distribuição");
      return false;
    }
    if (
      !this.f.tipoDeProduto ||
      this.f.tipoDeProduto.value == null ||
      this.f.tipoDeProduto == undefined
    ) {
      this.showError("É necessário informar qual é o ramo de seguro");
      return false;
    }
    if (
      !this.f.segurado_cpfCnpj ||
      this.f.segurado_cpfCnpj.value == "" ||
      this.f.segurado_cpfCnpj.value == null ||
      this.f.segurado_cpfCnpj == undefined
    ) {
      this.showError("É necessário informar qual é o cpf ou cnpj do segurado");
      return false;
    }
    if (
      !this.f.segurado_nome ||
      this.f.segurado_nome.value == "" ||
      this.f.segurado_nome.value == null ||
      this.f.segurado_nome == undefined
    ) {
      this.showError("É necessário informar qual é o nome do segurado");
      return false;
    }
    if (
      !this.f.segurado_email ||
      this.f.segurado_email.value == "" ||
      this.f.segurado_email.value == null ||
      this.f.segurado_email == undefined
    ) {
      this.showError("É necessário informar qual é o email do segurado");
      return false;
    }
    if (
      !this.f.segurado_telefonePrincipal ||
      this.f.segurado_telefonePrincipal.value == "" ||
      this.f.segurado_telefonePrincipal.value == null ||
      this.f.segurado_telefonePrincipal == undefined
    ) {
      this.showError("É necessário informar qual é o telefone do segurado");
      return false;
    }
    if (
      (!this.f.segurado_vinculoBNB ||
        this.f.segurado_vinculoBNB.value == null ||
        this.f.segurado_vinculoBNB == undefined) &&
      this.MostrarVinculoBNB() &&
      !this.DesabilitaVinculoBNB()
    ) {
      this.showError("É necessário informar qual é o vinculo BNB do segurado");
      return false;
    }
    if (
      (!this.f.agenciaConta ||
        this.f.agenciaConta.value == null ||
        this.f.agenciaConta == undefined) &&
      (this.MostrarAgenciaConta() || this.NaoEhSinistro())
    ) {
      this.showError("É necessário informar qual é a agência conta");
      return false;
    }
    return true;
  }
}
