## Database Setup

Manually to set up the database table for logging, I just run the following SQL script:


CREATE TABLE [Logs] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Message] NVARCHAR(MAX) NULL,
    [MessageTemplate] NVARCHAR(MAX) NULL,
    [Level] NVARCHAR(MAX) NULL,
    [TimeStamp] DATETIME NULL,
    [Exception] NVARCHAR(MAX) NULL,
    [Properties] NVARCHAR(MAX) NULL,
    [UserName] NVARCHAR(50) NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([Id] ASC)
);
