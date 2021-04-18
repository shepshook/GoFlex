USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_CommentSelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CommentSelect] 
END 
GO
CREATE PROC [dbo].[usp_CommentSelect] 
    @CommentId uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [CommentId], [ParentId], [EventId], [Text] 
	FROM   [dbo].[Comment] 
	WHERE  ([CommentId] = @CommentId OR @CommentId IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_CommentInsert]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CommentInsert] 
END 
GO
CREATE PROC [dbo].[usp_CommentInsert] 
    @CommentId uniqueidentifier,
    @ParentId uniqueidentifier = NULL,
    @EventId int = NULL,
    @Text nvarchar(256)
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[Comment] ([CommentId], [ParentId], [EventId], [Text])
	SELECT @CommentId, @ParentId, @EventId, @Text
	

	SELECT [CommentId], [ParentId], [EventId], [Text]
	FROM   [dbo].[Comment]
	WHERE  [CommentId] = @CommentId

               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_CommentUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CommentUpdate] 
END 
GO
CREATE PROC [dbo].[usp_CommentUpdate] 
    @CommentId uniqueidentifier,
    @ParentId uniqueidentifier = NULL,
    @EventId int = NULL,
    @Text nvarchar(256)
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	UPDATE [dbo].[Comment]
	SET    [ParentId] = @ParentId, [EventId] = @EventId, [Text] = @Text
	WHERE  [CommentId] = @CommentId
	

	SELECT [CommentId], [ParentId], [EventId], [Text]
	FROM   [dbo].[Comment]
	WHERE  [CommentId] = @CommentId	


	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_CommentDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CommentDelete] 
END 
GO
CREATE PROC [dbo].[usp_CommentDelete] 
    @CommentId uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[Comment]
	WHERE  [CommentId] = @CommentId

	COMMIT
GO