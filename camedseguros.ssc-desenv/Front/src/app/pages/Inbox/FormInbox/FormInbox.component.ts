//#region Imports
import { Component, Output, EventEmitter, Input, OnChanges } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService, Inbox, AnexoInbox, Result, AgenciaService } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { InboxService } from '../service';
import { UsuarioService } from '../../usuario';
import { environment } from 'src/environments/environment';
import { MessageService, SelectItem } from 'primeng/api';
import { FixedSizeVirtualScrollStrategy } from '@angular/cdk/scrolling';
import { SolicitacaoService } from '../../solicitacao';
import { TiposCancelamentoService } from '../../tiposDeCancelamento';
import { Observable } from 'rxjs';
import { AcaoService } from '../../acao';
import Swal from 'sweetalert2';

//#endregion

@Component({
  selector: "app-form-inbox",
  templateUrl: "FormInbox.component.html",
})
export class FormInboxComponent extends BaseComponent {
  submitted = false;
  display = false;
  post: Inbox;
  titulo: string;
  usuarios: any[];
  viewOnly: boolean;
  anexosList: any[];
  uploadedFiles: any[] = new Array<any>();
  vizualizacaoRecebido: boolean;
  respondendo: boolean = false;
  url: string;
  apiUrl = environment.api_url;
  tiposCancelamento: Observable<any>;

  solicitacoes$: any[];
  acaoSelecionada: any;

  solicitacaoSelecionada: any;
  solicitacoes: any[];
  agenciaSelecionada: any;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() inbox: any;

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    private usuarioService: UsuarioService,
    private acaoService: AcaoService,
    private solicitacaoService: SolicitacaoService,
    private tiposCancelamentoService: TiposCancelamentoService,
    private messageService: MessageService,
    router: Router,
    private inboxService: InboxService
  ) {
    super(authenticationService, fb, route, router);
    const inbox = this.router.getCurrentNavigation();
    this.inbox = inbox.extras.state;
  }

  ngOnInit() {
    this.solicitacoes = [];
    this.solicitacaoSelecionada = null;

    this.tiposCancelamento =
      this.tiposCancelamentoService.get("$select=descricao");

    this.url = environment.api_url;
    this.respondendo = false;
    // this.inbox = null;

    if (!this.respondendo) {
      this.uploadedFiles = new Array<any>();
      this.anexosList = new Array<any>();

      if (this.inbox) {
        if (
          this.inbox.usuarioRemetente != null &&
          this.inbox.usuarioDestinatario != null
        ) {
          this.vizualizacaoRecebido =
            this.inbox.usuarioRemetente.id ==
              this.authenticationService.getLoggedUser().id
              ? false
              : true;
        } else if (this.inbox.usuarioRemetente == null) {
          this.vizualizacaoRecebido = true;
        } else {
          this.vizualizacaoRecebido = false;
        }

        this.titulo = "Visualizar Mensagem";
        this.viewOnly = true;
        this.anexosList = this.inbox.anexos.map((item) => item);
        this.respondendo = false;

        if (this.inbox.lida == false && this.vizualizacaoRecebido) {
          const post = {
            id: this.inbox.id,
          };
          this.inboxService
            .markAsRead(post)
            .subscribe((error) => this.showError(error));
        }
        console.log(this.inbox);
        this.form = this.fb.group({
          id: this.inbox.id,
          assunto: this.inbox.assunto,
          texto: this.inbox.texto,
          anexos: this.inbox.anexos,
          usuarioRemetente:
            this.inbox.usuarioRemetente != null
              ? this.inbox.usuarioRemetente.nome != null
                ? this.inbox.usuarioRemetente.nome
                : ""
              : "",
          usuariosDestinatarios: [[]],
          usuarioDestinatario:
            this.inbox.usuarioDestinatario != null
              ? this.inbox.usuarioRemetente.email != null
                ? this.inbox.usuarioRemetente.email
                : ""
              : "",
          IdMensagemOriginal: 0,
          numeroSolicitacao:
            this.inbox.solicitacao_Id != null
              ? this.inbox.solicitacao_Id != null
                ? this.inbox.solicitacao_Id
                : ""
              : "",
        });
        this.solicitacaoSelecionada = this.inbox.solicitacao_Id;
      } else {
        this.titulo = "Nova Mensagem";
        this.viewOnly = false;
        this.vizualizacaoRecebido = false;
        this.respondendo = false;
        this.form = this.fb.group({
          id: [0],
          assunto: [""],
          texto: [""],
          anexos: [[]],
          usuarioRemetente: [""],
          usuariosDestinatarios: [[]],
          usuarioDestinatario: null,
          IdMensagemOriginal: 0,
          numeroSolicitacao: null,
        });
      }
    } else {
      this.viewOnly = false;
      this.vizualizacaoRecebido = false;
      this.titulo = "Respondendo uma Mensagem";
      this.form.setValue({
        id: 0,
        assunto: this.inbox.assunto,
        texto: [""],
        anexos: [[]],
        usuarioRemetente: this.authenticationService.getLoggedUser().id,
        usuariosDestinatarios: [[]],
        usuarioDestinatario: this.inbox.usuarioRemetente.nome,
        IdMensagemOriginal: this.inbox.id,
        numeroSolicitacao: this.inbox.numeroSolicitacao.numero,
      });
    }
  }

  onClosePanel() {
    this.respondendo = false;
    this.viewOnly = false;
    this.vizualizacaoRecebido = false;
    this.inbox = null;
    this.form.reset();
    this.showError("");
    this.alertError = null;
    this.uploadedFiles = new Array<any>();
    this.setResult({} as Result);
    window.history.back();
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;

    const newValue = this.form.controls.id.value === 0;
    if (!this.respondendo) {
      const post = {
        RemetenteId: this.authenticationService.getLoggedUser().id,
        Assunto: this.f.assunto.value,
        Texto: this.f.texto.value,
        numero: this.f.texto.value,
        Destinatarios: this.f.usuariosDestinatarios.value == null ? null : this.f.usuariosDestinatarios.value.map(
          (item) => item.id
        ),
        IdMensagemOriginal: 0,
        numeroSolicitacao: this.f.numeroSolicitacao.value,
        Anexos: this.uploadedFiles.map((file) => file),
      };
      this.inboxService.save(post).subscribe(
        (response) => {
          this.setResult(response);
          if (response.successfully) {
            this.form.reset();
            Swal.fire(
              '',
              'Mensagem enviada com sucesso!',
              'success'
            );
            //this.form.controls.usuariosDestinatarios.setValue([[]]);
            this.solicitacaoSelecionada = null;
            this.uploadedFiles = new Array<any>();
            this.eventoConcluido.emit({ newValue });
          }
        },
        (error) => this.showError(error)
      );
    } else {
      const post = {
        RemetenteId: this.authenticationService.getLoggedUser().id,
        Assunto: this.f.assunto.value,
        Texto: this.f.texto.value,
        Destinatarios: [this.inbox.usuarioRemetente.id],
        IdMensagemOriginal: this.inbox.id,
        Anexos: this.uploadedFiles.map((file) => file),
        numeroSolicitacao: this.f.numeroSolicitacao.value,//this.inbox.solicitacao_Id,
      };
      this.inboxService.save(post).subscribe(
        (response) => {
          this.setResult(response);
          if (response.successfully) {
            Swal.fire(
              '',
              'Resposta enviada com sucesso!',
              'success'
            );
            this.onClosePanel();
            this.uploadedFiles = new Array<any>();
            this.eventoConcluido.emit({ newValue });
          }
        },
        (error) => this.showError(error)
      );
    }
  }

  getAgencias() {
    this.solicitacaoService.getAll().subscribe((data) => {
      data.forEach((element) => {
        this.solicitacoes$.push({
          label: element.label,
          value: element.id,
        });
      });
    });
  }

  UploadFiles(event: any) {
    this.inboxService.formatFile(<File[]>event.files).subscribe((response) => {
      this.uploadedFiles = response.payload;
    });
  }

  preparaGridResposta() {
    this.respondendo = true;
    $("#textoInput").val("");
    this.form.controls.texto.setValue("");
    $("#solicitacaoInput").val(null);
    this.form.controls.texto.setValue(null);
  }

  search(event) {
    let query = `$filter=startswith(nome, '${event.query}') and ativo eq true`;

    this.usuarioService.get(query).subscribe((response) => {
      this.usuarios = response.value;
    });
  }

  searchSolicitacoes(event) {
    this.solicitacaoService
      .get(`$select=id,numero&$filter=numero eq ${event.query}`)
      .subscribe((data) => {
        this.solicitacoes = data.value;
      });
  }

  setSolicitacao(acao) {
    this.form.controls.numeroSolicitacao.setValue(acao.id);
  }

  onClear() {
    this.uploadedFiles = new Array<any>();
  }

  onRemove(event: any) {
    this.uploadedFiles.splice(
      this.uploadedFiles.indexOf(
        this.uploadedFiles.find(
          (w) => `${w.nome}${w.extensao}` == event.file.name
        )
      ),
      1
    );
  }

  downloadFile(fileName: string) {
    const post = {
      fileName: fileName,
      entidade_id: this.inbox.id,
    };
    this.inboxService.downloadFile(post).subscribe(
      (Response) => { },
      (error) => this.showError(error)
    );
  }

  validarInbox() {
    if (
      this.f.usuariosDestinatarios.value == null ||
      this.f.usuariosDestinatarios.value == undefined
    ) {
      this.showError("É necessário informar pelo menos um destinatário");
      return false;
    }
  }
}



