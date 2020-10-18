using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;

namespace GoFlex.Core.Repositories
{
    public interface IEventRepository : IRepository<Event, int>, IPagedRepository<Event>
    {
    }
}
