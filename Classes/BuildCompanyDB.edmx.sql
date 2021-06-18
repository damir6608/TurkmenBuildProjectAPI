
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/27/2021 20:42:33
-- Generated from EDMX file: C:\Users\Oleg\source\repos\ServerAPI\Classes\BuildCompanyDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [C:\USERS\OLEG\SOURCE\REPOS\SERVERAPI\BIN\DATABASE\BUILDCOMPANYDB.MDF];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserUserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserProfileSet] DROP CONSTRAINT [FK_UserUserProfile];
GO
IF OBJECT_ID(N'[dbo].[FK_UserOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderSet] DROP CONSTRAINT [FK_UserOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderRent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RentSet] DROP CONSTRAINT [FK_OrderRent];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderProject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProjectSet] DROP CONSTRAINT [FK_OrderProject];
GO
IF OBJECT_ID(N'[dbo].[FK_RentGoods]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GoodsSet] DROP CONSTRAINT [FK_RentGoods];
GO
IF OBJECT_ID(N'[dbo].[FK_RentVehicles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VehiclesSet] DROP CONSTRAINT [FK_RentVehicles];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[UserProfileSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserProfileSet];
GO
IF OBJECT_ID(N'[dbo].[OrderSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderSet];
GO
IF OBJECT_ID(N'[dbo].[RentSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RentSet];
GO
IF OBJECT_ID(N'[dbo].[ProjectSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectSet];
GO
IF OBJECT_ID(N'[dbo].[GoodsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GoodsSet];
GO
IF OBJECT_ID(N'[dbo].[VehiclesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VehiclesSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Login] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Role] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserProfileSet'
CREATE TABLE [dbo].[UserProfileSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NOT NULL,
    [Information] nvarchar(max)  NULL,
    [Picture] nvarchar(max)  NOT NULL,
    [User_Id] int  NOT NULL
);
GO

-- Creating table 'OrderSet'
CREATE TABLE [dbo].[OrderSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateOfOrder] datetime  NOT NULL,
    [OrderType] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'RentSet'
CREATE TABLE [dbo].[RentSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DurationTime] int  NOT NULL,
    [RentType] nvarchar(max)  NOT NULL,
    [Order_Id] int  NULL
);
GO

-- Creating table 'ProjectSet'
CREATE TABLE [dbo].[ProjectSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectType] nvarchar(max)  NOT NULL,
    [ProjectPrice] float  NOT NULL,
    [ProjectArea] float  NOT NULL,
    [Information] nvarchar(max)  NULL,
    [Picture] nvarchar(max)  NOT NULL,
    [Foundation] bit  NOT NULL,
    [Order_Id] int  NULL
);
GO

-- Creating table 'GoodsSet'
CREATE TABLE [dbo].[GoodsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GoodName] nvarchar(max)  NOT NULL,
    [Weight] float  NOT NULL,
    [RentPrice] float  NOT NULL,
    [Information] nvarchar(max)  NULL,
    [Picture] nvarchar(max)  NOT NULL,
    [Rent_Id] int  NULL
);
GO

-- Creating table 'VehiclesSet'
CREATE TABLE [dbo].[VehiclesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [VehicleName] nvarchar(max)  NOT NULL,
    [MaxWeight] float  NOT NULL,
    [RentPrice] float  NOT NULL,
    [GasolineLevel] float  NOT NULL,
    [Information] nvarchar(max)  NULL,
    [Picture] nvarchar(max)  NOT NULL,
    [Rent_Id] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserProfileSet'
ALTER TABLE [dbo].[UserProfileSet]
ADD CONSTRAINT [PK_UserProfileSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OrderSet'
ALTER TABLE [dbo].[OrderSet]
ADD CONSTRAINT [PK_OrderSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RentSet'
ALTER TABLE [dbo].[RentSet]
ADD CONSTRAINT [PK_RentSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectSet'
ALTER TABLE [dbo].[ProjectSet]
ADD CONSTRAINT [PK_ProjectSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GoodsSet'
ALTER TABLE [dbo].[GoodsSet]
ADD CONSTRAINT [PK_GoodsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VehiclesSet'
ALTER TABLE [dbo].[VehiclesSet]
ADD CONSTRAINT [PK_VehiclesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'UserProfileSet'
ALTER TABLE [dbo].[UserProfileSet]
ADD CONSTRAINT [FK_UserUserProfile]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserUserProfile'
CREATE INDEX [IX_FK_UserUserProfile]
ON [dbo].[UserProfileSet]
    ([User_Id]);
GO

-- Creating foreign key on [UserId] in table 'OrderSet'
ALTER TABLE [dbo].[OrderSet]
ADD CONSTRAINT [FK_UserOrder]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserOrder'
CREATE INDEX [IX_FK_UserOrder]
ON [dbo].[OrderSet]
    ([UserId]);
GO

-- Creating foreign key on [Order_Id] in table 'RentSet'
ALTER TABLE [dbo].[RentSet]
ADD CONSTRAINT [FK_OrderRent]
    FOREIGN KEY ([Order_Id])
    REFERENCES [dbo].[OrderSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderRent'
CREATE INDEX [IX_FK_OrderRent]
ON [dbo].[RentSet]
    ([Order_Id]);
GO

-- Creating foreign key on [Order_Id] in table 'ProjectSet'
ALTER TABLE [dbo].[ProjectSet]
ADD CONSTRAINT [FK_OrderProject]
    FOREIGN KEY ([Order_Id])
    REFERENCES [dbo].[OrderSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderProject'
CREATE INDEX [IX_FK_OrderProject]
ON [dbo].[ProjectSet]
    ([Order_Id]);
GO

-- Creating foreign key on [Rent_Id] in table 'GoodsSet'
ALTER TABLE [dbo].[GoodsSet]
ADD CONSTRAINT [FK_RentGoods]
    FOREIGN KEY ([Rent_Id])
    REFERENCES [dbo].[RentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RentGoods'
CREATE INDEX [IX_FK_RentGoods]
ON [dbo].[GoodsSet]
    ([Rent_Id]);
GO

-- Creating foreign key on [Rent_Id] in table 'VehiclesSet'
ALTER TABLE [dbo].[VehiclesSet]
ADD CONSTRAINT [FK_RentVehicles]
    FOREIGN KEY ([Rent_Id])
    REFERENCES [dbo].[RentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RentVehicles'
CREATE INDEX [IX_FK_RentVehicles]
ON [dbo].[VehiclesSet]
    ([Rent_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------