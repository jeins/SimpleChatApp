CREATE TABLE [dbo].[Chats]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [UserId] INT NOT NULL, 
    [Message] NTEXT NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_UserId_User] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId])
)
