USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_CategorySelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CategorySelect] 
END 
GO
CREATE PROC [dbo].[usp_CategorySelect] 
    @CategoryId int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [CategoryId], [Name] 
	FROM   [dbo].[Category] 
	WHERE  ([CategoryId] = @CategoryId OR @CategoryId IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_CategoryInsert]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CategoryInsert] 
END 
GO
CREATE PROC [dbo].[usp_CategoryInsert] 
    @Name nvarchar(32) = NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[Category] ([Name])
	SELECT @Name
	

	SELECT [CategoryId], [Name]
	FROM   [dbo].[Category]
	WHERE  [CategoryId] = SCOPE_IDENTITY()

               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_CategoryUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CategoryUpdate] 
END 
GO
CREATE PROC [dbo].[usp_CategoryUpdate] 
    @CategoryId int,
    @Name nvarchar(32) = NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	UPDATE [dbo].[Category]
	SET    [Name] = @Name
	WHERE  [CategoryId] = @CategoryId
	

	SELECT [CategoryId], [Name]
	FROM   [dbo].[Category]
	WHERE  [CategoryId] = @CategoryId	


	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_CategoryDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CategoryDelete] 
END 
GO
CREATE PROC [dbo].[usp_CategoryDelete] 
    @CategoryId int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[Category]
	WHERE  [CategoryId] = @CategoryId

	COMMIT
GO