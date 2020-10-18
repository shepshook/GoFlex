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

        public int EventCategoryId { get; set; }
        public int LocationId { get; set; }
        public Guid OrganizerId { get; set; }

        public virtual EventCategory EventCategory { get; set; }
        public virtual Location Location { get; set; }
        public virtual User Organizer { get; set; }
        public virtual ICollection<EventPrice> Prices { get; set; }

        public string ShortDate => DateTime.ToString("d.MM.yyyy");
        public string ShortDateTime => DateTime.ToString("dddd, d.MM.yyy, H:mm");
        public bool IsNew => (DateTime.Now - CreateTime).Days <= 70;
    }
}
