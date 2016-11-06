CREATE TABLE [dbo].[Connections]
(
	[ConnectionId] NVARCHAR(255) NOT NULL PRIMARY KEY, 
    [UserId] INT NOT NULL, 
    [FullName] NVARCHAR(200) NOT NULL, 
    CONSTRAINT [UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId])
)
