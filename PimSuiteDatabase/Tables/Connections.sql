CREATE TABLE [dbo].[Connections]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[ConnectionId] NVARCHAR(255) NOT NULL, 
    [UserId] INT NOT NULL, 
    [FullName] NVARCHAR(200) NOT NULL, 
    CONSTRAINT [UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId])
)
