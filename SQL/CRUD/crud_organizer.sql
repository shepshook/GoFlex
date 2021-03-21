USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_OrganizerSelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrganizerSelect] 
END 
GO
CREATE PROC [dbo].[usp_OrganizerSelect] 
    @UserId uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [UserId], [CompanyName], [BankAccountNumber] 
	FROM   [dbo].[Organizer] 
	WHERE  ([UserId] = @UserId OR @UserId IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_OrganizerInsert]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrganizerInsert] 
END 
GO
CREATE PROC [dbo].[usp_OrganizerInsert] 
    @UserId uniqueidentifier,
    @CompanyName nvarchar(128),
    @BankAccountNumber varchar(20)
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[Organizer] ([UserId], [CompanyName], [BankAccountNumber])
	SELECT @UserId, @CompanyName, @BankAccountNumber
	
	
	SELECT [UserId], [CompanyName], [BankAccountNumber]
	FROM   [dbo].[Organizer]
	WHERE  [UserId] = @UserId
	
               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_OrganizerUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrganizerUpdate] 
END 
GO
CREATE PROC [dbo].[usp_OrganizerUpdate] 
    @UserId uniqueidentifier,
    @CompanyName nvarchar(128),
    @BankAccountNumber varchar(20)
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	UPDATE [dbo].[Organizer]
	SET    [CompanyName] = @CompanyName, [BankAccountNumber] = @BankAccountNumber
	WHERE  [UserId] = @UserId

	SELECT [UserId], [CompanyName], [BankAccountNumber]
	FROM   [dbo].[Organizer]
	WHERE  [UserId] = @UserId	


	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_OrganizerDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrganizerDelete] 
END 
GO
CREATE PROC [dbo].[usp_OrganizerDelete] 
    @UserId uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[Organizer]
	WHERE  [UserId] = @UserId

	COMMIT
GO