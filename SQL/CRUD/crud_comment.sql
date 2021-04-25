USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_CommentSelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CommentSelect] 
END 
GO
CREATE PROC [dbo].[usp_CommentSelect] 
    @Id uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [CommentId] as Id, [ParentId], [EventId], [UserId], [Text] 
	FROM   [dbo].[Comment] 
	WHERE  ([CommentId] = @Id OR @Id IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_CommentSelectList]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CommentSelectList] 
END 
GO
CREATE PROC [dbo].[usp_CommentSelectList] 
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [CommentId] as Id, [ParentId], [EventId], [UserId], [Text] 
	FROM   [dbo].[Comment]

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_CommentInsert]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CommentInsert] 
END 
GO
CREATE PROC [dbo].[usp_CommentInsert] 
    @Id uniqueidentifier,
    @ParentId uniqueidentifier = NULL,
    @EventId int = NULL,
	@UserId uniqueidentifier,
    @Text nvarchar(256)
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[Comment] ([CommentId], [ParentId], [EventId], [UserId], [Text])
	SELECT @Id, @ParentId, @EventId, @UserId, @Text
	

	SELECT [CommentId] as Id, [ParentId], [EventId], [UserId], [Text]
	FROM   [dbo].[Comment]
	WHERE  [CommentId] = @Id

               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_CommentUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CommentUpdate] 
END 
GO
CREATE PROC [dbo].[usp_CommentUpdate] 
    @Id uniqueidentifier,
    @ParentId uniqueidentifier = NULL,
    @EventId int = NULL,
	@UserId uniqueidentifier,
    @Text nvarchar(256)
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	UPDATE [dbo].[Comment]
	SET    [ParentId] = @ParentId, [EventId] = @EventId, [UserId] = @UserId, [Text] = @Text
	WHERE  [CommentId] = @Id
	

	SELECT [CommentId] as Id, [ParentId], [EventId], [UserId], [Text]
	FROM   [dbo].[Comment]
	WHERE  [CommentId] = @Id	


	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_CommentDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_CommentDelete] 
END 
GO
CREATE PROC [dbo].[usp_CommentDelete] 
    @Id uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[Comment]
	WHERE  [CommentId] = @Id

	COMMIT
GO