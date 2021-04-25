USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_UserSelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_UserSelect] 
END 
GO
CREATE PROC [dbo].[usp_UserSelect] 
    @Id uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [UserId] as Id, [Email], [PasswordHash], [PasswordSalt], [Role] 
	FROM   [dbo].[User] 
	WHERE  ([UserId] = @Id OR @Id IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_UserInsert]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_UserInsert] 
END 
GO
CREATE PROC [dbo].[usp_UserInsert] 
    @Id uniqueidentifier,
    @Email varchar(100),
    @PasswordHash nvarchar(128),
    @PasswordSalt nvarchar(128),
    @Role varchar(10)
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[User] ([UserId], [Email], [PasswordHash], [PasswordSalt], [Role])
	SELECT @Id, @Email, @PasswordHash, @PasswordSalt, @Role
	

	SELECT [UserId] as Id, [Email], [PasswordHash], [PasswordSalt], [Role]
	FROM   [dbo].[User]
	WHERE  [UserId] = @Id
	
               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_UserSelectList]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_UserSelectList] 
END 
GO
CREATE PROC [dbo].[usp_UserSelectList] 
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [UserId] as Id, [Email], [PasswordHash], [PasswordSalt], [Role]
	FROM   [dbo].[User] 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_UserUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_UserUpdate] 
END 
GO
CREATE PROC [dbo].[usp_UserUpdate] 
    @Id uniqueidentifier,
    @Email varchar(100),
    @PasswordHash nvarchar(128),
    @PasswordSalt nvarchar(128),
    @Role varchar(10)
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	UPDATE [dbo].[User]
	SET    [Email] = @Email, [PasswordHash] = @PasswordHash, [PasswordSalt] = @PasswordSalt, [Role] = @Role
	WHERE  [UserId] = @Id
	
	
	SELECT [UserId] as Id, [Email], [PasswordHash], [PasswordSalt], [Role]
	FROM   [dbo].[User]
	WHERE  [UserId] = @Id	


	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_UserDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_UserDelete] 
END 
GO
CREATE PROC [dbo].[usp_UserDelete] 
    @Id uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[User]
	WHERE  [UserId] = @Id

	COMMIT
GO