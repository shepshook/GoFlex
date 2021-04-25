USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_TicketSelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_TicketSelect] 
END 
GO
CREATE PROC [dbo].[usp_TicketSelect] 
    @Id int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [TicketId] as Id, [Name], [Price], [EventId], [TotalCount], [IsRemoved]
	FROM   [dbo].[Ticket] 
	WHERE  ([TicketId] = @Id OR @Id IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_TicketSelectList]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_TicketSelectList] 
END 
GO
CREATE PROC [dbo].[usp_TicketSelectList] 
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [TicketId] as Id, [Name], [Price], [EventId], [TotalCount], [IsRemoved]
	FROM   [dbo].[Ticket] 

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
    @TotalCount int,
	@IsRemoved bit = 0
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[Ticket] ([Name], [Price], [EventId], [TotalCount], [IsRemoved])
	SELECT @Name, @Price, @EventId, @TotalCount, @IsRemoved
	
	
	SELECT [TicketId] as Id, [Name], [Price], [EventId], [TotalCount], [IsRemoved]
	FROM   [dbo].[Ticket]
	WHERE  [TicketId] = SCOPE_IDENTITY()

               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_TicketUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_TicketUpdate] 
END 
GO
CREATE PROC [dbo].[usp_TicketUpdate] 
    @Id int,
    @Name nvarchar(64),
    @Price money = NULL,
    @EventId int,
    @TotalCount int,
	@IsRemoved bit = 0
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	UPDATE [dbo].[Ticket]
	SET    [Name] = @Name, [Price] = @Price, [EventId] = @EventId, [TotalCount] = @TotalCount, [IsRemoved] = @IsRemoved
	WHERE  [TicketId] = @Id
	
	
	SELECT [TicketId] as Id, [Name], [Price], [EventId], [TotalCount], [IsRemoved]
	FROM   [dbo].[Ticket]
	WHERE  [TicketId] = @Id	


	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_TicketDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_TicketDelete] 
END 
GO
CREATE PROC [dbo].[usp_TicketDelete] 
    @Id int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[Ticket]
	WHERE  [TicketId] = @Id

	COMMIT
GO