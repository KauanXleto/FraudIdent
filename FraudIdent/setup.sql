IF DB_ID('FraudIdent') IS NULL
begin
	CREATE DATABASE FraudIdent;
end
GO
USE FraudIdent;
go

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Truck]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Truck](
		[Id] [int] IDENTITY(1,1) primary key NOT NULL,
		[Name] [varchar](500) NULL

	)
END
go


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TruckParam]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[TruckParam](
		[Id] [int] IDENTITY(1,1) primary key NOT NULL,
		[TruckId] [int] NULL,
		[Height] [decimal](18, 9) NULL,
		[Length] [decimal](18, 9) NULL,
		[Width] [decimal](18, 9) NULL,

		FOREIGN KEY([TruckId]) REFERENCES [dbo].[Truck] ([Id])
	)
END
go

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LobInfo]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[LobInfo](
		[Id] [int] IDENTITY(1,1) primary key NOT NULL,
		[TruckId] [int] NULL,
		[CreateDate] [datetime] NULL,
		[HasError] [bit] NULL,
		[HasSuccess] [bit] NULL,
		[IsDistanceImage] [bit] NULL,		
		[MessageError] [varchar](max) NULL,
		[BackImageTruck] [varchar](max) NULL,
		[SideImageTruck] [varchar](max) NULL,

		FOREIGN KEY([TruckId]) REFERENCES [dbo].[Truck] ([Id])
	)
END
go

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BalanceInfo]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[BalanceInfo](
		[Id] [int] IDENTITY(1,1) primary key NOT NULL,
		[Length] [decimal](18, 9) NULL,
		[Width] [decimal](18, 9) NULL,
		[Height] [decimal](18, 9) NULL,
		[DistanceScaleCam1] [decimal](18, 9) NULL,
		[DistanceScaleCam2] [decimal](18, 9) NULL
	)

	insert into [BalanceInfo](Length, Width, DistanceScaleCam1, DistanceScaleCam2)
					   values(50, 40, 100, 200)
END

