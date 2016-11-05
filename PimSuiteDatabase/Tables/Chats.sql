CREATE TABLE [dbo].[Chats]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [UserId] INT NOT NULL, 
    [ChatToUserId] INT NOT NULL, 
    [Message] NTEXT NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_UserId_User] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]), 
    CONSTRAINT [FK_ChatToUserId_User] FOREIGN KEY ([ChatToUserId]) REFERENCES [Users]([UserId])
)
