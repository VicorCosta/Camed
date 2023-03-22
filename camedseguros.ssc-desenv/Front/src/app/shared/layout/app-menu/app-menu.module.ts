import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AppMenuComponent } from './app-menu.component';
import { ValidacaoCargoService } from 'src/app/service/validacao-cargo.service';
import { UsuarioService } from 'src/app/pages/usuario';
import { UsuarioGrupo } from 'src/app/pages/auditoria';



@NgModule({
  imports: [CommonModule, RouterModule,],
  declarations: [AppMenuComponent],
  providers: [ValidacaoCargoService, UsuarioService, UsuarioGrupo],
  exports: [AppMenuComponent,]
})
export class AppMenuModule { }
