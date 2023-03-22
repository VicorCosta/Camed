import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SidebarModule } from 'primeng/sidebar';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { AngularSlickgridModule } from 'angular-slickgrid';

import { AuditoriaService, UsuarioGrupo } from './service';
import { AuditoriaDetalhesService } from './service';
import { ListAuditoriaComponent } from './list';
import { DetailAuditoriaComponent } from './detail/detail-auditoria.component';
import { PrintAuditoriaComponent } from "./print/print-auditoria.component";
import { PlaceholderGridModule, HeaderPageModule } from 'src/app/shared';
import { MomentModule } from 'angular2-moment';
import { NgxPrintModule } from 'ngx-print';

const routes: Routes = [
  { path: '', component: ListAuditoriaComponent },
  { path: 'visualizar-impressao', component: PrintAuditoriaComponent},
  { path: 'detalhes/:id/:userName/:eventTime/:eventType/:tableName/:chave/:numeroDaSolicitacao', component: DetailAuditoriaComponent}
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
    ListAuditoriaComponent,
    DetailAuditoriaComponent,
    PrintAuditoriaComponent,
  ],

  providers: [
    AuditoriaService,
    AuditoriaDetalhesService,
    UsuarioGrupo
  ],
  exports: [
    RouterModule
  ]
})

export class AuditoriaModule { }
