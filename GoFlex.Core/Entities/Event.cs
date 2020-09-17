using System;
using System.Collections.Generic;

namespace GoFlex.Core.Entities
{
    public class Event : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public byte[] Poster { get; set; }

        public int EventCategoryId { get; set; }
        public int LocationId { get; set; }
        public Guid OrganizerId { get; set; }

        public EventCategory EventCategory { get; set; }
        public Location Location { get; set; }
        public User Organizer { get; set; }
        public ICollection<EventPrice> Prices { get; set; }
    }
}
