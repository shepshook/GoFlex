using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoFlex.Core;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    public class OrganizerRepository : Repository<Organizer>, IOrganizerRepository
    {
        public OrganizerRepository(Database db, UnitOfWork uow) : base(db, uow)
        {
        }

        public async Task<Organizer> GetAsync(Guid key)
        {
            var entity = await _database.ReadEntityAsync<Organizer>("usp_OrganizerSelect", key);
            return (await MakeInclusionsAsync(new[] { entity })).Single();
        }

        public Task<IEnumerable<Organizer>> GetAllAsync(IDictionary<string, object> parameters = null)
        {
            return _database.ReadEntitiesAsync<Organizer>("usp_OrganizerSelectList", parameters);
        }

        public Task<Organizer> UpdateAsync(Organizer entity)
        {
            return _database.UpdateEntityAsync("usp_OrganizerUpdate", entity);
        }

        public async Task<Organizer> InsertAsync(Organizer entity)
        {
            if (entity.Id == default)
            {
                var guid = Guid.NewGuid();
                entity.User.Id = guid;
                await _unitOfWork.UserRepository.InsertAsync(entity.User);
                entity.Id = guid;
            }
            else if (await GetAsync(entity.Id) != null)
            {
                return await UpdateAsync(entity);
            }

            return await _database.CreateEntityAsync("usp_OrganizerInsert", entity);
        }

        public async Task RemoveAsync(Guid key)
        {
            await _database.RemoveEntityAsync("usp_OrganizerDelete", key);
        }

        private async Task<IEnumerable<Organizer>> MakeInclusionsAsync(IList<Organizer> entities)
        {
            foreach (var organizer in entities)
            {
                organizer.User = await _unitOfWork.UserRepository.GetAsync(organizer.Id);
            }
            return entities;
        }
    }
}