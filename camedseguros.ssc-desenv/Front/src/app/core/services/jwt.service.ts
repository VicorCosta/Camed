import { Injectable } from '@angular/core';
import { User, Identity } from '../models';
import { Globals } from 'src/app/globals';
import { JwtHelperService } from '@auth0/angular-jwt';


@Injectable()
export class JwtService {

  constructor(private global: Globals, private jwtHelper: JwtHelperService) {

  }

  save(identity: Identity) {
    window.localStorage[this.global.token] = identity.accessToken;
  }

  destroy() {
    window.localStorage.removeItem(this.global.token);
  }

  getToken(): string {
    return window.localStorage[this.global.token];
  }

  private getTokenData() {
    return this.jwtHelper.decodeToken(this.getToken());
  }

  getCurrentUser(): User {
    if (this.getToken()) {
      const tokenData = this.getTokenData();

      if (tokenData) {
        return {
          id: tokenData.Id,
          nome: tokenData.Nome,
          login: tokenData.Login,
          matricula: tokenData.Matricula,
          email: tokenData.Email,
          ehCalculista: tokenData.EhCalculista === 'true',
          ehSolicitante: tokenData.EhSolicitante  === 'true',
          ehAtendente: tokenData.EhAtendente  === 'true',
          ehAgrosul: tokenData.EhAgrosul  === 'true',
          podeVisualizarObservacoes: tokenData.PodeVisualizarObservacoes  === 'true',
          permitidoGerarCotacao: tokenData.PermitidoGerarCotacao  === 'true',
          areasDeNegocio: tokenData.AreasDeNegocio,
          telefonePrincipal: tokenData.TelefonePrincipal,
          telefoneCelular: tokenData.TelefoneCelular,
          telefoneAdicional: tokenData.TelefoneAdicional,
          solicitante_id: tokenData.Solicitante_id
        };
      }
      return null;
    }
    return null;
  }

  tokenHasExpired(): boolean {
    const now = new Date();
    const tokenData = this.getTokenData();

    if (tokenData) {
      return new Date(tokenData.DataExpiracaoToken) < now;
    }
    else {
      return true;
    }
  }

  getAccess(): string[] {
    const tokenData = this.getTokenData();
    return tokenData.Acessos ? tokenData.Acessos.split(',') : null;
  }
}
