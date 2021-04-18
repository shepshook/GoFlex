using System;

namespace GoFlex.Core
{
    public class Comment : Entity<Guid>
    {
        public Guid ParentId { get; set; }

        public int EventId { get; set; }

        public string Text { get; set; }
    }
}