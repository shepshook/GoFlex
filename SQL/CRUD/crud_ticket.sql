USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_TicketSelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_TicketSelect] 
END 
GO
CREATE PROC [dbo].[usp_TicketSelect] 
    @EventPriceId int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [EventPriceId], [Name], [Price], [EventId], [TotalCount] 
	FROM   [dbo].[Ticket] 
	WHERE  ([EventPriceId] = @EventPriceId OR @EventPriceId IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_TicketInsert]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_TicketInsert] 
END 
GO
CREATE PROC [dbo].[usp_TicketInsert] 
    @Name nvarchar(64),
    @Price money = NULL,
    @EventId int,
    @TotalCount int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[Ticket] ([Name], [Price], [EventId], [TotalCount])
	SELECT @Name, @Price, @EventId, @TotalCount
	
	
	SELECT [EventPriceId], [Name], [Price], [EventId], [TotalCount]
	FROM   [dbo].[Ticket]
	WHERE  [EventPriceId] = SCOPE_IDENTITY()

               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_TicketUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_TicketUpdate] 
END 
GO
CREATE PROC [dbo].[usp_TicketUpdate] 
    @EventPriceId int,
    @Name nvarchar(64),
    @Price money = NULL,
    @EventId int,
    @TotalCount int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	UPDATE [dbo].[Ticket]
	SET    [Name] = @Name, [Price] = @Price, [EventId] = @EventId, [TotalCount] = @TotalCount
	WHERE  [EventPriceId] = @EventPriceId
	
	
	SELECT [EventPriceId], [Name], [Price], [EventId], [TotalCount]
	FROM   [dbo].[Ticket]
	WHERE  [EventPriceId] = @EventPriceId	


	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_TicketDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_TicketDelete] 
END 
GO
CREATE PROC [dbo].[usp_TicketDelete] 
    @EventPriceId int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[Ticket]
	WHERE  [EventPriceId] = @EventPriceId

	COMMIT
GO