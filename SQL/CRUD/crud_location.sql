USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_LocationSelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_LocationSelect] 
END 
GO
CREATE PROC [dbo].[usp_LocationSelect] 
    @Id int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [LocationId] as Id, [Name], [Address], [PhoneNumber], [Photo] 
	FROM   [dbo].[Location] 
	WHERE  ([LocationId] = @Id OR @Id IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_LocationSelectList]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_LocationSelectList] 
END 
GO
CREATE PROC [dbo].[usp_LocationSelectList] 
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [LocationId] as Id, [Name], [Address], [PhoneNumber], [Photo] 
	FROM   [dbo].[Location] 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_LocationInsert]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_LocationInsert] 
END 
GO
CREATE PROC [dbo].[usp_LocationInsert] 
    @Name nvarchar(256),
    @Address nvarchar(512),
    @PhoneNumber varchar(15),
    @Photo varbinary(MAX) = NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[Location] ([Name], [Address], [PhoneNumber], [Photo])
	SELECT @Name, @Address, @PhoneNumber, @Photo
	
	SELECT [LocationId] as Id, [Name], [Address], [PhoneNumber], [Photo]
	FROM   [dbo].[Location]
	WHERE  [LocationId] = SCOPE_IDENTITY()
               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_LocationUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_LocationUpdate] 
END 
GO
CREATE PROC [dbo].[usp_LocationUpdate] 
    @Id int,
    @Name nvarchar(256),
    @Address nvarchar(512),
    @PhoneNumber varchar(15),
    @Photo varbinary(MAX) = NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	UPDATE [dbo].[Location]
	SET    [Name] = @Name, [Address] = @Address, [PhoneNumber] = @PhoneNumber, [Photo] = @Photo
	WHERE  [LocationId] = @Id
	

	SELECT [LocationId] as Id, [Name], [Address], [PhoneNumber], [Photo]
	FROM   [dbo].[Location]
	WHERE  [LocationId] = @Id	
	

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_LocationDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_LocationDelete] 
END 
GO
CREATE PROC [dbo].[usp_LocationDelete] 
    @Id int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[Location]
	WHERE  [LocationId] = @Id

	COMMIT
GO