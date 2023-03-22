import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { RouterModule, Routes } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { LayoutModule, HelpPageModule } from "../shared";
import { SidebarModule } from "primeng/sidebar";
import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { SolicitacaoService } from "./solicitacao";
import { AvAtendimentoService } from "./solicitacao/service/avAtendimento.service";

import {
  HttpTokenInterceptor,
  ErrorInterceptor,
  CoreModule,
  AuthGuard,
} from "../core";
import { Globals } from "../globals";

import { PagesComponent } from "./pages.component";
import { AppMenuModule } from "../shared/layout/app-menu/app-menu.module";

const routes: Routes = [
  {
    path: "",
    component: PagesComponent,
    canActivate: [AuthGuard],
    children: [
      { path: "", redirectTo: "dashboard", pathMatch: "full" },

      { path: "", redirectTo: "dashboard", pathMatch: "full" },

      {
        path: "dashboard",
        loadChildren: () =>
          import("./dashboard/dashboard.module").then((m) => m.DashboardModule),
      },
      {
        path: "acoes-de-acompanhamento",
        loadChildren: () =>
          import("./acaodeacompanhamento/acaoDeAcompanhamento.module").then(
            (m) => m.AcaoDeAcompanhamentoModule
          ),
      },
      {
        path: "acoes-de-acompanhamento/cadastrar",
        loadChildren: () =>
          import("./acaodeacompanhamento/acaoDeAcompanhamento.module").then(
            (m) => m.AcaoDeAcompanhamentoModule
          ),
      },
      {
        path: "acoes-de-acompanhamento/editar/:id",
        loadChildren: () =>
          import("./acaodeacompanhamento/acaoDeAcompanhamento.module").then(
            (m) => m.AcaoDeAcompanhamentoModule
          ),
      },
      {
        path: "acoes",
        loadChildren: () =>
          import("./acao/acao.module").then((m) => m.AcaoModule),
      },
      {
        path: "acoes/cadastrar",
        loadChildren: () =>
          import("./acao/acao.module").then((m) => m.AcaoModule),
      },
      {
        path: "acoes/editar/:id",
        loadChildren: () =>
          import("./acao/acao.module").then((m) => m.AcaoModule),
      },
      {
        path: "usuarios",
        loadChildren: () =>
          import("./usuario/usuario.module").then((m) => m.UsuarioModule),
      },
      {
        path: "usuarios/cadastrar",
        loadChildren: () =>
          import("./usuario/usuario.module").then((m) => m.UsuarioModule),
      },
      {
        path: "usuarios/editar/:id",
        loadChildren: () =>
          import("./usuario/usuario.module").then((m) => m.UsuarioModule),
      },
      {
        path: "grupos",
        loadChildren: () =>
          import("./grupo/grupo.module").then((m) => m.GrupoModule),
      },
      {
        path: "grupos/cadastrar",
        loadChildren: () =>
          import("./grupo/grupo.module").then((m) => m.GrupoModule),
      },
      {
        path: "grupos/editar/:id",
        loadChildren: () =>
          import("./grupo/grupo.module").then((m) => m.GrupoModule),
      },
      {
        path: "expedientes",
        loadChildren: () =>
          import("./expediente/expediente.module").then(
            (m) => m.ExpedienteModule
          ),
      },
      {
        path: "expedientes/cadastrar",
        loadChildren: () =>
          import("./expediente/expediente.module").then(
            (m) => m.ExpedienteModule
          ),
      },
      {
        path: "expedientes/editar/:id",
        loadChildren: () =>
          import("./expediente/expediente.module").then(
            (m) => m.ExpedienteModule
          ),
      },
      {
        path: "situacoes",
        loadChildren: () =>
          import("./situacoes/situacao.module").then((m) => m.SituacaoModule),
      },
      {
        path: "situacoes/cadastrar",
        loadChildren: () =>
          import("./situacoes/situacao.module").then((m) => m.SituacaoModule),
      },
      {
        path: "situacoes/editar/:id",
        loadChildren: () =>
          import("./situacoes/situacao.module").then((m) => m.SituacaoModule),
      },
      {
        path: "ramos-de-seguro",
        loadChildren: () =>
          import("./ramosdeseguro/ramoDeSeguro.module").then(
            (m) => m.RamoDeSeguroModule
          ),
      },
      {
        path: "ramos-de-seguro/cadastrar",
        loadChildren: () =>
          import("./ramosdeseguro/ramoDeSeguro.module").then(
            (m) => m.RamoDeSeguroModule
          ),
      },
      {
        path: "ramos-de-seguro/editar/:id",
        loadChildren: () =>
          import("./ramosdeseguro/ramoDeSeguro.module").then(
            (m) => m.RamoDeSeguroModule
          ),
      },
      {
        path: "tipos-de-categoria",
        loadChildren: () =>
          import("./tiposDeCategoria/tipoDeCategoria.module").then(
            (m) => m.TipoDeCategoriaModule
          ),
      },
      {
        path: "tipos-de-categoria/cadastrar",
        loadChildren: () =>
          import("./tiposDeCategoria/tipoDeCategoria.module").then(
            (m) => m.TipoDeCategoriaModule
          ),
      },
      {
        path: "tipos-de-categoria/editar/:id",
        loadChildren: () =>
          import("./tiposDeCategoria/tipoDeCategoria.module").then(
            (m) => m.TipoDeCategoriaModule
          ),
      },
      {
        path: "empresas",
        loadChildren: () =>
          import("./empresas/empresa.module").then((m) => m.EmpresaModule),
      },
      {
        path: "empresas/cadastrar",
        loadChildren: () =>
          import("./empresas/empresa.module").then((m) => m.EmpresaModule),
      },
      {
        path: "empresas/editar/:id",
        loadChildren: () =>
          import("./empresas/empresa.module").then((m) => m.EmpresaModule),
      },
      {
        path: "canais-de-distribuicao",
        loadChildren: () =>
          import("./canaldedistribuicao/canalDeDistribuicao.module").then(
            (m) => m.CanalDeDistribuicaoModule
          ),
      },
      {
        path: "canais-de-distribuicao/cadastrar",
        loadChildren: () =>
          import("./canaldedistribuicao/canalDeDistribuicao.module").then(
            (m) => m.CanalDeDistribuicaoModule
          ),
      },
      {
        path: "canais-de-distribuicao/editar/:id",
        loadChildren: () =>
          import("./canaldedistribuicao/canalDeDistribuicao.module").then(
            (m) => m.CanalDeDistribuicaoModule
          ),
      },
      {
        path: "areas-de-negocio",
        loadChildren: () =>
          import("./areadenegocio/areaDeNegocio.module").then(
            (m) => m.AreaDeNegocioModule
          ),
      },
      {
        path: "areas-de-negocio/cadastrar",
        loadChildren: () =>
          import("./areadenegocio/areaDeNegocio.module").then(
            (m) => m.AreaDeNegocioModule
          ),
      },
      {
        path: "areas-de-negocio/editar/:id",
        loadChildren: () =>
          import("./areadenegocio/areaDeNegocio.module").then(
            (m) => m.AreaDeNegocioModule
          ),
      },
      {
        path: "frames",
        loadChildren: () =>
          import("./frames/frame.module").then((m) => m.FrameModule),
      },
      {
        path: "frames/cadastrar",
        loadChildren: () =>
          import("./frames/frame.module").then((m) => m.FrameModule),
      },
      {
        path: "frames/editar/:id",
        loadChildren: () =>
          import("./frames/frame.module").then((m) => m.FrameModule),
      },
      {
        path: "tipos-de-documento",
        loadChildren: () =>
          import("./tiposDeDocumento/tipoDeDocumento.module").then(
            (m) => m.TipoDeDocumentoModule
          ),
      },
      {
        path: "tipos-de-documento/cadastrar",
        loadChildren: () =>
          import("./tiposDeDocumento/tipoDeDocumento.module").then(
            (m) => m.TipoDeDocumentoModule
          ),
      },
      {
        path: "tipos-de-documento/editar/:id",
        loadChildren: () =>
          import("./tiposDeDocumento/tipoDeDocumento.module").then(
            (m) => m.TipoDeDocumentoModule
          ),
      },
      {
        path: "feriados",
        loadChildren: () =>
          import("./feriado/feriado.module").then((m) => m.FeriadoModule),
      },
      {
        path: "feriados/cadastro",
        loadChildren: () =>
          import("./feriado/feriado.module").then((m) => m.FeriadoModule),
      },

      {
        path: "feriados/editar/:id",
        loadChildren: () =>
          import("./feriado/feriado.module").then((m) => m.FeriadoModule),
      },
      {
        path: "grupos-de-agencias",
        loadChildren: () =>
          import("./grupoagencias/grupoAgencia.module").then(
            (m) => m.GrupoAgenciaModule
          ),
      },
      {
        path: "grupos-de-agencias/cadastrar",
        loadChildren: () =>
          import("./grupoagencias/grupoAgencia.module").then(
            (m) => m.GrupoAgenciaModule
          ),
      },
      {
        path: "grupos-de-agencias/editar/:id",
        loadChildren: () =>
          import("./grupoagencias/grupoAgencia.module").then(
            (m) => m.GrupoAgenciaModule
          ),
      },
      {
        path: "tipos-de-agencia",
        loadChildren: () =>
          import("./tiposDeAgencia/tipoDeAgencia.module").then(
            (m) => m.TipoDeAgenciaModule
          ),
      },
      {
        path: "tipos-de-agencia/cadastrar",
        loadChildren: () =>
          import("./tiposDeAgencia/tipoDeAgencia.module").then(
            (m) => m.TipoDeAgenciaModule
          ),
      },
      {
        path: "tipos-de-agencia/editar/:id",
        loadChildren: () =>
          import("./tiposDeAgencia/tipoDeAgencia.module").then(
            (m) => m.TipoDeAgenciaModule
          ),
      },
      {
        path: "tipos-de-cancelamento",
        loadChildren: () =>
          import("./tiposDeCancelamento/tipoDeCancelamento.module").then(
            (m) => m.TipoDeCancelamentoModule
          ),
      },
      {
        path: "tipos-de-cancelamento/cadastrar",
        loadChildren: () =>
          import("./tiposDeCancelamento/tipoDeCancelamento.module").then(
            (m) => m.TipoDeCancelamentoModule
          ),
      },
      {
        path: "tipos-de-cancelamento/editar/:id",
        loadChildren: () =>
          import("./tiposDeCancelamento/tipoDeCancelamento.module").then(
            (m) => m.TipoDeCancelamentoModule
          ),
      },
      {
        path: "motivos-de-recusa",
        loadChildren: () =>
          import("./motivorecusa/motivoDeRecusa.module").then(
            (m) => m.MotivoDeRecusaModule
          ),
      },
      {
        path: "motivos-de-recusa/cadastrar",
        loadChildren: () =>
          import("./motivorecusa/motivoDeRecusa.module").then(
            (m) => m.MotivoDeRecusaModule
          ),
      },
      {
        path: "motivos-de-recusa/editar/:id",
        loadChildren: () =>
          import("./motivorecusa/motivoDeRecusa.module").then(
            (m) => m.MotivoDeRecusaModule
          ),
      },
      {
        path: "tipos-de-retorno-de-ligacao",
        loadChildren: () =>
          import("./tiporetornoligacao/tipoDeRetornoDeLigacao.module").then(
            (m) => m.TipoDeRetornoDeLigacaoModule
          ),
      },
      {
        path: "tipos-de-retorno-de-ligacao/cadastrar",
        loadChildren: () =>
          import("./tiporetornoligacao/tipoDeRetornoDeLigacao.module").then(
            (m) => m.TipoDeRetornoDeLigacaoModule
          ),
      },
      {
        path: "tipos-de-retorno-de-ligacao/editar/:id",
        loadChildren: () =>
          import("./tiporetornoligacao/tipoDeRetornoDeLigacao.module").then(
            (m) => m.TipoDeRetornoDeLigacaoModule
          ),
      },
      {
        path: "motivos-de-endosso-de-cancelamento",
        loadChildren: () =>
          import("./motivocancelamento/motivoCancelamento.module").then(
            (m) => m.MotivoCancelamentoModule
          ),
      },
      {
        path: "motivos-de-endosso-de-cancelamento/cadastrar",
        loadChildren: () =>
          import("./motivocancelamento/motivoCancelamento.module").then(
            (m) => m.MotivoCancelamentoModule
          ),
      },
      {
        path: "motivos-de-endosso-de-cancelamento/editar/:id",
        loadChildren: () =>
          import("./motivocancelamento/motivoCancelamento.module").then(
            (m) => m.MotivoCancelamentoModule
          ),
      },
      {
        path: "vinculo-bnb",
        loadChildren: () =>
          import("./vinculobnb/vinculoBNB.module").then(
            (m) => m.VinculoBNBModule
          ),
      },
      {
        path: "vinculo-bnb/cadastrar",
        loadChildren: () =>
          import("./vinculobnb/vinculoBNB.module").then(
            (m) => m.VinculoBNBModule
          ),
      },
      {
        path: "vinculo-bnb/editar/:id",
        loadChildren: () =>
          import("./vinculobnb/vinculoBNB.module").then(
            (m) => m.VinculoBNBModule
          ),
      },
      {
        path: "teste-de-conexao",
        loadChildren: () =>
          import("./testeConexao/testeConexao.module").then(
            (m) => m.TesteCoonexaoModule
          ),
      },
      {
        path: "tipos-de-seguro",
        loadChildren: () =>
          import("./tiposdeseguro/tipoDeSeguro.module").then(
            (m) => m.TipoDeSeguroModule
          ),
      },
      {
        path: "tipos-de-seguro/cadastrar",
        loadChildren: () =>
          import("./tiposdeseguro/tipoDeSeguro.module").then(
            (m) => m.TipoDeSeguroModule
          ),
      },
      {
        path: "tipos-de-seguro/editar:id",
        loadChildren: () =>
          import("./tiposdeseguro/tipoDeSeguro.module").then(
            (m) => m.TipoDeSeguroModule
          ),
      },
      {
        path: "fluxo-de-operacao",
        loadChildren: () =>
          import("./fluxoDeOperacao/fluxoDeOperacao.module").then(
            (m) => m.FluxoDeOperacaoModule
          ),
      },
      {
        path: "fluxo-de-operacao/cadastrar",
        loadChildren: () =>
          import("./fluxoDeOperacao/fluxoDeOperacao.module").then(
            (m) => m.FluxoDeOperacaoModule
          ),
      },
      {
        path: "fluxo-de-operacao/editar/:id",
        loadChildren: () =>
          import("./fluxoDeOperacao/fluxoDeOperacao.module").then(
            (m) => m.FluxoDeOperacaoModule
          ),
      },
      {
        path: "auditoria",
        loadChildren: () =>
          import("./auditoria/auditoria.module").then((m) => m.AuditoriaModule),
      },
      {
        path: "auditoria/visualizar-impressao",
        loadChildren: () =>
          import("./auditoria/auditoria.module").then((m) => m.AuditoriaModule),
      },
      {
        path: "auditoria/detalhes/:id/:userName/:eventTime/:eventType/:tableName/:chave/:numeroDaSolicitacao",
        loadChildren: () =>
          import("./auditoria/auditoria.module").then((m) => m.AuditoriaModule),
      },
      {
        path: "agencia-tipos-agencia",
        loadChildren: () =>
          import("./agenciaTipoAgencia/agenciaTipoAgencia.module").then(
            (m) => m.AgenciaTipoAgenciaModule
          ),
      },
      {
        path: "agencia-tipos-agencia/cadastrar",
        loadChildren: () =>
          import("./agenciaTipoAgencia/agenciaTipoAgencia.module").then(
            (m) => m.AgenciaTipoAgenciaModule
          ),
      },
      {
        path: "agencia-tipos-agencia/editar/:id",
        loadChildren: () =>
          import("./agenciaTipoAgencia/agenciaTipoAgencia.module").then(
            (m) => m.AgenciaTipoAgenciaModule
          ),
      },
      {
        path: "consultar-documentos-apolices",
        loadChildren: () =>
          import("./documentoApolice/documentoApolice.module").then(
            (m) => m.DocumentoApoliceModule
          ),
      },
      {
        path: "relatorios",
        loadChildren: () =>
          import('./relatorio/relatorio.module').then(m => m.RelatorioModule)
      },
      {
        path: "menus",
        loadChildren: () =>
          import("./menu/menu.module").then((m) => m.MenuModule),
      },
      {
        path: "menus/cadastrar",
        loadChildren: () =>
          import("./menu/menu.module").then((m) => m.MenuModule),
      },
      {
        path: "menus/editar/:id",
        loadChildren: () =>
          import("./menu/menu.module").then((m) => m.MenuModule),
      },
      {
        path: "tipos-de-mortes",
        loadChildren: () =>
          import("./tipomorte/tipomorte.module").then((m) => m.TipomorteModule),
      },
      {
        path: "tipos-de-mortes/cadastrar",
        loadChildren: () =>
          import("./tipomorte/tipomorte.module").then((m) => m.TipomorteModule),
      },
      {
        path: "tipos-de-mortes/editar/:id",
        loadChildren: () =>
          import("./tipomorte/tipomorte.module").then((m) => m.TipomorteModule),
      },
      {
        path: "inbox",
        loadChildren: () =>
          import("./Inbox/Inbox.module").then((m) => m.InboxModule),
      },
      {
        path: "inbox/form",
        loadChildren: () =>
          import("./Inbox/Inbox.module").then((m) => m.InboxModule),
      },
      {
        path: "solicitacao",
        loadChildren: () =>
          import("./solicitacao/solicitacao.module").then(
            (m) => m.SolicitacaoModule
          ),
      },
      {
        path: "solicitacao/form",
        loadChildren: () =>
          import("./solicitacao/solicitacao.module").then(
            (m) => m.SolicitacaoModule
          ),
      },
      {
        path: "solicitacao/historico",
        loadChildren: () =>
          import("./solicitacao/solicitacao.module").then(
            (m) => m.SolicitacaoModule
          ),
      },
      {
        path: "mapeamento-area-de-negocio",
        loadChildren: () =>
          import(
            "./mapeamentoAreaDeNegocio/mapeamentoAreaDeNegocio.module"
          ).then((m) => m.MapeamentoAreaDeNegocioModule),
      },
      {
        path: "mapeamento-area-de-negocio/cadastrar",
        loadChildren: () =>
          import(
            "./mapeamentoAreaDeNegocio/mapeamentoAreaDeNegocio.module"
          ).then((m) => m.MapeamentoAreaDeNegocioModule),
      },
      {
        path: "mapeamento-area-de-negocio/editar/:id",
        loadChildren: () =>
          import(
            "./mapeamentoAreaDeNegocio/mapeamentoAreaDeNegocio.module"
          ).then((m) => m.MapeamentoAreaDeNegocioModule),
      },
      {
        path: "variaveis-de-email",
        loadChildren: () =>
          import("./variaveisdeemail/variaveisDeEmail.module").then(
            (m) => m.VariaveisDeEmailModule
          ),
      },
      {
        path: "variaveis-de-email",
        loadChildren: () =>
          import("./variaveisdeemail/variaveisDeEmail.module").then(
            (m) => m.VariaveisDeEmailModule
          ),
      },
      {
        path: "variaveis-de-email",
        loadChildren: () =>
          import("./variaveisdeemail/variaveisDeEmail.module").then(
            (m) => m.VariaveisDeEmailModule
          ),
      },
      {
        path: "tipos-de-parametros",
        loadChildren: () =>
          import("./tipos-de-parametros/tipos-de-parametros.module").then(
            (m) => m.TipodeParametroModule
          ),
      },
      {
        path: "mapeamento-de-atendente",
        loadChildren: () =>
          import(
            "./mapeamento-de-atendente/mapeamento-de-atendente.module"
          ).then((m) => m.MapeamentoDeAtendenteModule),
      },
      {
        path: "mapeamento-de-atendente/cadastrar",
        loadChildren: () =>
          import(
            "./mapeamento-de-atendente/mapeamento-de-atendente.module"
          ).then((m) => m.MapeamentoDeAtendenteModule),
      },
      {
        path: "mapeamento-de-atendente/editar/:id",
        loadChildren: () =>
          import(
            "./mapeamento-de-atendente/mapeamento-de-atendente.module"
          ).then((m) => m.MapeamentoDeAtendenteModule),
      },
      {
        path: "texto-personalizado-parametro",
        loadChildren: () =>
          import(
            "./texto-personalizado-parametro/texto-personalizado-parametro.module"
          ).then((m) => m.TextoPersonalizadoParametroModule),
      },
      {
        path: "texto-personalizado-parametro/cadastrar",
        loadChildren: () =>
          import(
            "./texto-personalizado-parametro/texto-personalizado-parametro.module"
          ).then((m) => m.TextoPersonalizadoParametroModule),
      },
      {
        path: "texto-personalizado-parametro/editar/:id",
        loadChildren: () =>
          import(
            "./texto-personalizado-parametro/texto-personalizado-parametro.module"
          ).then((m) => m.TextoPersonalizadoParametroModule),
      },
      {
        path: "campanha",
        loadChildren: () =>
          import("./campanha/campanha.module").then((m) => m.CampanhaModule),
      },
      {
        path: "campanha/cadastrar",
        loadChildren: () =>
          import("./campanha/campanha.module").then((m) => m.CampanhaModule),
      },
      {
        path: "campanha/editar/:id",
        loadChildren: () =>
          import("./campanha/campanha.module").then((m) => m.CampanhaModule),
      },
      {
        path: "alterar-senha",
        loadChildren: () =>
          import("./senhaUsuario/senhaUsuario.module").then(
            (m) => m.SenhaUsuarioModule
          ),
      },
      {
        path: "agendamentos-sla",
        loadChildren: () =>
          import("./slaagendamentos/slaAgendamento.module").then(
            (m) => m.SLAAgendametoModule
          ),
      },

      {
        path: "parametros-do-sistema",
        loadChildren: () =>
          import("./parametros-sistema/parametros-sistema.module").then(
            (m) => m.ParametrosSistemaModule
          ),
      },
      {
        path: "parametros-do-sistema/cadastrar",
        loadChildren: () =>
          import("./parametros-sistema/parametros-sistema.module").then(
            (m) => m.ParametrosSistemaModule
          ),
      },
      {
        path: "parametros-do-sistema/editar/:id",
        loadChildren: () =>
          import("./parametros-sistema/parametros-sistema.module").then(
            (m) => m.ParametrosSistemaModule
          ),
      },
      {
        path: "texto-personalizado-seguradora",
        loadChildren: () =>
          import(
            "./textopersonalizadoseguradora/textopersonalizadoseguro.module"
          ).then((m) => m.TextoPersonalizadoSeguroModule),
      },
      {
        path: "texto-personalizado-seguradora/cadastrar",
        loadChildren: () =>
          import(
            "./textopersonalizadoseguradora/textopersonalizadoseguro.module"
          ).then((m) => m.TextoPersonalizadoSeguroModule),
      },
      {
        path: "texto-personalizado-seguradora/editar/:id",
        loadChildren: () =>
          import(
            "./textopersonalizadoseguradora/textopersonalizadoseguro.module"
          ).then((m) => m.TextoPersonalizadoSeguroModule),
      },
      {
        path: "retornar-a-posicao-de-acompanhamento",
        loadChildren: () =>
          import(
            "./retornarPosicaoAcompanhamento/retornarPosicaoAcompanhamento.module"
          ).then((m) => m.RetornarPosicaoAcompanhamentoModule),
      },
      {
        path: "agendamentos-gerais",
        loadChildren: () =>
          import("./agendamento-de-ligacao/agendamento-de-ligacao.module").then(
            (m) => m.AgendamentoDeLigacaoModule
          ),
      },
      {
        path: "agendamentos-gerais/cadastrar",
        loadChildren: () =>
          import("./agendamento-de-ligacao/agendamento-de-ligacao.module").then(
            (m) => m.AgendamentoDeLigacaoModule
          ),
      },
      {
        path: "agendamentos-gerais/editar/:id",
        loadChildren: () =>
          import("./agendamento-de-ligacao/agendamento-de-ligacao.module").then(
            (m) => m.AgendamentoDeLigacaoModule
          ),
      },
      {
        path: "ausencia-de-atendente",
        loadChildren: () =>
          import("./ausencia-de-atendente/ausencia-de-atendente.module").then(
            (m) => m.AusenciaDeAtendenteModule
          ),
      },
      {
        path: "ausencia-de-atendente/cadastrar",
        loadChildren: () =>
          import("./ausencia-de-atendente/ausencia-de-atendente.module").then(
            (m) => m.AusenciaDeAtendenteModule
          ),
      },
      {
        path: "ausencia-de-atendente/editar/:id",
        loadChildren: () =>
          import("./ausencia-de-atendente/ausencia-de-atendente.module").then(
            (m) => m.AusenciaDeAtendenteModule
          ),
      },
    ],
  },
];


@NgModule({
  imports: [
    CommonModule,
    CoreModule,
    HttpClientModule,
    RouterModule.forChild(routes),
    LayoutModule,
    AppMenuModule,
    HelpPageModule,
    SidebarModule,
    DialogModule, 
    TableModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  declarations: [PagesComponent],
  providers: [
    SolicitacaoService,
    AvAtendimentoService,
    Globals,
    { provide: HTTP_INTERCEPTORS, useClass: HttpTokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  exports: [],
})
export class PagesModule { }
