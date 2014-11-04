CREATE TABLE [dbo].[IdentityUsers]
(
    [Id]               NVARCHAR(56) NOT NULL,
    [UserName]             NVARCHAR (50)            NOT NULL,
    [Email]                NVARCHAR (100)          NULL,
    [EmailConfirmed]       BIT           NOT NULL,
    [PasswordHash]         NVARCHAR (100)         NULL,
    [SecurityStamp]        NVARCHAR (100)         NULL,
    [PhoneNumber]          NVARCHAR (25)         NULL,
    [PhoneNumberConfirmed] BIT           NOT NULL,
    [TwoFactorEnabled]     BIT          NOT NULL,
    [LockoutEndDateUtc]    DATETIME               NULL,
    [LockoutEnabled]       BIT          NOT NULL,
    [AccessFailedCount]    INT                    NOT NULL,

    CONSTRAINT [PK_IdentityUsers_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_IdentityUsers_UserName] UNIQUE NONCLUSTERED ([UserName] ASC)
);

CREATE TABLE [dbo].[IdentityRoles]
(
    [Id]  NVARCHAR(56)          NOT NULL,
    [Name]   NVARCHAR (50)  NOT NULL,

    CONSTRAINT [PK_IdentityRoles_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_IdentityRolse_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

CREATE TABLE [dbo].[IdentityUserRoles]
(
	[Id]  NVARCHAR(56)          NOT NULL,
    [UserId] NVARCHAR(56) NOT NULL,
    [RoleId] NVARCHAR(56) NOT NULL,
	CONSTRAINT [PK_IdentityUserRoles_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_IdentityUserRoles_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[IdentityUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_IdentityUserRoles_UserRole] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[IdentityRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[IdentityUserClaims]
(
	[Id]  NVARCHAR(56)          NOT NULL,
    [UserID]     NVARCHAR(56)                   NOT NULL,
    [ClaimType]  NVARCHAR (MAX)        NULL,
    [ClaimValue] NVARCHAR (MAX)        NULL,

    CONSTRAINT [PK_IdentityUserClaims_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_IdentityUserClaims_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[IdentityUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[IdentityUserLogins]
(
	[Id]  NVARCHAR(56)          NOT NULL,
    [UserID]        NVARCHAR(56)           NOT NULL,
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
	CONSTRAINT [PK_IdentityUserLogins_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_IdentityUserLogins_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[IdentityUsers] ([Id]) ON DELETE CASCADE
);


