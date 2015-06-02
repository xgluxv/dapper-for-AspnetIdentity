CREATE TABLE `IdentityUsers`
(
    `Id`               VARCHAR(56) NOT NULL,
    `UserName`             VARCHAR (50)            NOT NULL,
    `Email`                VARCHAR (100)          NULL,
    `EmailConfirmed`       BIT           NOT NULL,
    `PasswordHash`         VARCHAR (100)         NULL,
    `SecurityStamp`        VARCHAR (100)         NULL,
    `PhoneNumber`          VARCHAR (25)         NULL,
    `PhoneNumberConfirmed` BIT           NOT NULL,
    `TwoFactorEnabled`     BIT          NOT NULL,
    `LockoutEndDateUtc`    DATETIME               NULL,
    `LockoutEnabled`       BIT          NOT NULL,
    `AccessFailedCount`    INT                    NOT NULL,

    CONSTRAINT `PK_IdentityUsers_Id` PRIMARY KEY CLUSTERED (`Id` ASC),
    CONSTRAINT `UK_IdentityUsers_UserName` UNIQUE NONCLUSTERED (`UserName` ASC)
);

CREATE TABLE `IdentityRoles`
(
    `Id`  VARCHAR(56)          NOT NULL,
    `Name`   VARCHAR (50)  NOT NULL,

    CONSTRAINT `PK_IdentityRoles_Id` PRIMARY KEY CLUSTERED (`Id` ASC),
    CONSTRAINT `UK_IdentityRolse_Name` UNIQUE NONCLUSTERED (`Name` ASC)
);

CREATE TABLE `IdentityUserRoles`
(
	`Id`  VARCHAR(56)          NOT NULL,
    `UserId` VARCHAR(56) NOT NULL,
    `RoleId` VARCHAR(56) NOT NULL,
	CONSTRAINT `PK_IdentityUserRoles_Id` PRIMARY KEY CLUSTERED (`Id` ASC),
    CONSTRAINT `FK_IdentityUserRoles_User` FOREIGN KEY (`UserID`) REFERENCES `IdentityUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_IdentityUserRoles_UserRole` FOREIGN KEY (`RoleID`) REFERENCES `IdentityRoles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityUserClaims`
(
	`Id`  VARCHAR(56)          NOT NULL,
    `UserID`     VARCHAR(56)                   NOT NULL,
    `ClaimType`  TEXT       NULL,
    `ClaimValue` TEXT        NULL,

    CONSTRAINT `PK_IdentityUserClaims_Id` PRIMARY KEY CLUSTERED (`Id` ASC),
    CONSTRAINT `FK_IdentityUserClaims_User` FOREIGN KEY (`UserID`) REFERENCES `IdentityUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityUserLogins`
(
	`Id`  VARCHAR(56)          NOT NULL,
    `UserID`        VARCHAR(56)           NOT NULL,
    `LoginProvider` VARCHAR (128) NOT NULL,
    `ProviderKey`   VARCHAR (128) NOT NULL,
	CONSTRAINT `PK_IdentityUserLogins_Id` PRIMARY KEY CLUSTERED (`Id` ASC),
    CONSTRAINT `FK_IdentityUserLogins_User` FOREIGN KEY (`UserID`) REFERENCES `IdentityUsers` (`Id`) ON DELETE CASCADE
);


