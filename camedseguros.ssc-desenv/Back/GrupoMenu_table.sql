create table GrupoMenu(
    Grupo_Id int not null FOREIGN KEY REFERENCES grupo(id),
    Menu_Id int not null FOREIGN KEY REFERENCES menu(id),
    CONSTRAINT PkGrupoMenu PRIMARY KEY (Grupo_Id, Menu_Id)
);