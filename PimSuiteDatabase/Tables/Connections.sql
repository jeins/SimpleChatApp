CREATE TABLE [dbo].[Connections]
(
	[ConnectionId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [UserId] INT NOT NULL, 
    [FullName] NVARCHAR(200) NOT NULL, 
    CONSTRAINT [UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId])
)
