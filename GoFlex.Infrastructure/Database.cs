using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GoFlex.Infrastructure
{
    public class Database
    {
        private readonly string _connectionString;

        public Database(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<T> ReadEntityAsync<T>(string storedProcedure, object id)
        {
            await using var connection = new SqlConnection(_connectionString);
            var command = BuildStoredProcedure(storedProcedure, connection, GetIdAsDictionary(id));

            connection.Open();
            var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                return default;

            var columns = (await reader.GetColumnSchemaAsync()).Select(t => t.ColumnName).ToList();

            reader.Read();

            return LoadEntity<T>(reader, columns);
        }

        public async Task<IEnumerable<T>> ReadEntitiesAsync<T>(string storedProcedure, IDictionary<string, object> parameters = null)
        {
            var list = new List<T>();

            await using var connection = new SqlConnection(_connectionString);
            var command = BuildStoredProcedure(storedProcedure, connection, parameters);

            connection.Open();
            var reader = await command.ExecuteReaderAsync();

            var columns = (await reader.GetColumnSchemaAsync()).Select(t => t.ColumnName).ToList();

            while (reader.Read())
            {
                list.Add(LoadEntity<T>(reader, columns));
            }

            return list;
        }

        public async Task<T> CreateEntityAsync<T>(string storedProcedure, T entity)
        {
            var dict = entity.ToDictionary();

            await using var connection = new SqlConnection(_connectionString);
            var command = BuildStoredProcedure(storedProcedure, connection, dict);

            connection.Open();
            var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                return default;

            var columns = (await reader.GetColumnSchemaAsync()).Select(t => t.ColumnName).ToList();

            reader.Read();

            return LoadEntity<T>(reader, columns);
        }

        public Task<T> UpdateEntityAsync<T>(string storedProcedure, T entity)
        {
            return CreateEntityAsync(storedProcedure, entity);
        }

        public async Task RemoveEntityAsync(string storedProcedure, object id)
        {
            await using var connection = new SqlConnection(_connectionString);
            var command = BuildStoredProcedure(storedProcedure, connection, GetIdAsDictionary(id));

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        private static T LoadEntity<T>(SqlDataReader reader, IList<string> columns)
        {
            var entity = Activator.CreateInstance<T>();
            foreach (var column in columns)
            {
                var value = reader.GetValue(column);
                var prop = typeof(T).GetProperty(column)
                    ?? throw new ArgumentException($"Property {column} was not found");

                prop.SetValue(entity, ResolveValue(value, prop.PropertyType));
            }

            return entity;
        }

        private static Dictionary<string, object> GetIdAsDictionary(object id)
        {
            if (id is not ITuple tuple) 
                return new Dictionary<string, object> {{"Id", id}};

            var dict = new Dictionary<string, object>();
            for (var i = 0; i < tuple.Length; i++)
            {
                dict[$"Id{i + 1}"] = tuple[i];
            };

            return dict;
        }

        private static object ResolveValue(object dbValue, Type propertyType)
        {
            if (dbValue == DBNull.Value || dbValue == null)
                return null;

            if (propertyType.IsEnum)
                return Enum.Parse(propertyType, dbValue.ToString() ?? string.Empty);

            return dbValue;
        }

        private static SqlCommand BuildStoredProcedure(string procedureName, SqlConnection connection, IDictionary<string, object> parameters)
        {
            var command = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters == null || !parameters.Any())
                return command;

            foreach (var (key, value) in parameters)
            {
                var stringValue = value switch
                {
                    string => value.ToString(),
                    int => value,
                    IEnumerable<Guid> sequence => string.Join(",", sequence),
                    IEnumerable<object> sequence => string.Join(",", sequence),
                    _ => value?.ToString()
                };
                command.Parameters.AddWithValue($"@{key}", stringValue);
            }

            return command;
        }
    }
}