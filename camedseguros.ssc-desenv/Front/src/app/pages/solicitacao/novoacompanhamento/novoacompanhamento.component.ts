import { TiposCategoriaService } from './../../tiposDeCategoria/service/tipocategoria.service';
  import { TipoDeProdutoService } from './../../texto-personalizado-parametro/service/tipoDeProduto.service';
  import { subscribe } from 'graphql';
  import { element } from 'protractor';
  import { Identity } from './../../../core/models/identity.model';
  import { MessageService } from 'primeng/api';
  /* import { SolicitacaoService } from './../service/solicitacao.service'; */
  import { indexOf } from 'underscore';
  import { AcaoDeAcompanhamentoService } from './../../acaodeacompanhamento/service/acaodeacompanhamento.service';
  import { AcaoDeAcompanhamento } from './../../../core/models/acaodeacompanhamento.model';
  import {
    Component,
    Output,
    EventEmitter,
    Input,
    OnChanges,
    OnInit,
    SimpleChange
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
  import { VinculoBNBService } from "../../../core/services/vinculoBNB.service";
  import { SolicitanteService } from "../../../core/services/solicitante.service";
  import * as moment from "moment";
  import { SituacaoService } from "../../../core/services/situacao.service";
  import { CotacaoSombreroService } from "../../cotacao-sombrero";
  import { SafeResourceUrl, DomSanitizer } from "@angular/platform-browser";
  import { NONE_TYPE } from "@angular/compiler";
  import { TipoProdutoService } from "../../../core/services/tipodeproduto.service";
  import { MapeamentoAcaoSituacaoService } from '../../parametros-sistema';
  import Swal from 'sweetalert2';
  import { CheckboxModule } from 'primeng/checkbox';
  import { VW_SEGURADORA } from "../../textopersonalizadoseguradora";
  import { GrupoProducoesService } from '../service/grupoproducao.service';
  import { MotivoRecusaService } from '../service/motivorecusa.service';
  import { FormaPagamentoService } from '../service/formapagamento.service';
  import { TipoRamoService } from '../service/tiporamo.service';
  import { TipoSeguroGSService } from '../../../core/services/tiposegurogs.service';
  import { FormGroup, Validators } from '@angular/forms';
  import { stringSortComparer } from 'angular-slickgrid';



  export enum PageNames {
    Solicitante,
    Solicitacao,
    Segurado,
    Indicacoes,
    Anexos,
  }

  @Component({
    styleUrls: ["./novoacompanhamento.component.css"],
    templateUrl: "./novoacompanhamento.component.html",
    styles: ['iframe { min-height: 100%; border:0; }']
  })

  export class NovoAcompanhamentoComponent
    extends BaseComponent
    implements OnInit {
    submitted = false;
    display = false;
    post: Solicitacao;
    titulo: string;
    erros: string;
    @Output() eventoConcluido = new EventEmitter<any>();
    @Output() closePanel = new EventEmitter<any>();
    @Input() solicitacao: any;
    @Input() idCotacaoSombreroInput: any;

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
    isDisabled: boolean = false;
    varejo: any;
    ignoraVarejo: boolean = false;
    soLeituraSolicitante: boolean = false;
    seguradoPreenchido: boolean = false;
    acompanhamentos: any = [];
    situacoes: any = [];
    tiposDeProduto: any = [];
    activeIndex: number = 0;
    EtapasSolicitacao: MenuItem[];
    LoggedUser = this.authenticationService.getLoggedUser();
    apiUrl = environment.api_url;

    tipoSeguroSelecionado: any;
    tipodeprodutoSelecionado: any;

    solicitacaoSelecionada: any = [];
    solicitacaoSelecionadaId: any;
    anexosSolicitacao: any;
    acaoSelecionada: any = [];
    acoesIdList: any[] = [];

    situacaoAtualId: any;
    solicitacaoId: any;

    AcaoDeAcompanhamento: any = [];

    src: SafeResourceUrl;
    validarSolicitacao: any;
    acompanhamento: any;
    permissaoobservacao: any;
    permissaoanexo: any;
    visualizaranexo: boolean;
    Observacoesacao: string;
    textoObs: any;
    messageService: any;
    SolicitacaoService: any;
    motivorecusas: any = [];
    tipocategorias: any = [];
    grupoproducoes: any = [];
    pgtoformas: any = [];
    tiposeguros: any = [];
    acaoNovoAcompanhamento: any = {};
    respondendo: any;
    observacaoAcao: any;
    premioAtual: String;
    comissaoAtual: String;
    solicitacao_Id: any;
    viewseguradora: any;
    VW_SEGURADORAService: any;
    vwSeguradora: any = 0;
    anexofiles: any;
    invalidFileSizeMessageSummary: any;
    files: boolean;
    rmatividade: any;
    vwRamo: any;
    numSolicitacao: any = [];
    idramo: boolean;
    motivoSelecionado: any;
    selecaoRastreamento: any = [];
    tipoProdutos: any = [];
    bnbProdutor: any;
    selecaoProdutor: any = [];
    tpSeguradora: any;
    solicitacaoSelecionado: any;
    enviaEmail:  any = [];
    selecionaCross: boolean;

    valorIS: any;
    valorPremioLiquido: any;
    valorPrimeiraParcela: any;
    descricaoCapaProposta: any;
    propostaAnterior: any;
    comissao: any;
    agenciamento: any;
    rastreador: any;
    obsVistoria: any;
    selecionaVistoria: any = [];
    selecionaSeguroObrigatorio: any = [];
    selecionaMercado: any = [];
    selGrupoProducao: any = [];
    selCadastroGS: any = [];
    selSedeEnviaDocumento: any = [];
    selseguradoVIP: any = [];
    selComissaoRV: any = [];
    selPgtoDemaisParcelas: any = [];
    selTipoSeguro: any = [];
    selFormaPagamentoPrimeiraParcela: any = [];
    tpRamo: any;
    dataVencimentoPrimeiraParcela: Date;
    numSol: any;
    numeroSolicitacao: any;
    solSelecionada: any = [];
    seguroGS: any = [];
    selTipoSeguroGS: any = [];
    ramoSelecionado: any;
    bnbProdutorSelecionado: any;
    seguradora: any = [];
    Agencia_Id: number;
    vendaCompartilhada: boolean;
    isSeguroObgChecked: any;
    isOrcamentoPrvChecked: boolean;
    checked: boolean = false;
    checkedcrossup: boolean = false;
    isChecked: any;
    isCheckedCrossUP:  any;
    selCategoria: any;
    tpCategoria: any;
    selecaoCategoria: any;




    constructor(
      authenticationService: AuthenticationService,
      fb: FormBuilder,
      route: ActivatedRoute,
      router: Router,
      private mapeamentoAcaoSituacaoService: MapeamentoAcaoSituacaoService,
      private acaoDeAcompanhamentoService: AcaoDeAcompanhamentoService,
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
      private sanitizer: DomSanitizer,
      private motivoRecusa: MotivoRecusaService,
      private tipoCategoria: TiposCategoriaService,
      private vW_SEGURADORAService: VW_SEGURADORA,
      private gpProducao: GrupoProducoesService,
      private fmPagamento: FormaPagamentoService,
      private tipoRamoService: TipoRamoService,
      private tipoSeguroGSService: TipoSeguroGSService

    ) {
      super(authenticationService, fb, route, router);
      const navigation = router.getCurrentNavigation();
      const solicitacaoEditar = this.router.getCurrentNavigation();
      this.route.params.subscribe((params) => (this.solicitacaoSelecionadaId = params["item"]));
      this.solicitacao = solicitacaoEditar.extras.state;
    }

    receiveMessage($event) {
      this.idCotacaoSombreroOutput = $event;
    }

    ngOnInit() {

      this.titulo = "Novo Acompanhamento";
      this.situacaoAtualId = "";
      this.solicitacaoId = "";
      this.vwSeguradora = "";



      this.route.queryParams.subscribe(params => {
        if (params['0']) this.situacaoAtualId += params['0'];
        if (params['1']) this.situacaoAtualId += params['1'];
        if (params['2']) this.situacaoAtualId += params['2'];
      });

      this.route.queryParams.subscribe(params => {
        if (params['5']) this.solicitacaoId += params['5'];
        if (params['6']) this.solicitacaoId += params['6'];
        if (params['7']) this.solicitacaoId += params['7'];
        if (params['8']) this.solicitacaoId += params['8'];
        if (params['9']) this.solicitacaoId += params['9'];
        if (params['10']) this.solicitacaoId += params['10'];
      });

      this.mapeamentoAcaoSituacaoService.getAll(`$select=SituacaoAtual_Id,ProximaSituacao_Id,Acao_Id&$filter=SituacaoAtual_Id eq ${this.situacaoAtualId}`).subscribe((data) => {
        let dataResult: any[] = data.value;
        for (let i = 0; i < dataResult.length; i++) this.acoesIdList[i] = dataResult[i].acao_Id;
        this.acaoDeAcompanhamentoService.getAll(`$filter=id in (${this.acoesIdList})`).subscribe((res) => {
          this.acompanhamentos = res.value;
        });
      })
      this.motivoRecusa.getAll(`$select=id,descricao&$orderby=descricao`).subscribe((data) => {
        this.motivorecusas = data.value;
      })

   /*    this.tipoCategoria.getAll(`$select=id,descricao&$orderby=id`).subscribe((data) => {
        this.tipocategorias = data.value;
      });
 */
      this.tipoSeguroService.getAll(`$select=id,nome&$orderby=id`).subscribe((data) => {
        this.tiposeguros = data.value;
      });

      this.gpProducao.getAll(`$select=id,nome&$orderby=id`).subscribe((data) => {
        this.grupoproducoes = data.value;
      });

      this.fmPagamento.getAll(`$select=Id,ds_forma&$orderby=id`).subscribe((data) => {
        this.pgtoformas = data.value;
      });

      this.funcionarioService.getAll(`$select=id,nome&$orderby=nome`).subscribe((data) => {
        this.bnbProdutor = data.value;
      });

      this.solicitacaoService.getAll('$select=id,numero&$orderby=numero').subscribe((data) => {
        this.numeroSolicitacao = data.value
      });

      this.tipoSeguroGSService.getAll(`$select=id,nm_Abrev`).subscribe((data) => {
        this.seguroGS = data.value
      });
    }

    onClosePanel() {
      window.history.back();
      this.showError("");
      this.form.reset();
      this.alertError = null;
      this.setResult({} as Result);
    }

    /* Mecanismo de verificação dos frames, qual deles deve ser acionado de acordo com a ação escolhida */
    handleChange() {
      if (this.acaoSelecionada) {
        let query = "";
        this.tipoProdutoService.get(query).subscribe((data) => { this.tipoProdutos = data.value; });
        this.tipoSeguroService.get(`$select=id,nome&$orderby=id`).subscribe((data) => { this.tiposeguros = data.value; });
      }
      if (this.motivoSelecionado) { }
      if (this.enviaEmail) { }
      if (this.selecionaCross) { }
      if (this.selecaoRastreamento) { }
      if (this.selecionaVistoria) { }
      if (this.selecionaSeguroObrigatorio) { }
      if (this.selecionaMercado) { }
      if (this.selGrupoProducao) { }
      if (this.selCadastroGS) { }
      if (this.selSedeEnviaDocumento) { }
      if (this.selseguradoVIP) { }
      if (this.selComissaoRV) { }
      if (this.selTipoSeguroGS) {
          this.tipoSeguroGSService.get(`$select=id,nm_Abrev`).subscribe((data) => { this.seguroGS = data.value });
        if (this.selTipoSeguroGS == 1 || this.selTipoSeguroGS == 5) {
          (document.getElementById('propostaAnterior') as HTMLButtonElement).disabled = false;
        } else if (this.selTipoSeguroGS == 2) {
          (document.getElementById('propostaAnterior') as HTMLButtonElement).disabled = true;
        }
      }
      if (this.tpSeguradora){ }

    }

    handle(errorResponse: any){
      let msg: string;
      if(typeof errorResponse === 'string'){
      msg = errorResponse;
      }
    }

    searchSeguradora(event) {
      let query = `$filter=contains(nm_Seguradora, '${event.query}') eq true`
      this.vW_SEGURADORAService.get(query).subscribe((data) => {
        this.vwSeguradora = data.value;
      });
    }

    selecionaSeguradora(event) {
      console.log(event)
      this.tpSeguradora = event.id
      if (this.tpSeguradora == "1 - UNIMED SEGURADORA S A"){
        (document.getElementById('vwRamo') as HTMLButtonElement).disabled = false;
      }
    }

    searchProdutorBNB(event) {
      let query = `$filter=contains(nome, '${event.query}') eq true`
      this.funcionarioService.get(query).subscribe((data) => {
        this.bnbProdutor = data.value;
      });
    }

    selecionaProdutor(event) {
      this.bnbProdutorSelecionado = event.id
    }

    searchRamo(event) {
      let query = this.tpSeguradora
      this.tipoRamoService.get(`$select=id,nm_Ramo,seguradora_Id&$filter=seguradora_Id eq ${query}`).subscribe((data) => {
        this.vwRamo = data.value;
      });
    }

    selecionaRamo(event) {
      this.ramoSelecionado = event.id
    }

    searchCategoria(event) {
      let query = `$filter=contains(descricao, '${event.query}') eq true`
      this.tipoCategoria.get(query).subscribe((data) => {
        this.selCategoria = data.value;
      });
    }

    selecaoTPCategoria(event){
      this.selecaoCategoria = event.id
    }

    searchNumeroSolicitacao(event) {
      this.solicitacaoService
        .get(`$select=id,numero&$filter=numero eq ${event.query}`)
        .subscribe((data) => {
          this.numSolicitacao = data.value;
        });
    }

    selecionaSolicitacao(event) {
      console.log(event)
      this.numSol = event.numero
    }

    UploadFiles(event: any) {
      if (event.files[0].size <= 1000000) {
        this.solicitacaoService
          .formatFile(this.solicitacaoSelecionado, <File[]>event.files)
          .subscribe((response) => {
            this.uploadedFiles.push(response.payload[0]);
            console.log(this.uploadedFiles);
          });
      }
    }

    handleEnviaEmail(event) {
        this.isChecked = event.checked;
    }

    handleCrossUP(event) {
      this.isCheckedCrossUP = event.checked;
    }

    onSubmit() {
      this.submitted = true;
      this.loading = true;

      if (this.acaoSelecionada == "") {
        throw (document.getElementById('acaoSelectAlert').style.visibility = 'visible');
      } else if (this.acaoSelecionada == 9 || this.acaoSelecionada == 1 || this.acaoSelecionada == 51 && this.observacaoAcao == null){
        throw (document.getElementById('observacao').style.visibility = 'visible');
      } /* else if (this.acaoSelecionada == 7 || this.acaoSelecionada == 54 || this.acaoSelecionada == 58 || this.acaoSelecionada == 64){
        throw (document.getElementById('obsanexos').style.visibility = 'visible');
      }*/

      let elementobservacao = document.getElementById('permissaoobservacao') as HTMLInputElement;
      let elementanexo = document.getElementById('permissaoanexo') as HTMLInputElement;
      let vendaCompartilhada = document.getElementById('vendaCompartilhada') as HTMLInputElement;

      this.acaoNovoAcompanhamento = {
        solicitacao_Id: this.solicitacaoId,
        acao: { id: this.acaoSelecionada },
        observacao: this.observacaoAcao,
        permitevisualizarobservacao: elementobservacao.checked == true ? 1 : 0,
        permitevisualizaranexo: elementanexo.checked == true ? 1 : 0,
        codigodobem: "",
        numerofinanciamento: "",
        cadastro_gs: 0,
        sede_envia_doc_fisico: this.selSedeEnviaDocumento == true ? 1 : 0,
        produtor: { id: this.bnbProdutorSelecionado },
        ramo: { id: this.ramoSelecionado },
        segmento: { id: 0 },
        seguradora: { id: this.tpSeguradora },
        cd_estudo: 0,
        nu_Apolice_Anterior: this.propostaAnterior,
        pc_agenciamento: this.agenciamento,
        pc_comissao: this.comissao,
        co_Corretagem: 0,
        vL_IS: this.valorIS,
        crossup: this.isCheckedCrossUP == true? 1 : 0,
        tipoDeCategoria: this.selecaoCategoria,
        vlr_premiotot_atual: this.premioAtual,
        perc_comissao_atual: this.comissaoAtual,



        motivoRecusa: this.motivoSelecionado,
        email: "a.gmail.com",
        emailSecundario: "",
        telefoneAdicional: "",
        telefoneCelular: "",
        telefonePrincipal: "",
        permiteEmailAoSegurado: this.isChecked == true? 1 : 0 ,
      }

      this.solicitacaoService.novoAcompanhamento(this.acaoNovoAcompanhamento).subscribe((response) => {
        console.log(response);
        Swal.fire(
          '',
          `Novo acompanhamento atualizado!`,
          'success'
        );
      })
    }
  }
  export { MotivoRecusaService, GrupoProducoesService, FormaPagamentoService, TipoRamoService };








