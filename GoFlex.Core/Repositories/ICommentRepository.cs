using System;
using GoFlex.Core.Repositories.Abstractions;

namespace GoFlex.Core.Repositories
{
    public interface ICommentRepository : IRepository<Comment, Guid>
    {   
    }
}