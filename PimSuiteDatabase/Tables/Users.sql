CREATE TABLE [dbo].[Users]
(
	[UserId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [FirstName] NVARCHAR(200) NOT NULL, 
    [LastName] NVARCHAR(200) NOT NULL, 
    [Email] NVARCHAR(200) NOT NULL, 
    [Password] NVARCHAR(200) NOT NULL, 
    [RoleId] INT NOT NULL, 
    CONSTRAINT [FK_RolesId_Role] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([RoleId])
)
