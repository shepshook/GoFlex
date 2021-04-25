using System.Collections.Generic;
using System.Threading.Tasks;
using GoFlex.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace GoFlex.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner().GetAwaiter().GetResult();
        }

        public static async Task Runner()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"ConnectionStrings__DefaultConnection", "Server=localhost; Database=DataModelling; Integrated Security=true" }
                }).Build();

            var db = new Database(configuration);

            var uow = new UnitOfWork(db);

            //await uow.LocationRepository.InsertAsync(new Location {Address = "Address", Name = "New Location", PhoneNumber = "112345"});
            var x = await uow.LocationRepository.GetAsync(1);
        }
    }
}
