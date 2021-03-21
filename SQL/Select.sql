use DataModelling
go

----- 1. Shared pages for all roles
-- Logging in a user
select * from [dbo].[User]
    where Email = 'test@email.com'
      and PasswordHash = 'abcdefghijklmnopqrstuvwxyz'
go

-- List of events with filters
select * from [dbo].[Event] e
    join [dbo].[Category] c
        on e.CategoryId = c.CategoryId
    join [dbo].[Location] l
        on e.LocationId = l.LocationId
    where IsApproved = 1
        and Category = 'Concerts'
    order by [DateTime] desc
go

-- Get single event's info
select * from [dbo].[Event] e
    join [dbo].[Category] c
        on c.CategoryId = e.CategoryId
    join [dbo].[Location] l
        on l.LocationId = e.LocationId
    left join [dbo].[Comment] com
        on com.EventId = e.EventId
    left join [dbo].[Comment] com_child
        on com_child.ParentId = com.CommentId
    where e.EventId = 123
go

-- List of locations
select * from [dbo].[Location]
    order by [Name]
go


----- 2. Admin's specific page
-- List of event requests for admin to approve
select * from [dbo].[Event]
    join [dbo].[Category] c
        on c.CategoryId = e.CategoryId
    join [dbo].[Location] l
        on l.LocationId = e.LocationId
    where [IsApproved] = 0
    order by [CreateDate] desc
go


----- 3. Organizer's specific page
-- Get organizer's events
select * from [dbo].[Event]
    join [dbo].[Category] c
        on c.CategoryId = e.CategoryId
    join [dbo].[Location] l
        on l.LocationId = e.LocationId
    where e.OrganizerId = '1234-5678-9101-1121'
go
