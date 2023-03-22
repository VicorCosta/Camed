import { Injectable } from '@angular/core';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { AuthenticationService } from '../core';
import { UsuarioGrupo } from '../pages/auditoria';
import { UsuarioService } from '../pages/usuario';

@Injectable({
  providedIn: 'root'
})

export class ValidacaoCargoService {

  constructor(private userService: UsuarioService, private userGrupoService: UsuarioGrupo, private authenticationService: AuthenticationService) { }

  usuarioLogado: any = this.authenticationService.getLoggedUser()
  isGerente: Subject<boolean> = new BehaviorSubject<boolean>(false);
  isAtendente: Subject<boolean> = new BehaviorSubject<boolean>(false);

  setarCargos(){
    this.userService.getAll(`$select=id&$filter=id eq ${this.usuarioLogado.id}`).subscribe((content) => {
      content.value.map(usuario => {
        this.userGrupoService.getAll(`$select=Usuario_Id,Grupo_Id&$filter=Usuario_Id eq ${usuario.id}`).subscribe((data) => {
          for(let usuarioGrupo of data.value) {
            switch(usuarioGrupo.grupo_Id){
              case 2: 
                this.isGerente.next(true);
              break;

              case 3:
                this.isAtendente.next(true);
              break;
            }
          }
        });
      });
    });

  }
}
