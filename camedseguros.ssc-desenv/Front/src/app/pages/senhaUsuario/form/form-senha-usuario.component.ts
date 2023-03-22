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
  Empresas,
} from "src/app/core";
import { FormBuilder } from "@angular/forms";
import _ from "underscore";

import { MessageService } from "primeng/api";
import { SenhaUsuarioService } from "../service/";

@Component({
  selector: "",
  templateUrl: "./form-senha-usuario.component.html",
  styleUrls: ["./form-senha-usuario.component.css"],
})
export class FormSenhaUsuarioComponent extends BaseComponent implements OnInit {
  user;
  submitted = false;
  display = false;
  post: Empresas;
  titulo: string;
  senhaAntigaDatabase: string = "Leon#13";
  startAnimationIcons: boolean = false;
  letraMaiuscula: boolean = false;
  letraMinuscula: boolean = false;
  digito: boolean = false;
  caractEspecial: boolean = false;
  totalCaracteres: boolean = false;

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePanel = new EventEmitter<any>();
  @Input() id: any;

  constructor(
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private senhaUsuarioService: SenhaUsuarioService,
    private messageService: MessageService
  ) {
    super(authenticationService, fb, route, router);
  }

  ngOnInit() {
    this.user = JSON.parse(localStorage.getItem("_user"))
    this.submitted = false;
    this.loading = false;
    this.newFormEdit();
  }

  newFormEdit() {
    this.titulo = "Alterar Senha";
    this.form = this.fb.group({
      id: [0],
      username: this.user.username,
      senhaAtual: [null],
      senhaNova: [null],
      senhaConfirmar: [null],
    });
  }

  validateSenha(senha) {
    var re =
      /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$*&@#])[0-9a-zA-Z$*&@#]{6,}$/;
    return re.test(senha);
  }

  validateMaiusc(senha) {
    var re = /^(?=.*[A-Z])/;
    return re.test(senha);
  }

  validateMinusc(senha) {
    var re = /^(?=.*[a-z])/;
    return re.test(senha);
  }

  validateDigit(senha) {
    var re = /[0-9]/;
    return re.test(senha);
  }

  validateCaractEspecial(senha) {
    var re = /^(?=.*[$*&@#])/;
    return re.test(senha);
  }

  validateTotalCaracts(senha) {
    return senha.length >= 6;
  }

  //Send Data
  onSubmit() {
    this.submitted = true;
    this.loading = true;


      this.senhaUsuarioService.save(this.form.value).subscribe(
        (response) => {
          this.setResult(response);
          if (response.successfully) {
            this.messageService.add({
              severity: "success",
              summary: "Sucesso",
              detail: "Senha Editada",
            });

            this.startAnimationIcons = false;
            this.letraMaiuscula = false;
            this.letraMinuscula = false;
            this.digito = false;
            this.caractEspecial = false;
            this.totalCaracteres = false;
            this.newFormEdit();
          }
        },
        (error) => this.showError(error)
      );
  }

  modelChangeIcons(password) {
    if (password.length > 0) {
      this.startAnimationIcons = true;
    } else {
      this.startAnimationIcons = false;
    }
    this.letraMaiuscula = this.validateMaiusc(password);
    this.letraMinuscula = this.validateMinusc(password);
    this.digito = this.validateDigit(password);
    this.caractEspecial = this.validateCaractEspecial(password);
    this.totalCaracteres = this.validateTotalCaracts(password);
  }
}
