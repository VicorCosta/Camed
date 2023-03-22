import { SelectItem } from 'primeng/api';
//#region Imports

import { Component } from '@angular/core';
import { BaseComponent, AuthenticationService, AgenciaService } from 'src/app/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';

import { DocumentoApoliceService } from '../service';

//#endregion

@Component({
  templateUrl: 'list-documentoApolice.component.html',
  styleUrls: ['list-documentoApolice.component.css']
})

export class ListDocumentoApoliceComponent extends BaseComponent {

  documentos: any[];
  agencias: any[];
  agenciaSelecionada: any;
  consultando = false;
  documento = 'CPF';

  agencia: string = null;
  cpfcgc: string = null;
  tipoSolicitacao = '';
  arquivo: any[];
  donwloadSelecionado: any;
  arquivoBoleto: string = null;

  constructor(private service: DocumentoApoliceService,
    private agenciaService: AgenciaService,
    authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router
  ) {
    super(authenticationService, fb, route, router);
  }

  searchAgencia(event) {
    this.agenciaService.get(`$select=codigo,nome&$filter=(contains(nome,'${event.query}'))&$orderby=nome`).subscribe(data => {
      this.agencias = data.value;
    });
  }

  setAgencia(agencia) {
    this.agencia = agencia.codigo;
  }

  consultar() {
    this.alertError = ""

    if (!this.agencia || !this.cpfcgc || !this.tipoSolicitacao) {
      this.alertError = "Preencha todos os campos origatórios"
    } else {

      const doc = this.cpfcgc.match(/\d+/g).join('');;

      this.service.obterDocumentos(this.agencia, doc, this.tipoSolicitacao).subscribe((data) => {
        if (data && data.successfully) {
          this.documentos = data.payload;
          this.consultando = false;
        }
      });
    }
    this.consultando = false;
  }

  funcaoDownload(docCaminho: string) {
    if(docCaminho && docCaminho != "NULL") {
      this.service.downloadDocumento(docCaminho).subscribe((res) => {
        let url = window.URL.createObjectURL(res);
        let a = document.createElement('a');
        a.href = url;
        a.download = 'Arquivo';
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
      });
    }
  }

  checkDownload(arqv) {
    if(arqv == "NULL" || !arqv) {
      return 'red';
    } else {
      return "green";
    }
  }
}
