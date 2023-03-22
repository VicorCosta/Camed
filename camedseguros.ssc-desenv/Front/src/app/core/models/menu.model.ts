export interface Menu {
  id: number;
  label: string;
  icone: string;
  rota: string;
  submenus: Menu[];
  superior: Menu;
}
