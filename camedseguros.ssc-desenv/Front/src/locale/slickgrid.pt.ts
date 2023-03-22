import { GridOption, Locale } from "angular-slickgrid";

const ptBR: Locale = {
  TEXT_ALL_SELECTED: "Selecionar todos",
  TEXT_CANCEL: "Cancelar",
  TEXT_CLEAR_ALL_FILTERS: "Limpar filtros",
  TEXT_CLEAR_ALL_SORTING: "Limpar ordenações",
  TEXT_COLUMNS: "Colunas",
  TEXT_COMMANDS: "Comandos",
  TEXT_CONTAINS: "Contém",
  TEXT_ENDS_WITH: "Termina com",
  TEXT_EQUALS: "Igual",
  TEXT_EXPORT_TO_CSV:"Exportar para csv",
  TEXT_EXPORT_TO_EXCEL: "Exportar para excel",
  TEXT_FORCE_FIT_COLUMNS: "Ajustar colunas",
  TEXT_GROUP_BY: "Agrupar por",
  TEXT_HIDE_COLUMN: "Esconder coluna",
  TEXT_OK: "OK",
  TEXT_REMOVE_FILTER: "Remover filtro",
  TEXT_REMOVE_SORT: "Remover ordenação",
  TEXT_SAVE: "Salvar",
  TEXT_SELECT_ALL: "Selecionar todos",
  TEXT_SORT_ASCENDING: "Ordenação A-Z",
  TEXT_SORT_DESCENDING: "Ordenação Z-A",
  TEXT_STARTS_WITH: "Começa com",
  TEXT_X_OF_Y_SELECTED: "# de % selecionado(s)",
  TEXT_EXPORT_TO_TEXT_FORMAT:"Exportar para txt",
  TEXT_ITEMS: "itens",
  TEXT_ITEMS_PER_PAGE: "itens por página",
  TEXT_OF: "de",
  TEXT_PAGE: "Página",
  TEXT_REFRESH_DATASET: "Atualizar Grid",
  TEXT_SYNCHRONOUS_RESIZE: "Redimensionamento síncrono",
  TEXT_TOGGLE_FILTER_ROW: "Ocultar/Mostrar filtro",
  TEXT_TOGGLE_PRE_HEADER_ROW: "Ocultar/Mostrar cabeçalho",

  
  TEXT_CLEAR_PINNING: 'Habilitar colunas/linhas',
  TEXT_COLLAPSE_ALL_GROUPS: 'Recolher todos os grupos',
  TEXT_COLUMN_RESIZE_BY_CONTENT: 'Redimensionar por conteúdo',
  TEXT_COPY: 'Copiar',
  TEXT_EMPTY_DATA_WARNING_MESSAGE: 'Nenhum dado encontrado.',
  TEXT_EQUAL_TO: 'Igual a',
  TEXT_EXPAND_ALL_GROUPS: 'Expandir todos os grupos',
  TEXT_EXPORT_TO_TAB_DELIMITED: 'Exportar em formato de texto (delimitado por tabulação)',
  TEXT_FREEZE_COLUMNS: 'Habilitar colunas',
  TEXT_GREATER_THAN: 'Maior que',
  TEXT_GREATER_THAN_OR_EQUAL_TO: 'Maior ou igual a',
  TEXT_LESS_THAN: 'Menor que',
  TEXT_LESS_THAN_OR_EQUAL_TO: 'Menor ou igual a',
  TEXT_NOT_CONTAINS: 'Não contém',
  TEXT_NOT_EQUAL_TO: 'Diferente',
  TEXT_LAST_UPDATE: 'Última atualização',
  TEXT_CLEAR_ALL_GROUPING: 'Limpar todos os grupos',
} as Locale;

const options: GridOption = {} as GridOption;
options.locales = ptBR;

export const localePtBR = ptBR;
export const gridOptions = options;
