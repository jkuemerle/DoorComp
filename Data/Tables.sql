CREATE TABLE Doors (ID bigint NOT NULL IDENTITY(1,1), DoorID nvarchar(20) NOT NULL, Location nvarchar(200), Description nvarchar(200) NULL)
GO

CREATE TABLE Events (ID bigint NOT NULL IDENTITY(1,1), Code nvarchar(50) NOT NULL, Description nvarchar(200) NULL, Status int NOT NULL, LogoURL nvarchar(500))
GO

CREATE TABLE Votes (ID bigint NOT NULL IDENTITY(1,1), DoorID nvarchar(20), VoterID nvarchar(500))
GO

CREATE TABLE Claims (ID bigint NOT NULL IDENTITY(1,1), DoorID nvarchar(20), Name nvarchar(100), Email nvarchar(100))
GO