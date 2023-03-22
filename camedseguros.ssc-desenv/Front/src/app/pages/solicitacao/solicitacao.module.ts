import { Solicitacao } from './../../core/models/solicitacao.model';
import { Form } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { AcompanhamentoService, SolicitacaoService } from './service';
import { AvAtendimentoService } from './service/avAtendimento.service';
import { ListSolicitacaoComponent } from './list';
import { FormSolicitacaoComponent } from './form';
import { FormEditSolicitacaoComponent } from './formEdit';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { AutoCompleteModule } from 'primeng/autocomplete';
import {InputSwitchModule} from 'primeng/inputswitch';
import {InputMaskModule} from 'primeng/inputmask';

import { AgenciaService } from '../../core/services/agencia.service';
import { GrupoService } from '../grupo';
import { TipoSeguroService } from '../tiposdeseguro/service/tiposeguro.service';
import { TiposCancelamentoService } from '../tiposDeCancelamento/service/tipocancelamento.service';
import { UsuarioService } from '../usuario/service/usuario.service';
import { SeguradoService } from '../../core/services/segurado.service';
import { SituacaoService } from '../../core/services/situacao.service';
import { FuncionarioService } from '../../core/services/funcionario.service';
import { SegmentoService } from '../../core/services/segmento.service';
import { CanalDeDistribuicaoService } from '../../core/services/canalDeDistribuicao.service';
import { TipoProdutoService } from '../../core/services/tipodeproduto.service';
import { VinculoBNBService } from '../../core/services/vinculoBNB.service';
import { SolicitanteService } from '../../core/services/solicitante.service';
import { CalendarModule } from 'primeng/calendar';

import { DialogModule } from "primeng/dialog";

import { FileUploadModule } from 'primeng/fileupload';
import {StepsModule} from 'primeng/steps';
import {TabMenuModule} from 'primeng/tabmenu';
import { MomentModule } from 'angular2-moment';

import { from } from 'rxjs';
import { DistribuicaoService } from '../canaldedistribuicao';
import { AcaoDeAcompanhamentoService } from '../acaodeacompanhamento';
import { MapeamentoAcaoSituacaoService } from '../parametros-sistema';
import { TipoDeProdutoService } from '../mapeamentoAreaDeNegocio';
import { AreaDeNegocioService } from '../areadenegocio';
import { VW_SEGURADORA } from '../textopersonalizadoseguradora';
import { CotacaoSombreroModule } from '../cotacao-sombrero/cotacao-sombrero.module';
import { AgendamentoDeLigacaoServices } from '../agendamento-de-ligacao';
import { RetonoligacaoService } from '../tiporetornoligacao';
import {CdkAccordionModule} from '@angular/cdk/accordion';
import { HistoricoSolicitacaoComponent } from './historico/historico-solicitacao.component';
import { NovoAcompanhamentoComponent } from './novoacompanhamento/novoacompanhamento.component';
import { CookieService } from 'ngx-cookie-service';
import { MotivoRecusaService } from './novoacompanhamento/novoacompanhamento.component';
import { TiposCategoriaService } from '../tiposDeCategoria/service/tipocategoria.service';
import { GrupoProducoesService } from './novoacompanhamento/novoacompanhamento.component';
import { FormaPagamentoService } from './novoacompanhamento/novoacompanhamento.component';
import { TipoRamoService } from './novoacompanhamento/novoacompanhamento.component';
import { TipoSeguroGSService } from '../../core/services/tiposegurogs.service';



const routes: Routes = [
  { path: "", component: ListSolicitacaoComponent },
  { path: "form", component: FormSolicitacaoComponent },
  { path: "historico", component: HistoricoSolicitacaoComponent },
  { path: "editar/:id", component: FormEditSolicitacaoComponent },
  { path: "novoacompanhamento", component: NovoAcompanhamentoComponent}

];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    AngularSlickgridModule,
    ScrollPanelModule,
    SidebarModule,
    FormsModule,
    ReactiveFormsModule,
    PlaceholderGridModule,
    HeaderPageModule,
    CotacaoSombreroModule,
    AutoCompleteModule,
    InputSwitchModule,
    FileUploadModule,
    StepsModule,
    DialogModule,
    TabMenuModule,
    InputMaskModule,
    CalendarModule,
    MomentModule,
    CdkAccordionModule,

  ],

  declarations: [
    ListSolicitacaoComponent,
    FormSolicitacaoComponent,
    FormEditSolicitacaoComponent,
    HistoricoSolicitacaoComponent,
    NovoAcompanhamentoComponent

  ],

  providers: [
    AcaoDeAcompanhamentoService,
    MapeamentoAcaoSituacaoService,
    SolicitacaoService,
    AgenciaService,
    GrupoService,
    TipoSeguroService,
    TiposCancelamentoService,
    UsuarioService,
    SeguradoService,
    SituacaoService,
    SegmentoService,
    FuncionarioService,
    RetonoligacaoService,
    CanalDeDistribuicaoService,
    DistribuicaoService,
    TipoProdutoService,
    VinculoBNBService,
    SolicitanteService,
    TipoDeProdutoService,
    AreaDeNegocioService,
    VW_SEGURADORA,
    AcompanhamentoService,
    AvAtendimentoService,
    AgendamentoDeLigacaoServices,
    CookieService,
    MotivoRecusaService,
    TiposCategoriaService,
    GrupoProducoesService,
    FormaPagamentoService,
    TipoRamoService,
    TipoSeguroGSService

  ],
  exports: [RouterModule],
})
export class SolicitacaoModule {}

export class AppModule {}

