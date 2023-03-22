import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { RelatorioService } from './service';
import { ListRelatorioComponent } from './list';
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { MomentModule } from 'angular2-moment';
import { NgxPrintModule } from 'ngx-print';
import { UsuarioService } from '../usuario';
import { AgenciaService } from 'src/app/core';
import { TipoSeguroService } from '../tiposdeseguro';
import { TipoDeProdutoService } from '../texto-personalizado-parametro/service/tipoDeProduto.service';
import { AreaDeNegocioService } from '../areadenegocio';
import { SituacaoService } from '../situacoes';
import { UsuarioGrupo } from '../auditoria';

const routes: Routes = [
  { path: '', component: ListRelatorioComponent },
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
    MomentModule,
    NgxPrintModule
  ],

  declarations: [
    ListRelatorioComponent,
  ],

  providers: [
    RelatorioService,
    UsuarioService,
    AgenciaService,
    TipoSeguroService,
    TipoDeProdutoService,
    AreaDeNegocioService,
    SituacaoService,
    UsuarioGrupo
  ],
  exports: [
    RouterModule
  ]
})

export class RelatorioModule { }
