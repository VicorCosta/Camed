create table Menu
(
    Id int primary key identity(1,1),
    Label nvarchar(200) not null,
    Rota nvarchar(100),
    Icone nvarchar(50),
	AjudaTexto nvarchar(1000),
    Menu_Id int null foreign key references Menu (Id)
)

--Menus Principais
insert into Menu (Label, Rota, Icone)
values 
('Dashboard', '/', 'fa-th-large'), 
('Cadastros', null, 'fa-edit'), 
('Parametrização', null, 'fa-sitemap'), 
('Acompanhamento', null, 'fa-laptop'), 
('Relatórios', null, 'fa-bar-chart-o'),
('Inbox', 'inbox', 'fa-inbox'),
('Solicitação', 'solicitacao', 'far fa-file')

--Menus filhos de Cadastros
insert into Menu (Label, Rota, Menu_Id) values ('Usuários', 'usuarios', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Grupos', 'grupos', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Expedientes', 'expedientes', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Situação', 'situacoes', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Ações de Acompanhamento', 'acoes-de-acompanhamento', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Ramos de Seguro', 'ramos-de-seguro', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Tipos de Categoria', 'tipos-de-categoria', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Empresas', 'empresas', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Canal de Distribuição', 'canais-de-distribuicao', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Área de Negócio', 'areas-de-negocio', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Frames', 'frames', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Tipos de Documento', 'tipos-de-documento', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Feriados', 'feriados', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Grupos de Agências', 'grupos-de-agencias', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Tipos de Agência', 'tipos-de-agencia', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Tipos de Cancelamento', 'tipos-de-cancelamento', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Motivos de Recusa', 'motivos-de-recusa', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Tipos de Retorno de Ligação', 'tipos-de-retorno-de-ligacao', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Motivos de Endosso de Cancelamento', 'motivos-de-endosso-de-cancelamento', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Teste de Conexão', 'teste-de-conexao', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Vínculo BNB', 'vinculo-bnb', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Tipos de Seguro', 'tipos-de-seguro', 2)
insert into Menu (Label, Rota, Menu_Id) values ('Menus', 'menus', 2)

insert into Menu (Label, Rota, Menu_Id) values ('Agência x Tipos de Agência', 'agencia-tipos-agencia', 3)
insert into Menu (Label, Rota, Menu_Id) values ('Auditoria', 'auditoria', 3)
insert into Menu (Label, Rota, Menu_Id) values ('Fluxo de Operação', 'fluxo-de-operacao', 3)


insert into Menu (Label, Rota, Menu_Id) values ('Consultar Documento Apólices', 'consultar-documentos-apolice', 4)
