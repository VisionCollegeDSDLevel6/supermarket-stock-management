USE [master]
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'aspnet-SupermarketStockManagement-5f678e49-7dbc-459b-862b-656610d18216')
BEGIN
    ALTER DATABASE [aspnet-SupermarketStockManagement-5f678e49-7dbc-459b-862b-656610d18216] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE [aspnet-SupermarketStockManagement-5f678e49-7dbc-459b-862b-656610d18216]
END
GO
CREATE DATABASE [aspnet-SupermarketStockManagement-5f678e49-7dbc-459b-862b-656610d18216]
GO
USE [aspnet-SupermarketStockManagement-5f678e49-7dbc-459b-862b-656610d18216]
GO

CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC)
)
GO

CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
)
GO

CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([ProductId] ASC)
)
GO

CREATE TABLE [dbo].[Staff](
	[StaffId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[IdentityUserId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED ([StaffId] ASC)
)
GO

CREATE TABLE [dbo].[StockHistories](
	[StockHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[PreviousQuantity] [int] NOT NULL,
	[NewQuantity] [int] NOT NULL,
	[ChangeDate] [datetime2](7) NOT NULL,
	[ChangedBy] [nvarchar](450) NULL,
 CONSTRAINT [PK_StockHistories] PRIMARY KEY CLUSTERED ([StockHistoryId] ASC)
)
GO

CREATE TABLE [dbo].[Stocks](
	[StockId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[LowStockThreshold] [int] NOT NULL,
 CONSTRAINT [PK_Stocks] PRIMARY KEY CLUSTERED ([StockId] ASC)
)
GO

CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC)
)
GO

CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC)
)
GO

CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC)
)
GO

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'00000000000000_CreateIdentitySchema', N'10.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260831024309_InitialDatabase', N'10.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260831215612_AddStaffTable', N'10.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260907222109_AddIdentityUserIdToStaff', N'10.0.11')
GO

INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'04867400-a00e-4322-9bfe-3dbafdee639a', N'Staff', N'STAFF', N'ba85836b-634a-40dd-89fc-e02bc5dbd6fe')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'896c5985-9d26-4d28-ad00-07dd1b80a921', N'Manager', N'MANAGER', N'4ea5c9bb-8540-45c5-828e-6649438740b1')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'ad06f821-6fba-4eb3-b87b-43b212539b43', N'Admin', N'ADMIN', N'c6ce7bb8-b300-4bad-a31d-bc74b478887d')
GO

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'33aee913-be44-438c-ac52-f622abbd86ee', N'manager@stockflow.com', N'MANAGER@STOCKFLOW.COM', N'manager@stockflow.com', N'MANAGER@STOCKFLOW.COM', 1, N'AQAAAAIAAYagAAAAEOu20UXxnPOw9nNPsAPQ0YbQfmLsDeA0ZYSF8llv9UQchmgWn5C1fPy6Y1juewp3zw==', N'MCJ6UDTZR2RWYSXOWWEJKVIOLML2KCB5', N'de8b3953-1659-4005-9863-aa8f389a7537', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'606a86e5-3ee5-4a35-ae45-320ef0024e10', N'admin@stockflow.co.nz', N'ADMIN@STOCKFLOW.CO.NZ', N'admin@stockflow.co.nz', N'ADMIN@STOCKFLOW.CO.NZ', 1, N'AQAAAAIAAYagAAAAEPOPJscvNgSHOKVG5uoMkP73pc7r7kBrHEbn1zYUYgzv+5oumonvspMqW9zK6mKCKQ==', N'PEQNGUPBMPI5D55BFJ6CW3KJONNJQTCF', N'cfad7231-a467-4f26-a20a-4c3137eaf2e8', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'a69bfa6f-ae47-40dd-b984-d5b4604e5295', N'ngthanh123426@gmail.com', N'NGTHANH123426@GMAIL.COM', N'ngthanh123426@gmail.com', N'NGTHANH123426@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAENE5ron169NThHFaYTyuRxgRMey9M8MQieKzzibDYEGBlqA96azD4MD4N+M3c2wU5g==', N'OEQD4QINIFY3EB3YZLBNYGUIJWBXBUGM', N'd4198f71-48e5-4a02-adf6-ebdb8b84ad27', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'de368360-b28a-4520-94e9-dcae2e205828', N'staff@stockflow.com', N'STAFF@STOCKFLOW.COM', N'staff@stockflow.com', N'STAFF@STOCKFLOW.COM', 1, N'AQAAAAIAAYagAAAAEOQ0azJlPyqoJshOBKWuwu/TDe6ZimQww4JcCr7YhsylGcCVfMXuGQ6lwkCYmJf0sA==', N'VXNCONYOYD2ZSWZWF23PZVQNNKXQUTF4', N'4da7e01a-e075-44c9-bf0f-2a16a99c5a28', NULL, 0, 0, NULL, 1, 0)
GO

INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'de368360-b28a-4520-94e9-dcae2e205828', N'04867400-a00e-4322-9bfe-3dbafdee639a')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'33aee913-be44-438c-ac52-f622abbd86ee', N'896c5985-9d26-4d28-ad00-07dd1b80a921')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'606a86e5-3ee5-4a35-ae45-320ef0024e10', N'ad06f821-6fba-4eb3-b87b-43b212539b43')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'a69bfa6f-ae47-40dd-b984-d5b4604e5295', N'ad06f821-6fba-4eb3-b87b-43b212539b43')
GO

SET IDENTITY_INSERT [dbo].[Categories] ON 
INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (1, N'Test1', N'123')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO

SET IDENTITY_INSERT [dbo].[Products] ON 
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [Price], [ImageUrl], [CategoryId]) VALUES (1, N'Test', NULL, CAST(1.00 AS Decimal(10, 2)), N'https://th.bing.com/th/id/OIP.zJcerO1OR1nSb_UEkRx_aAHaHa?w=200&h=200&o=6&pid=Dictionary', 1)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO

SET IDENTITY_INSERT [dbo].[Staff] ON 
INSERT [dbo].[Staff] ([StaffId], [Name], [Email], [Role], [IdentityUserId]) VALUES (1, N'P', N'test@test.com', N'Administrator', NULL)
INSERT [dbo].[Staff] ([StaffId], [Name], [Email], [Role], [IdentityUserId]) VALUES (2, N'Manager', N'manager@stockflow.com', N'Manager', N'33aee913-be44-438c-ac52-f622abbd86ee')
INSERT [dbo].[Staff] ([StaffId], [Name], [Email], [Role], [IdentityUserId]) VALUES (4, N'Staff', N'staff@stockflow.com', N'Staff', N'de368360-b28a-4520-94e9-dcae2e205828')
SET IDENTITY_INSERT [dbo].[Staff] OFF
GO

SET IDENTITY_INSERT [dbo].[Stocks] ON 
INSERT [dbo].[Stocks] ([StockId], [ProductId], [Quantity], [LowStockThreshold]) VALUES (1, 1, 2, 1)
SET IDENTITY_INSERT [dbo].[Stocks] OFF
GO

ALTER TABLE [dbo].[AspNetRoleClaims] WITH CHECK ADD CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserClaims] WITH CHECK ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserLogins] WITH CHECK ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserRoles] WITH CHECK ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserRoles] WITH CHECK ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[AspNetUserTokens] WITH CHECK ADD CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Products] WITH CHECK ADD CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY([CategoryId]) REFERENCES [dbo].[Categories] ([CategoryId]) ON DELETE CASCADE
ALTER TABLE [dbo].[StockHistories] WITH CHECK ADD CONSTRAINT [FK_StockHistories_Products_ProductId] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([ProductId]) ON DELETE CASCADE
ALTER TABLE [dbo].[Stocks] WITH CHECK ADD CONSTRAINT [FK_Stocks_Products_ProductId] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([ProductId]) ON DELETE CASCADE
GO