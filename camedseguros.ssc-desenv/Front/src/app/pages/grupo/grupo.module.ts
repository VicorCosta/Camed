import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { SidebarModule } from "primeng/sidebar";
import { ScrollPanelModule } from "primeng/scrollpanel";
import { MultiSelectModule } from "primeng/multiselect";
import { CheckboxModule } from "primeng/checkbox";

import { AngularSlickgridModule } from "angular-slickgrid";

import { GrupoService } from "./service";
import { ListGrupoComponent } from "./list";
import { FormGrupoComponent } from "./form";
import { CommonModule } from "@angular/common";
import { PlaceholderGridModule, HeaderPageModule } from "src/app/shared";
import { MenuService } from "../menu";
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { AcaoService } from '../acao';


const routes: Routes = [
  { path: "", component: ListGrupoComponent },
  { path: "cadastrar", component: FormGrupoComponent },
  { path: "editar/:id", component: FormGrupoComponent },
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
    FormsModule,
    ReactiveFormsModule,
    PlaceholderGridModule,
    HeaderPageModule,
    ToastModule
  ],

  declarations: [ListGrupoComponent, FormGrupoComponent],

  providers: [GrupoService, MenuService, MessageService, AcaoService],
  exports: [RouterModule],
})
export class GrupoModule { }
