import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { SidebarModule } from "primeng/sidebar";
import { ScrollPanelModule } from "primeng/scrollpanel";
import { CheckboxModule } from "primeng/checkbox";
import { AngularSlickgridModule } from "angular-slickgrid";

import { TipoDeDocumentoService, TipoMorteService } from "./service";
import { ListTiposDeDocumentoComponent } from "./list";
import { FormTipoDeDocumentoComponent } from "./form";
import { PlaceholderGridModule, HeaderPageModule } from "src/app/shared";
import { RamosService } from "../ramosdeseguro";
import { MultiSelectModule } from "primeng/multiselect";
import { MessageService } from "primeng/api";
import { ToastModule } from "primeng/toast";

const routes: Routes = [
  { path: "", component: ListTiposDeDocumentoComponent },
  { path: "cadastrar", component: FormTipoDeDocumentoComponent },
  { path: "editar/:id", component: FormTipoDeDocumentoComponent },
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    AngularSlickgridModule,
    ScrollPanelModule,
    SidebarModule,
    CheckboxModule,
    FormsModule,
    MultiSelectModule,
    ReactiveFormsModule,
    PlaceholderGridModule,
    HeaderPageModule,
    ToastModule,
  ],

  declarations: [ListTiposDeDocumentoComponent, FormTipoDeDocumentoComponent],

  providers: [
    TipoDeDocumentoService,
    RamosService,
    TipoMorteService,
    MessageService,
  ],
  exports: [RouterModule],
})
export class TipoDeDocumentoModule {}
