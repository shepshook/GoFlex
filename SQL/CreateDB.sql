create table EventCategory
(
	EventCategoryId int identity
		constraint EventCategory_pk
			primary key nonclustered,
	Name varchar(64) not null
)
go

create unique index EventCategory_Name_uindex
	on EventCategory (Name)
go

create table Location
(
	LocationId int identity
		constraint Location_pk
			primary key nonclustered,
	Name nvarchar(256) not null,
	Address nvarchar(512) not null,
	PhoneNumber varchar(15) not null,
	Photo varbinary(max)
)
go

create table Role
(
	RoleId int identity
		constraint Role_pk
			primary key nonclustered,
	Name varchar(64) not null
)
go

create unique index Role_Name_uindex
	on Role (Name)
go

create table [User]
(
	UserId uniqueidentifier not null
		constraint User_pk
			primary key nonclustered,
	PhoneNumber varchar(15) not null,
	PasswordHash nvarchar(128) not null,
	PasswordSalt nvarchar(128) not null,
	RoleId int
		constraint User_Role_fk
			references Role
				on update cascade on delete set null
)
go

create table Event
(
	EventId int identity
		constraint Event_pk
			primary key nonclustered,
	EventCategoryId int not null
		constraint Event_EventCategory_EventCategoryId_fk
			references EventCategory,
	LocationId int
		constraint Event_Location_LocationId_fk
			references Location,
	OrganizerId uniqueidentifier not null
		constraint Event_User_UserId_fk
			references [User],
	Name nvarchar(128) not null,
	Description nvarchar(1024),
	DateTime datetime not null,
	Poster varbinary(max)
)
go

create table EventPrice
(
	EventPriceId int identity
		constraint EventPrice_pk
			primary key nonclustered,
	Name nvarchar(64) not null,
	Price money,
	EventId int not null
		constraint EventPrice_Event_EventId_fk
			references Event
				on update cascade on delete cascade,
	Total int not null
)
go

create table [Order]
(
	OrderId int identity
		constraint Order_pk
			primary key nonclustered,
	UserId uniqueidentifier not null
		constraint Order_User_UserId_fk
			references [User],
	EventId int not null
		constraint Order_Event_EventId_fk
			references Event,
	Timestamp datetime not null
)
go

create table OrderItem
(
	OrderId int not null
		constraint OrderItem_Order_OrderId_fk
			references [Order],
	EventPriceId int not null
		constraint OrderItem_EventPrice_EventPriceId_fk
			references EventPrice,
	Quantity int not null,
	constraint OrderItem_pk
		primary key nonclustered (OrderId, EventPriceId)
)
go

create unique index User_PhoneNumber_uindex
	on [User] (PhoneNumber)
go

