USE [2RP_Processo];
GO

INSERT INTO Tipos(Nome)
VALUES ('geral'),('admin'),('root')
GO

INSERT INTO Usuarios(IdTipo, Nome, Email, Senha)
VALUES (3,'root','root@root.com','root@132')
GO