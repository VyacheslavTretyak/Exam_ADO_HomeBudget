
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/27/2018 15:59:09
-- Generated from EDMX file: E:\Documents\visual studio 2017\Projects\ADO.NET\Exam_ADO\HomeBudget\DataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [HomeBudgetDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_OperationAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OperationSet] DROP CONSTRAINT [FK_OperationAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_OperationOperationName]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OperationSet] DROP CONSTRAINT [FK_OperationOperationName];
GO
IF OBJECT_ID(N'[dbo].[FK_TransferAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransferSet] DROP CONSTRAINT [FK_TransferAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_TransferAccount1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransferSet] DROP CONSTRAINT [FK_TransferAccount1];
GO
IF OBJECT_ID(N'[dbo].[FK_OperationPerson]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OperationSet] DROP CONSTRAINT [FK_OperationPerson];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[PersonSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonSet];
GO
IF OBJECT_ID(N'[dbo].[OperationNameSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OperationNameSet];
GO
IF OBJECT_ID(N'[dbo].[AccountSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountSet];
GO
IF OBJECT_ID(N'[dbo].[OperationSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OperationSet];
GO
IF OBJECT_ID(N'[dbo].[TransferSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransferSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'PersonSet'
CREATE TABLE [dbo].[PersonSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'OperationNameSet'
CREATE TABLE [dbo].[OperationNameSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'AccountSet'
CREATE TABLE [dbo].[AccountSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'OperationSet'
CREATE TABLE [dbo].[OperationSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Sum] float  NOT NULL,
    [Comment] nvarchar(255)  NULL,
    [Date] datetime  NOT NULL,
    [Account_Id] int  NOT NULL,
    [OperationName_Id] int  NOT NULL,
    [Person_Id] int  NOT NULL
);
GO

-- Creating table 'TransferSet'
CREATE TABLE [dbo].[TransferSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Sum] float  NOT NULL,
    [AccountFrom_Id] int  NOT NULL,
    [AccountTo_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'PersonSet'
ALTER TABLE [dbo].[PersonSet]
ADD CONSTRAINT [PK_PersonSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OperationNameSet'
ALTER TABLE [dbo].[OperationNameSet]
ADD CONSTRAINT [PK_OperationNameSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccountSet'
ALTER TABLE [dbo].[AccountSet]
ADD CONSTRAINT [PK_AccountSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OperationSet'
ALTER TABLE [dbo].[OperationSet]
ADD CONSTRAINT [PK_OperationSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransferSet'
ALTER TABLE [dbo].[TransferSet]
ADD CONSTRAINT [PK_TransferSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Account_Id] in table 'OperationSet'
ALTER TABLE [dbo].[OperationSet]
ADD CONSTRAINT [FK_OperationAccount]
    FOREIGN KEY ([Account_Id])
    REFERENCES [dbo].[AccountSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OperationAccount'
CREATE INDEX [IX_FK_OperationAccount]
ON [dbo].[OperationSet]
    ([Account_Id]);
GO

-- Creating foreign key on [OperationName_Id] in table 'OperationSet'
ALTER TABLE [dbo].[OperationSet]
ADD CONSTRAINT [FK_OperationOperationName]
    FOREIGN KEY ([OperationName_Id])
    REFERENCES [dbo].[OperationNameSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OperationOperationName'
CREATE INDEX [IX_FK_OperationOperationName]
ON [dbo].[OperationSet]
    ([OperationName_Id]);
GO

-- Creating foreign key on [AccountFrom_Id] in table 'TransferSet'
ALTER TABLE [dbo].[TransferSet]
ADD CONSTRAINT [FK_TransferAccount]
    FOREIGN KEY ([AccountFrom_Id])
    REFERENCES [dbo].[AccountSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransferAccount'
CREATE INDEX [IX_FK_TransferAccount]
ON [dbo].[TransferSet]
    ([AccountFrom_Id]);
GO

-- Creating foreign key on [AccountTo_Id] in table 'TransferSet'
ALTER TABLE [dbo].[TransferSet]
ADD CONSTRAINT [FK_TransferAccount1]
    FOREIGN KEY ([AccountTo_Id])
    REFERENCES [dbo].[AccountSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransferAccount1'
CREATE INDEX [IX_FK_TransferAccount1]
ON [dbo].[TransferSet]
    ([AccountTo_Id]);
GO

-- Creating foreign key on [Person_Id] in table 'OperationSet'
ALTER TABLE [dbo].[OperationSet]
ADD CONSTRAINT [FK_OperationPerson]
    FOREIGN KEY ([Person_Id])
    REFERENCES [dbo].[PersonSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OperationPerson'
CREATE INDEX [IX_FK_OperationPerson]
ON [dbo].[OperationSet]
    ([Person_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------