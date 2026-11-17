SELECT DB_NAME() AS BaseAtual;

SELECT
    OBJECT_ID(N'dbo.Docente', N'U') AS Docente,
    OBJECT_ID(N'dbo.UC', N'U') AS UC,
    OBJECT_ID(N'dbo.Student', N'U') AS Student,
    OBJECT_ID(N'dbo.InscrUC', N'U') AS InscrUC,
    OBJECT_ID(N'dbo.AvaliacoesUC', N'U') AS AvaliacoesUC,
    OBJECT_ID(N'dbo.Notas', N'U') AS Notas,
    OBJECT_ID(N'dbo.Trabalhos', N'U') AS Trabalhos,
    OBJECT_ID(N'dbo.GruposTrabalho', N'U') AS GruposTrabalho,
    OBJECT_ID(N'dbo.GruposTrabalhoMembros', N'U') AS GruposTrabalhoMembros;

SELECT MigrationId
FROM dbo.__EFMigrationsHistory
ORDER BY MigrationId;