USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_OrganizerSelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrganizerSelect] 
END 
GO
CREATE PROC [dbo].[usp_OrganizerSelect] 
    @Id uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [UserId] as Id, [CompanyName], [BankAccountNumber] 
	FROM   [dbo].[Organizer] 
	WHERE  ([UserId] = @Id OR @Id IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_OrganizerSelectList]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrganizerSelectList] 
END 
GO
CREATE PROC [dbo].[usp_OrganizerSelectList] 
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [UserId] as Id, [CompanyName], [BankAccountNumber] 
	FROM   [dbo].[Organizer]

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_OrganizerInsert]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrganizerInsert] 
END 
GO
CREATE PROC [dbo].[usp_OrganizerInsert] 
	@Id uniqueidentifier,
    @CompanyName nvarchar(128),
    @BankAccountNumber varchar(20) 
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[Organizer] ([UserId], [CompanyName], [BankAccountNumber])
	SELECT @Id, @CompanyName, @BankAccountNumber
	
	
	SELECT [UserId] as Id, [CompanyName], [BankAccountNumber]
	FROM   [dbo].[Organizer]
	WHERE  [UserId] = @Id
	
               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_OrganizerUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrganizerUpdate] 
END 
GO
CREATE PROC [dbo].[usp_OrganizerUpdate] 
    @Id uniqueidentifier,
    @CompanyName nvarchar(128),
    @BankAccountNumber varchar(20)
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	UPDATE [dbo].[Organizer]
	SET    [CompanyName] = @CompanyName, [BankAccountNumber] = @BankAccountNumber
	WHERE  [UserId] = @Id

	SELECT [UserId] as Id, [CompanyName], [BankAccountNumber]
	FROM   [dbo].[Organizer]
	WHERE  [UserId] = @Id	


	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_OrganizerDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_OrganizerDelete] 
END 
GO
CREATE PROC [dbo].[usp_OrganizerDelete] 
    @Id uniqueidentifier
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[Organizer]
	WHERE  [UserId] = @Id

	COMMIT
GO