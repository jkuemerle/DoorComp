CREATE TABLE Doors (DoorID nvarchar(20) NOT NULL, Location nvarchar(200), Description nvarchar(200) NULL)
GO

CREATE TABLE Events (Code nvarchar(50) NOT NULL, Description nvarchar(200) NULL, Status int NOT NULL, LogoURL nvarchar(500))
GO

CREATE TABLE Votes (DoorID nvarchar(20), VoterID nvarchar(500))
GO

CREATE TABLE Claims (DoorID nvarchar(20), Name nvarchar(100), Email nvarchar(100))
GO