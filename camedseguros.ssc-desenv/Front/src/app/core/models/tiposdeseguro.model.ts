import { GrupoAgencia } from './grupo-agencia.model';

export interface TiposSeguro {
  id: number;
  nome: string;
  grupoagencia: Array<GrupoAgencia>;
  ramosdeseguro: Array<any>
}
