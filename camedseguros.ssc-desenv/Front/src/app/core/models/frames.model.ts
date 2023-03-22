import { AcaoDeAcompanhamento } from '.';

export interface Frames {
  id: number;
  nome: string;
  acoesAcompanhamento: Array<AcaoDeAcompanhamento>;
}
