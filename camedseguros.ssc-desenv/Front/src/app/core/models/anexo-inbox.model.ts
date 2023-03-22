import { Inbox } from './inbox.model';

export interface AnexoInbox {
    Nome: string,
    Caminho: string,
    Bytes: Blob, 
    Inbox: Inbox
}