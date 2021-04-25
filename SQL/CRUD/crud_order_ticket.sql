USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_Order_TicketSelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_Order_TicketSelect] 
END 
GO
CREATE PROC [dbo].[usp_Order_TicketSelect] 
    @Id1 int,
    @Id2 int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [OrderId], [TicketId], [Quantity] 
	FROM   [dbo].[Order_Ticket] 
	WHERE  ([OrderId] = @Id1 OR @Id1 IS NULL) 
	       AND ([TicketId] = @Id2 OR @Id2 IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_Order_TicketSelectList]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_Order_TicketSelectList] 
END 
GO
CREATE PROC [dbo].[usp_Order_TicketSelectList] 
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [OrderId], [TicketId], [Quantity] 
	FROM   [dbo].[Order_Ticket] 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_Order_TicketInsert]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_Order_TicketInsert] 
END 
GO
CREATE PROC [dbo].[usp_Order_TicketInsert] 
    @OrderId int,
    @TicketId int,
    @Quantity int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[Order_Ticket] ([OrderId], [TicketId], [Quantity])
	SELECT @OrderId, @TicketId, @Quantity
	
	
	SELECT [OrderId], [TicketId], [Quantity]
	FROM   [dbo].[Order_Ticket]
	WHERE  [OrderId] = @OrderId
	       AND [TicketId] = @TicketId
               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_Order_TicketUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_Order_TicketUpdate] 
END 
GO
CREATE PROC [dbo].[usp_Order_TicketUpdate] 
    @OrderId int,
    @TicketId int,
    @Quantity int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	UPDATE [dbo].[Order_Ticket]
	SET    [Quantity] = @Quantity
	WHERE  [OrderId] = @OrderId
	       AND [TicketId] = @TicketId
	
	
	SELECT [OrderId] as Id, [TicketId], [Quantity]
	FROM   [dbo].[Order_Ticket]
	WHERE  [OrderId] = @OrderId
	       AND [TicketId] = @TicketId	
	

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_Order_TicketDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_Order_TicketDelete] 
END 
GO
CREATE PROC [dbo].[usp_Order_TicketDelete] 
    @Id1 int,
    @Id2 int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[Order_Ticket]
	WHERE  [OrderId] = @Id1
	       AND [TicketId] = @Id2

	COMMIT
GO