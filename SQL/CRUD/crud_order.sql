USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_OrderSelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrderSelect] 
END 
GO
CREATE PROC [dbo].[usp_OrderSelect] 
    @OrderId int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [OrderId], [UserId], [EventId], [Timestamp] 
	FROM   [dbo].[Order] 
	WHERE  ([OrderId] = @OrderId OR @OrderId IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_OrderInsert]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrderInsert] 
END 
GO
CREATE PROC [dbo].[usp_OrderInsert] 
    @UserId uniqueidentifier,
    @EventId int,
    @Timestamp datetime
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[Order] ([UserId], [EventId], [Timestamp])
	SELECT @UserId, @EventId, @Timestamp
	
	SELECT [OrderId], [UserId], [EventId], [Timestamp]
	FROM   [dbo].[Order]
	WHERE  [OrderId] = SCOPE_IDENTITY()
	
               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_OrderUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrderUpdate] 
END 
GO
CREATE PROC [dbo].[usp_OrderUpdate] 
    @OrderId int,
    @UserId uniqueidentifier,
    @EventId int,
    @Timestamp datetime
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	UPDATE [dbo].[Order]
	SET    [UserId] = @UserId, [EventId] = @EventId, [Timestamp] = @Timestamp
	WHERE  [OrderId] = @OrderId
	

	SELECT [OrderId], [UserId], [EventId], [Timestamp]
	FROM   [dbo].[Order]
	WHERE  [OrderId] = @OrderId	
	

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_OrderDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrderDelete] 
END 
GO
CREATE PROC [dbo].[usp_OrderDelete] 
    @OrderId int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[Order]
	WHERE  [OrderId] = @OrderId

	COMMIT
GO