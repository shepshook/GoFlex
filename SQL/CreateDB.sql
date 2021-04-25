if not exists (select * from sys.databases where [Name] = 'DataModelling')
begin
	create database [DataModelling]
end
go

use DataModelling
go

if object_id('[dbo].[Location]') is not null
	drop table [Location]
go

create table [Location]
(
	LocationId int identity
		constraint Location_pk
			primary key,
	[Name] nvarchar(256) not null,
	[Address] nvarchar(512) not null,
	PhoneNumber varchar(15) not null,
	Photo varbinary(max)
)
go


if object_id('[dbo].[User]') is not null
	drop table [User]
go

create table [User]
(
	UserId uniqueidentifier not null
		constraint User_pk
			primary key,
	Email varchar(100) not null,
	PasswordHash nvarchar(128) not null,
	PasswordSalt nvarchar(128) not null,
	[Role] varchar(10) not null
		check ([Role] in ('Admin', 'Customer', 'Organizer'))
)
go


if object_id('[dbo].[Organizer]') is not null
	drop table [Organizer]
go

create table [Organizer]
(
	UserId uniqueidentifier not null
		constraint Organizer_pk
			primary key
		constraint Organizer_User_UserId_fk
			references [User],
	CompanyName nvarchar(128) not null,
	BankAccountNumber varchar(20) not null
)


if object_id('[dbo].[Category]') is not null
	drop table [Category]
go

create table [Category]
(
	CategoryId int identity
		constraint Category_pk
			primary key,
	[Name] nvarchar(32)
)


if object_id('[dbo].[Event]') is not null
	drop table [Event]
go

create table [Event]
(
	EventId int identity
		constraint Event_pk
			primary key,
	CategoryId int not null
		constraint Event_Category_CategoryId_fk
			references [Category],
	LocationId int
		constraint Event_Location_LocationId_fk
			references Location,
	OrganizerId uniqueidentifier not null
		constraint Event_Organizer_UserId_fk
			references [Organizer],
	[Name] nvarchar(128) not null,
	[Description] nvarchar(1024),
	[DateTime] datetime not null,
	[CreateTime] datetime not null 
		default getdate(),
	IsApproved bit,
	Photo varbinary(max)
)
go


if object_id('[dbo].[Comment]') is not null
	drop table [Comment]
go

create table [Comment]
(
	CommentId uniqueidentifier not null
		constraint Comment_pk
			primary key,
	ParentId uniqueidentifier
		constraint Comment_ParentId_fk
			references [Comment],
	EventId int
		constraint Comment_Event_EventId_fk
			references [Event]
			on delete cascade,
	UserId uniqueidentifier
		constraint Comment_User_UserId_fk
			references [User],
	[Text] nvarchar(256) not null,
	constraint CHK_CommentHasParent 
		check (ParentId is not null or EventId is not null)
)


if object_id('[dbo].[Ticket]') is not null
	drop table [Ticket]
go

create table Ticket
(
	TicketId int identity
		constraint Ticket_pk
			primary key,
	[Name] nvarchar(64) not null,
	Price money,
	IsRemoved bit not null default 0,
	EventId int not null
		constraint Ticket_Event_EventId_fk
			references Event
				on update cascade on delete cascade,
	TotalCount int not null
)
go


if object_id('[dbo].[Order]') is not null
	drop table [Order]
go

create table [Order]
(
	OrderId int identity
		constraint Order_pk
			primary key,
	UserId uniqueidentifier not null
		constraint Order_User_UserId_fk
			references [User],
	EventId int not null
		constraint Order_Event_EventId_fk
			references Event,
	[Timestamp] datetime not null
)
go


if object_id('[dbo].[Order_Ticket]') is not null
	drop table [Order_Ticket]
go

create table Order_Ticket
(
	OrderId int not null
		constraint Order_Ticket_OrderId
			references [Order],
	TicketId int not null
		constraint Order_Ticket_TicketId
			references Ticket,
	Quantity int not null,
	constraint OrderItem_pk
		primary key (OrderId, TicketId)
)
go
