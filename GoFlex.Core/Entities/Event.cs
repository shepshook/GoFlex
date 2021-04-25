using System;
using System.Collections.Generic;

namespace GoFlex.Core.Entities
{
    public class Event : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public string Photo { get; set; }
        public bool? IsApproved { get; set; }

        public int CategoryId { get; set; }
        public int LocationId { get; set; }
        public Guid OrganizerId { get; set; }

        public Category Category { get; set; }
        public Location Location { get; set; }
        public User Organizer { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public IList<Comment> RootComments { get; set; }

        public string ShortDate() => DateTime.ToString("d.MM.yyyy");
        public string ShortDateTime() => DateTime.ToString("dddd, d.MM.yyy, H:mm");
        public bool IsNew() => (DateTime.Now - CreateTime).Days <= 70;
    }
}
