using GoFlex.Core;

namespace GoFlex.Infrastructure
{
    public abstract class Repository<TEntity> where TEntity : Entity
    {
        protected readonly Database _database;
        protected readonly UnitOfWork _unitOfWork;

        protected Repository(Database database, UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _database = database;
        }
    }
}
