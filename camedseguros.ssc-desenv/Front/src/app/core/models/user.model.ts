export interface User {
  id: number;
  login: string;
  nome: string;
  email: string;
  matricula: string;
  ehCalculista: boolean;
  ehSolicitante: boolean;
  ehAtendente: boolean;
  ehAgrosul: boolean;
  podeVisualizarObservacoes: boolean;
  permitidoGerarCotacao: boolean;
  areasDeNegocio: any;
  telefonePrincipal: string,
  telefoneCelular: string,
  telefoneAdicional: string,
  solicitante_id: number
}
