import { post } from 'jquery';
import { isNull } from 'underscore';
import {
  Component,
  Output,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  ChangeDetectorRef,
  NgZone,
} from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import {
  BaseComponent,
  AuthenticationService,
  Result,
} from "src/app/core";
import { FormBuilder } from "@angular/forms";
import { Observable } from "rxjs";
import { MessageService, SelectItem } from "primeng/api";

import { ParametrosSistemaService } from "../service";
import { TipodeParametroService } from "../../tipos-de-parametros";
import { VariaveisDeEmailService } from "../../variaveisdeemail/service";
import { AcaoService } from '../../acao';

@Component({
  selector: "app-form-parametros-sistema",
  templateUrl: "./form-parametros-sistema.component.html",
  styleUrls: ["./form-parametros-sistema.component.css"],
})
export class FormParametrosSistemaComponent extends BaseComponent implements OnInit {
  submitted = false;
  display = false;
  titulo: string;
  agenciaSelecionada: any;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() usuario: any;
  @Input() id: any;

  tipoDeParametro: Observable<any>;
  variaveisDeEmail: any = [];
  tipo: SelectItem[] = [];

  exibirValorEmail = false;
  exibirValorSMS = false;
  parametros: any = [];


  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private acaoservice: AcaoService,
    private service: ParametrosSistemaService,
    private tipodeParametroService: TipodeParametroService,
    private variaveisDeEmailService: VariaveisDeEmailService,
    private messageService: MessageService,
    private ref: ChangeDetectorRef,
  ) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.id = params['id']);
  }

  getFormEdit() {
    this.service.get(`$filter=id eq ${this.id}`).subscribe(({ value }) => {
      //console.log(value);
      let parametroId = value[0].id;
      this.titulo = 'Editar Parâmetro';

      this.form = this.fb.group({
        id: this.id,
        parametro: value[0].parametro,
        tipoDeParametro_Id: value[0].tipoDeParametro_Id,
        variaveisDeEmail_Id: value[0].variaveisDeEmail_Id,
        tipo: value[0].tipo,
        valor: value[0].valor,
      });

      this.getVariaveisEmail(parametroId);
    })
  }

  getVariaveisEmail(parametroId) {
    this.variaveisDeEmailService.getAll(`$filter=parametro_Id eq ${parametroId}`).subscribe(content => {
      this.variaveisDeEmail = content.value;
      console.log(this.variaveisDeEmail);
    });
  }

  addVariaveis() {
    this.variaveisDeEmail = [];
    const valorParametro = (<HTMLInputElement>document.getElementById("textoParametro")).value

    let ParametroFiltro = this.parametros.filter(elemento => elemento.parametro === valorParametro)
    let idDoParametroDoFiltro = ParametroFiltro.length == 0 ? "null" : this.parametros.filter(elemento => elemento.parametro === valorParametro)[0].variavelDeEmail_Id
    this.acaoservice.getAll(`$filter=id eq ${idDoParametroDoFiltro}`).subscribe(content => {
      content.value.map(resultado => { this.variaveisDeEmail.push({ id: resultado.id, nome: resultado.nome }); })
    });
    console.log(this.variaveisDeEmail)
  }

  exibirCampo(campo: string): boolean {
    return this.form.value.tipo === campo;
  }

  getForm() {
    this.titulo = 'Novo Parâmetro';
    this.form = this.fb.group({
      id: [''],
      parametro: [''],
      tipo: [''],
      valor: ['']
      //tipoDeParametro_Id: [null],
      //variaveisDeEmail_Id: [null],
    });


    if (this.id) {
      this.getFormEdit()
    }
  }

  ngOnInit(): void {
    this.service.get(`$select=id,parametro,variaveisDeEmail_Id`).subscribe(content => {
      content.value.map(resultado => { this.parametros.push({ id: resultado.id, parametro: resultado.parametro, variavelDeEmail_Id: resultado.variaveisDeEmail_Id }); })
    });

    this.tipoDeParametro = this.tipodeParametroService.get("$select=id,nome&$orderby=nome");
    //console.log(this.form.value.parametro);
    
    //this.variaveisDeEmail = this.variaveisDeEmailService.get("$select=id,nome&$orderby=nome");

    this.getForm()
  }

  onClosePanel() {
    this.setResult({} as Result);
    window.history.back()
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    const post = {
      id: this.f.id.value || 0,
      parametro: (this.form.value.parametro ? this.form.value.parametro : null),
      tipoDeParametro_id: ((this.form.value.tipo === 'EMAIL' || this.form.value.tipoDeParametro_Id === 'null') ? null : this.form.value.tipoDeParametro_Id),
      variaveisDeEmail_id: (this.form.value.tipo === 'EMAIL' ? this.form.value.variaveisDeEmail_Id == "null" ? null : this.form.value.variaveisDeEmail_Id : null),
      tipo: this.form.value.tipo,
      valor: this.form.value.valor,
    };

    console.log(post)
    console.log(this.form.value.tipo, this.form.value.tipoDeParametro_Id, this.form.value.variaveisDeEmail_Id)

    this.service.save(post).subscribe(response => {
      this.setResult(response);
      if (response.successfully) {
        this.eventoConcluido.emit({ adicionar: post.id === 0 });
        if (this.titulo == 'Novo Parâmetro') {
          this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Parâmetro Cadastrado' });
          this.getForm()
        } else {
          this.messageService.add({ severity: 'success', summary: 'Sucesso', detail: 'Parâmetro Editado' });
          this.getFormEdit()
        }
      }
    }, (error) => this.showError(error));
  }

  // checkedValorValor(event) {
  //   let email = event.target.value ==='EMAIL';

  //   this.exibirValorEmail = email;
  //   this.exibirValorSMS = !email;
  //   this.ref.detectChanges();
  // }
}
