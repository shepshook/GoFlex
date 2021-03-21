USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_UserSelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_UserSelect] 
END 
GO
CREATE PROC [dbo].[usp_UserSelect] 
    @UserId uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [UserId], [Email], [PasswordHash], [PasswordSalt], [Role] 
	FROM   [dbo].[User] 
	WHERE  ([UserId] = @UserId OR @UserId IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_UserInsert]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_UserInsert] 
END 
GO
CREATE PROC [dbo].[usp_UserInsert] 
    @UserId uniqueidentifier,
    @Email varchar(100),
    @PasswordHash nvarchar(128),
    @PasswordSalt nvarchar(128),
    @Role varchar(10)
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[User] ([UserId], [Email], [PasswordHash], [PasswordSalt], [Role])
	SELECT @UserId, @Email, @PasswordHash, @PasswordSalt, @Role
	

	SELECT [UserId], [Email], [PasswordHash], [PasswordSalt], [Role]
	FROM   [dbo].[User]
	WHERE  [UserId] = @UserId
	
               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_UserUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_UserUpdate] 
END 
GO
CREATE PROC [dbo].[usp_UserUpdate] 
    @UserId uniqueidentifier,
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
	WHERE  [UserId] = @UserId
	
	
	SELECT [UserId], [Email], [PasswordHash], [PasswordSalt], [Role]
	FROM   [dbo].[User]
	WHERE  [UserId] = @UserId	


	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_UserDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_UserDelete] 
END 
GO
CREATE PROC [dbo].[usp_UserDelete] 
    @UserId uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[User]
	WHERE  [UserId] = @UserId

	COMMIT
GO