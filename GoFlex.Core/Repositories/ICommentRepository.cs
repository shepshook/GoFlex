using System;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;

namespace GoFlex.Core.Repositories
{
    public interface ICommentRepository : IRepository<Comment, Guid>
    {   
    }
}