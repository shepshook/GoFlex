USE [DataModelling];
GO

IF OBJECT_ID('[dbo].[usp_EventSelect]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_EventSelect] 
END 
GO
CREATE PROC [dbo].[usp_EventSelect] 
    @Id int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [EventId] as Id, [CategoryId], [LocationId], [OrganizerId], [Name], [Description], [DateTime], [CreateTime], [IsApproved], [Photo] 
	FROM   [dbo].[Event] 
	WHERE  ([EventId] = @Id OR @Id IS NULL) 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_EventSelectList]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_EventSelectList] 
END 
GO
CREATE PROC [dbo].[usp_EventSelectList] 
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRAN

	SELECT [EventId] as Id, [CategoryId], [LocationId], [OrganizerId], [Name], [Description], [DateTime], [CreateTime], [IsApproved], [Photo] 
	FROM   [dbo].[Event] 

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_EventInsert]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_EventInsert] 
END 
GO
CREATE PROC [dbo].[usp_EventInsert] 
    @CategoryId int,
    @LocationId int = NULL,
    @OrganizerId uniqueidentifier,
    @Name nvarchar(128),
    @Description nvarchar(1024) = NULL,
    @DateTime datetime,
    @CreateTime datetime,
    @IsApproved bit = NULL,
    @Photo nvarchar(MAX) = NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN
	
	INSERT INTO [dbo].[Event] ([CategoryId], [LocationId], [OrganizerId], [Name], [Description], [DateTime], [CreateTime], [IsApproved], [Photo])
	SELECT @CategoryId, @LocationId, @OrganizerId, @Name, @Description, @DateTime, @CreateTime, @IsApproved, @Photo
	

	SELECT [EventId] as Id, [CategoryId], [LocationId], [OrganizerId], [Name], [Description], [DateTime], [CreateTime], [IsApproved], [Photo]
	FROM   [dbo].[Event]
	WHERE  [EventId] = SCOPE_IDENTITY()
               
	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_EventUpdate]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_EventUpdate] 
END 
GO
CREATE PROC [dbo].[usp_EventUpdate] 
    @Id int,
    @CategoryId int,
    @LocationId int = NULL,
    @OrganizerId uniqueidentifier,
    @Name nvarchar(128),
    @Description nvarchar(1024) = NULL,
    @DateTime datetime,
    @CreateTime datetime,
    @IsApproved bit = NULL,
    @Photo nvarchar(MAX) = NULL
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	UPDATE [dbo].[Event]
	SET    [CategoryId] = @CategoryId, [LocationId] = @LocationId, [OrganizerId] = @OrganizerId, [Name] = @Name, [Description] = @Description, [DateTime] = @DateTime, [CreateTime] = @CreateTime, [IsApproved] = @IsApproved, [Photo] = @Photo
	WHERE  [EventId] = @Id
	

	SELECT [EventId] as Id, [CategoryId], [LocationId], [OrganizerId], [Name], [Description], [DateTime], [CreateTime], [IsApproved], [Photo]
	FROM   [dbo].[Event]
	WHERE  [EventId] = @Id	

	COMMIT
GO
IF OBJECT_ID('[dbo].[usp_EventDelete]') IS NOT NULL
BEGIN 
    DROP PROC [dbo].[usp_EventDelete] 
END 
GO
CREATE PROC [dbo].[usp_EventDelete] 
    @Id int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRAN

	DELETE
	FROM   [dbo].[Event]
	WHERE  [EventId] = @Id

	COMMIT
GO