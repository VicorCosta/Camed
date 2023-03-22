import { Component, OnInit } from '@angular/core';
import { Console } from 'console';
import { Router, ActivatedRoute, NavigationExtras } from "@angular/router";
import { AuthenticationService, BaseComponent } from "src/app/core";
import { SolicitacaoService } from './solicitacao';
import { FormBuilder } from "@angular/forms";
import { AvAtendimentoService } from './solicitacao/service/avAtendimento.service';

declare var $: any;


@Component({
  selector: 'app-pages',
  templateUrl: 'pages.component.html',

})

export class PagesComponent extends BaseComponent implements OnInit {

  display: boolean = false;
  pendenciaAvLista: any[] = [];
  pendenciaAcaoLista: any[] = [];


  obsDaAvaliacao = "";
  obsAvAtendimentoFlag = false;
  notaDinamicaAv = "";
  notaDaAv = "";
  solicitacao_Id = "";
  disabled = false;

  constructor(
    private solicitacaoService: SolicitacaoService,
    authenticationService: AuthenticationService,
    private avAtendimentoService: AvAtendimentoService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router
  ) {
    super(authenticationService, fb, route, router)
    }

  ngOnInit(): void {

    $('.navbar-minimalize').on('click', (event) => {
      event.preventDefault();
      $('body').toggleClass('mini-navbar');
      if (!$('body').hasClass('mini-navbar') || $('body').hasClass('body-small')) {
        $('#side-menu').hide();
        setTimeout(
          () => {
            $('#side-menu').fadeIn(400);
          }, 200);
      } else if ($('body').hasClass('fixed-sidebar')) {
        $('#side-menu').hide();
        setTimeout(
          () => {
            $('#side-menu').fadeIn(400);
          }, 100);
      } else {
        $('#side-menu').removeAttr('style');
      }
    });
  }

  searchSolicitacoesPendenciaAv() {
    this.pendenciaAvLista = [];
    let loggedUser = this.authenticationService.getLoggedUser();
    
    let query = "$select=numero,id&$expand=avaliacoes,operador($select=nome,id),segurado,agencia($select=nome,id),agenciaConta($select=nome,id),situacaoAtual($select=nome,eFimFluxo,pendenciacliente),tipoDeSeguro,tipoDeCancelamento($select=descricao),atendente($select=nome,id)";
    let filter = `$filter=(situacaoAtual/eFimFluxo eq true)and(operador/id eq ${loggedUser.id})`;

    this.solicitacaoService.getAll(`${query}&${filter}`).subscribe(data => {
      if (data.value) {
        data.value.forEach(el => {
          if (el.avaliacoes.length < 1) this.pendenciaAvLista.push(el);
        });
      }
    })
  }

  searchSolicitacoesPendenciaAcao() {
    this.pendenciaAcaoLista = [];
    let loggedUser = this.authenticationService.getLoggedUser();
    let query = "$select=numero,aplicacao,dadosAdicionais,dataDeIngresso,operacaoDeFinanciamento,origem,dataFimVigencia,orcamentoPrevio,mercado,id&$expand=operador($select=nome,id),segurado($expand=vinculoBNB),solicitante,seguradora($select=nm_seguradora,id),agencia($select=nome,id),agenciaConta($select=nome,id),situacaoAtual($select=nome,eFimFluxo,pendenciacliente),tipoDeSeguro,areaDeNegocio,tipoDeProduto($select=id,nome),canalDeDistribuicao,segmento,tipoDeCancelamento($select=descricao),indicacoes,produtor($select=nome,id),atendente($select=nome,id)";
    let filter = `$filter=(situacaoAtual/eFimFluxo eq false)and(situacaoAtual/pendenciaCliente eq true)and(operador/id eq ${loggedUser.id})`;

    this.solicitacaoService.getAll(`${query}&${filter}`).subscribe(data => {
      this.pendenciaAcaoLista = data.value;
    });
  }

  showDialog() {
    this.display = true;

    this.searchSolicitacoesPendenciaAv();
    this.searchSolicitacoesPendenciaAcao();
  }

  abrirHistorico(item) {
    this.display = false;

    this.router.navigate([`solicitacao/historico`], {
      relativeTo: this.route,
      state: item,
    });
  }

  avaliarAtendimento(item) {
    this.display = false;

    this.solicitacao_Id = item.id;
    this.obsDaAvaliacao = "";
    $("#alertObsAtendimento").css("visibility", "hidden");

    if (item.situacaoAtual.eFimFluxo) {
      let avaliacao = item.avaliacoes;
      if (avaliacao.length < 1 && this.authenticationService.getLoggedUser().ehSolicitante) {
        this.disabled = false;
        this.notaDinamicaAv = '0';
        this.notaDaAv = "";
        let element = <HTMLInputElement>document.getElementById('rating2--0');
        element.checked = true;
        $("#alertAvAtendimento").css("visibility", "hidden");
        ($("#avaliarSolicitacao") as any).modal("show");
      } else if (avaliacao.length >= 1) {
        this.disabled = true;
        this.obsDaAvaliacao = avaliacao[0].observacao;
        $("#alertAvAtendimento").css("visibility", "hidden");
        ($("#avaliarSolicitacao") as any).modal("show");

        if (avaliacao[0].nota == "5") {
          let element = <HTMLInputElement>document.getElementById("rating2--50");
          element.checked = true;
          this.notaDinamicaAv = '5';
        }

        else if (avaliacao[0].nota == "4.5") {
          let element = <HTMLInputElement>document.getElementById("rating2--45");
          element.checked = true;
          this.notaDinamicaAv = '4.5';
        }

        else if (avaliacao[0].nota == "4") {
          let element = <HTMLInputElement>document.getElementById("rating2--40");
          element.checked = true;
          this.notaDinamicaAv = '4';
        }

        else if (avaliacao[0].nota == "3.5") {
          let element = <HTMLInputElement>document.getElementById("rating2--35");
          element.checked = true;
          this.notaDinamicaAv = '3.5';
        }

        else if (avaliacao[0].nota == "3") {
          let element = <HTMLInputElement>document.getElementById("rating2--30");
          element.checked = true;
          this.notaDinamicaAv = '3';
        }

        else if (avaliacao[0].nota == "2.5") {
          let element = <HTMLInputElement>document.getElementById("rating2--25");
          element.checked = true;
          this.notaDinamicaAv = '2.5';
        }

        else if (avaliacao[0].nota == "2") {
          let element = <HTMLInputElement>document.getElementById("rating2--20");
          element.checked = true;
          this.notaDinamicaAv = '2';
        }

        else if (avaliacao[0].nota == "1.5") {
          let element = <HTMLInputElement>document.getElementById("rating2--15");
          element.checked = true;
          this.notaDinamicaAv = '1.5';
        }

        else if (avaliacao[0].nota == "1") {
          let element = <HTMLInputElement>document.getElementById("rating2--10");
          element.checked = true;
          this.notaDinamicaAv = '1';
        }

        else if (avaliacao[0].nota == "0.5") {
          let element = <HTMLInputElement>document.getElementById("rating2--05");
          element.checked = true;
          this.notaDinamicaAv = '0.5';
        }

        else if (avaliacao[0].nota == "0") {
          let element = <HTMLInputElement>document.getElementById("rating2--0");
          element.checked = true;
          this.notaDinamicaAv = '0';
        }
      }
    }
  }

  setNotaDinamica(nota) {
    if (!this.disabled) this.notaDinamicaAv = nota;
  }

  notaAtualAv() {
    if (this.notaDaAv && !this.disabled) {
      this.notaDinamicaAv = this.notaDaAv;
    } else if (!this.disabled) {
      this.notaDinamicaAv = '0';
    }
  }

  notaParaAv(nota) {
    if (!this.disabled) this.notaDaAv = nota;
  }

  salvarAvSolicitacao() {
    //"2023-01-20T13:29:07.208Z"
    const atendimentoSolicitacao = {
      observacao: this.obsDaAvaliacao,
      nota: this.notaDaAv,
      solicitacao_Id: this.solicitacao_Id,
      dataAvaliacao: new Date(),
      usuario_Id: this.authenticationService.getLoggedUser().id
    }

    debugger;

    if (this.obsDaAvaliacao.length <= 0 && this.notaDaAv != "5") {
      $("#alertObsAtendimento").css("visibility", "visible");
    } else {
      this.avAtendimentoService.save(atendimentoSolicitacao).subscribe((response) => {
        console.log(response);
      });
    }
  }
}
