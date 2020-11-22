
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/20/2020 10:42:48
-- Generated from EDMX file: D:\Software Development\Repositories\CovidTracker\Models\ModelDb.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE COVIDTRACKER;
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Actions_Actions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Actions] DROP CONSTRAINT [FK_Actions_Actions];
GO
IF OBJECT_ID(N'[dbo].[FK_Towns_Towns]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Towns] DROP CONSTRAINT [FK_Towns_Towns];
GO
IF OBJECT_ID(N'[dbo].[FK_UserClaims_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserClaims] DROP CONSTRAINT [FK_UserClaims_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_UserLogins_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserLogins] DROP CONSTRAINT [FK_UserLogins_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRolesInFunctions_Functions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRolesInFunctions] DROP CONSTRAINT [FK_UserRolesInFunctions_Functions];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRolesInFunctions_UserRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRolesInFunctions] DROP CONSTRAINT [FK_UserRolesInFunctions_UserRoles];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersInRoles_UserRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UsersInRoles] DROP CONSTRAINT [FK_UsersInRoles_UserRoles];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersInRoles_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UsersInRoles] DROP CONSTRAINT [FK_UsersInRoles_Users];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Actions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Actions];
GO
IF OBJECT_ID(N'[dbo].[ActionTakens]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActionTakens];
GO
IF OBJECT_ID(N'[dbo].[Functions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Functions];
GO
IF OBJECT_ID(N'[dbo].[Provinces]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Provinces];
GO
IF OBJECT_ID(N'[dbo].[Towns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Towns];
GO
IF OBJECT_ID(N'[dbo].[UserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserClaims];
GO
IF OBJECT_ID(N'[dbo].[UserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserLogins];
GO
IF OBJECT_ID(N'[dbo].[UserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoles];
GO
IF OBJECT_ID(N'[dbo].[UserRolesInFunctions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRolesInFunctions];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[UsersInRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsersInRoles];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Functions'
CREATE TABLE [dbo].[Functions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Action] nvarchar(255)  NULL
);
GO

-- Creating table 'Provinces'
CREATE TABLE [dbo].[Provinces] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL,
    [SortOrder] nchar(10)  NULL
);
GO

-- Creating table 'Towns'
CREATE TABLE [dbo].[Towns] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL,
    [ProvinceId] int  NULL,
    [SortOrder] int  NULL
);
GO

-- Creating table 'UserClaims'
CREATE TABLE [dbo].[UserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'UserLogins'
CREATE TABLE [dbo].[UserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(50)  NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'Actions'
CREATE TABLE [dbo].[Actions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ItemOrder] int  NULL,
    [Category] nvarchar(50)  NULL,
    [Value] nvarchar(max)  NULL,
    [ParentId] int  NULL,
    [OfficeId] int  NULL
);
GO

-- Creating table 'ActionTakens'
CREATE TABLE [dbo].[ActionTakens] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ActionTaken] nvarchar(128)  NULL,
    [TableName] nvarchar(128)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(128)  NULL,
    [EmailConfirmed] bit  NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(25)  NULL,
    [PhoneNumberConfirmed] bit  NULL,
    [TwoFactorEnabled] bit  NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NULL,
    [AccessFailedCount] int  NULL,
    [UserName] nvarchar(50)  NULL,
    [LastUpdatedBy] nvarchar(150)  NULL,
    [LastUpdated] datetime  NULL,
    [CreatedDate] datetime  NULL,
    [FirstName] nvarchar(50)  NULL,
    [LastName] nvarchar(50)  NULL,
    [MiddleName] nvarchar(50)  NULL,
    [CivilStatus] nvarchar(12)  NULL,
    [Gender] nvarchar(6)  NULL,
    [BirthDate] datetime  NULL,
    [AddressLine2] nvarchar(500)  NULL,
    [AddressLine1] nvarchar(500)  NULL,
    [TownCity] int  NULL,
    [Cellular] nvarchar(25)  NULL,
    [Height] decimal(5,2)  NULL,
    [Weight] decimal(5,2)  NULL,
    [Religion] nvarchar(50)  NULL,
    [Citizenship] nvarchar(50)  NULL,
    [Languages] nvarchar(max)  NULL,
    [Position] nvarchar(max)  NULL,
    [OfficeId] int  NULL
);
GO

-- Creating table 'UserRolesInFunctions'
CREATE TABLE [dbo].[UserRolesInFunctions] (
    [Functions_Id] int  NOT NULL,
    [UserRoles_Id] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'UsersInRoles'
CREATE TABLE [dbo].[UsersInRoles] (
    [UserRoles_Id] nvarchar(128)  NOT NULL,
    [Users_Id] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Functions'
ALTER TABLE [dbo].[Functions]
ADD CONSTRAINT [PK_Functions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Provinces'
ALTER TABLE [dbo].[Provinces]
ADD CONSTRAINT [PK_Provinces]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Towns'
ALTER TABLE [dbo].[Towns]
ADD CONSTRAINT [PK_Towns]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserClaims'
ALTER TABLE [dbo].[UserClaims]
ADD CONSTRAINT [PK_UserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'UserLogins'
ALTER TABLE [dbo].[UserLogins]
ADD CONSTRAINT [PK_UserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Actions'
ALTER TABLE [dbo].[Actions]
ADD CONSTRAINT [PK_Actions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ActionTakens'
ALTER TABLE [dbo].[ActionTakens]
ADD CONSTRAINT [PK_ActionTakens]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Functions_Id], [UserRoles_Id] in table 'UserRolesInFunctions'
ALTER TABLE [dbo].[UserRolesInFunctions]
ADD CONSTRAINT [PK_UserRolesInFunctions]
    PRIMARY KEY CLUSTERED ([Functions_Id], [UserRoles_Id] ASC);
GO

-- Creating primary key on [UserRoles_Id], [Users_Id] in table 'UsersInRoles'
ALTER TABLE [dbo].[UsersInRoles]
ADD CONSTRAINT [PK_UsersInRoles]
    PRIMARY KEY CLUSTERED ([UserRoles_Id], [Users_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ProvinceId] in table 'Towns'
ALTER TABLE [dbo].[Towns]
ADD CONSTRAINT [FK_Towns_Towns]
    FOREIGN KEY ([ProvinceId])
    REFERENCES [dbo].[Provinces]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Towns_Towns'
CREATE INDEX [IX_FK_Towns_Towns]
ON [dbo].[Towns]
    ([ProvinceId]);
GO

-- Creating foreign key on [Functions_Id] in table 'UserRolesInFunctions'
ALTER TABLE [dbo].[UserRolesInFunctions]
ADD CONSTRAINT [FK_UserRolesInFunctions_Functions]
    FOREIGN KEY ([Functions_Id])
    REFERENCES [dbo].[Functions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserRoles_Id] in table 'UserRolesInFunctions'
ALTER TABLE [dbo].[UserRolesInFunctions]
ADD CONSTRAINT [FK_UserRolesInFunctions_UserRoles]
    FOREIGN KEY ([UserRoles_Id])
    REFERENCES [dbo].[UserRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRolesInFunctions_UserRoles'
CREATE INDEX [IX_FK_UserRolesInFunctions_UserRoles]
ON [dbo].[UserRolesInFunctions]
    ([UserRoles_Id]);
GO

-- Creating foreign key on [ParentId] in table 'Actions'
ALTER TABLE [dbo].[Actions]
ADD CONSTRAINT [FK_Actions_Actions]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[Actions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Actions_Actions'
CREATE INDEX [IX_FK_Actions_Actions]
ON [dbo].[Actions]
    ([ParentId]);
GO

-- Creating foreign key on [UserId] in table 'UserClaims'
ALTER TABLE [dbo].[UserClaims]
ADD CONSTRAINT [FK_UserClaims_Users]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserClaims_Users'
CREATE INDEX [IX_FK_UserClaims_Users]
ON [dbo].[UserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'UserLogins'
ALTER TABLE [dbo].[UserLogins]
ADD CONSTRAINT [FK_UserLogins_Users]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserLogins_Users'
CREATE INDEX [IX_FK_UserLogins_Users]
ON [dbo].[UserLogins]
    ([UserId]);
GO

-- Creating foreign key on [UserRoles_Id] in table 'UsersInRoles'
ALTER TABLE [dbo].[UsersInRoles]
ADD CONSTRAINT [FK_UsersInRoles_UserRoles]
    FOREIGN KEY ([UserRoles_Id])
    REFERENCES [dbo].[UserRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Users_Id] in table 'UsersInRoles'
ALTER TABLE [dbo].[UsersInRoles]
ADD CONSTRAINT [FK_UsersInRoles_Users]
    FOREIGN KEY ([Users_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersInRoles_Users'
CREATE INDEX [IX_FK_UsersInRoles_Users]
ON [dbo].[UsersInRoles]
    ([Users_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------