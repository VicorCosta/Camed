import { AnexoInbox } from './anexo-inbox.model';
import { User } from './user.model';

export interface Inbox {
        UsuarioRemetente: User,
        UsuarioDestinatario: User,
        Texto: string,
        Assunto: string,
        Anexos: AnexoInbox[],
        Lida: boolean,
        DataLida: string,
        VisivelEntrada: boolean 
        VisivelSaida: boolean   
        DataCriacao: string,
        MensagemResp_Id: number
}