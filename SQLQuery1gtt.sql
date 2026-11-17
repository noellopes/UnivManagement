SELECT DB_NAME() AS BaseAtual,
       OBJECT_ID(N'dbo.Trabalhos', N'U') AS Trabalhos,
       OBJECT_ID(N'dbo.GruposTrabalho', N'U') AS GruposTrabalho,
       OBJECT_ID(N'dbo.GruposTrabalhoMembros', N'U') AS GruposTrabalhoMembros;