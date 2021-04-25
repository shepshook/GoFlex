using System;
using System.Collections.Generic;

namespace GoFlex.Core.Entities
{
    public class Comment : Entity<Guid>
    {
        public Guid? ParentId { get; set; }

        public int EventId { get; set; }

        public Guid UserId { get; set; }

        public string Text { get; set; }


        public Comment Parent { get; set; }

        public IList<Comment> Children { get; set; }
    }
}