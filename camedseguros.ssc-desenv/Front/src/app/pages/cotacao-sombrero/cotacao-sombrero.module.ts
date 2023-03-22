import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { SidebarModule } from "primeng/sidebar";
import { ScrollPanelModule } from "primeng/scrollpanel";
import { MultiSelectModule } from "primeng/multiselect";
import { CheckboxModule } from "primeng/checkbox";

import { AngularSlickgridModule } from "angular-slickgrid";

import { InputMaskModule } from "primeng/inputmask";
import { CotacaoSombreroService } from "./service";
// import { ListGrupoComponent } from "./list";
import { FormCotacaoSombreroComponent } from "./form";
import { CommonModule } from "@angular/common";
import { PlaceholderGridModule, HeaderPageModule } from "src/app/shared";
import { MenuService } from "../menu";
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { AcaoService } from '../acao';


const routes: Routes = [
  // { path: "", component: ListGrupoComponent },
  { path: "cadastrar", component: FormCotacaoSombreroComponent },
  { path: "editar/:id", component: FormCotacaoSombreroComponent },
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    AngularSlickgridModule,
    ScrollPanelModule,
    SidebarModule,
    MultiSelectModule,
    CheckboxModule,
    InputMaskModule,
    FormsModule,
    ReactiveFormsModule,
    PlaceholderGridModule,
    HeaderPageModule,
    ToastModule,
  ],

  declarations: [FormCotacaoSombreroComponent],
  // ListGrupoComponent,
  providers: [CotacaoSombreroService, MenuService, MessageService, AcaoService],
  exports: [FormCotacaoSombreroComponent],
})
export class CotacaoSombreroModule {}
