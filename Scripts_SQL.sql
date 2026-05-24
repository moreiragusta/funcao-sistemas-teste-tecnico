USE [BancoDeDados]
GO

-- Adiciona coluna CPF na tabela CLIENTES
ALTER TABLE [dbo].[CLIENTES]
ADD [CPF] VARCHAR(14) NOT NULL DEFAULT ''
GO

-- Cria índice único para evitar CPF duplicado
CREATE UNIQUE INDEX IX_CLIENTES_CPF 
ON [dbo].[CLIENTES]([CPF])
WHERE [CPF] <> ''
GO

PRINT 'Campo CPF adicionado com sucesso!'
GO