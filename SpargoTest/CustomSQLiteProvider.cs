using System.Reflection;
using System.Text;

using Microsoft.Data.Sqlite;

using SpargoTest.Interfaces;

namespace SpargoTest
{
    /// <summary>
    /// Провайдер базы данных SQLite
    /// </summary>
    public class CustomSQLiteProvider : IDatabaseProvider
    {
        /// <summary>
        /// Строка подключения к базе данных SQLite
        /// </summary>
        private readonly string _connectionString = "Data Source=spargo_test.db";

        /// <summary>
        /// Конструктор провайдера базы данных SQLite
        /// </summary>
        /// <param name="createTables">Создание таблиц</param>
        public CustomSQLiteProvider(bool createTables = false)
        {
            if (createTables)
                CreateTables();
        }

        /// <summary>
        /// Записать объект в базу данных SQLite
        /// </summary>
        /// <typeparam name="T">Тип записываемого объекта</typeparam>
        /// <param name="obj">Объект для записи</param>
        /// <param name="crudResult">Возможнеы ошибки при записи</param>
        public void Add<T>(T obj, out CrudResult crudResult)
        {
            var type = typeof(T);

            var properties = type.GetProperties();

            var fieldNames = properties.Select(p => p.Name);
            var parameterNames = fieldNames.Select(f => "@" + f);

            var commandText = $"INSERT INTO {type.Name} ({string.Join(", ", fieldNames)}) VALUES ({string.Join(", ", parameterNames)})";

            var parameters = new List<SqliteParameter>();

            foreach (var property in properties)
                parameters.Add(new SqliteParameter("@" + property.Name, property.GetValue(obj)));

            var rowsAffected = 0;

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                
                using var command = new SqliteCommand(commandText, connection);
                command.Parameters.AddRange(parameters.ToArray());

                rowsAffected = command.ExecuteNonQuery();
            }

            if (rowsAffected > 0)
                crudResult = new CrudResult(CrudOperation.Create);
            else
                crudResult = new CrudResult(CrudOperation.Create, "Ошибка при добавлении объекта в базу данных");
        }

        /// <summary>
        /// Получить перечень всех объектов из базы данных SQLite
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="crudResult">Возможные ошибки при получении объектов</param>
        /// <returns>Перечень объектов</returns>
        public IEnumerable<T> GetAll<T>(out CrudResult crudResult)
        {
            var result = new List<T>();
            
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            
            using var command = new SqliteCommand($"SELECT * FROM {typeof(T).Name}", connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                var item = Activator.CreateInstance<T>();

                foreach (var property in typeof(T).GetProperties())
                {
                    var value = reader[property.Name];

                    if (value != DBNull.Value)
                        property.SetValue(item, value);
                }

                result.Add(item);
            }

            crudResult = new CrudResult(CrudOperation.Read);
            
            return result;
        }

        public void Remove<T>(int Id, out CrudResult crudResult)
        {
            if (Id == default)
            {
                crudResult = new CrudResult(CrudOperation.Delete, "Некорректное значение идентификатора");
                
                return;
            }

            var commandText = $"DELETE FROM {typeof(T).Name} WHERE Id = @Id";
            
            var rowsAffected = 0;
            
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                
                using var command = new SqliteCommand(commandText, connection);
                command.Parameters.Add(new SqliteParameter("@Id", Id));
                
                rowsAffected = command.ExecuteNonQuery();
            }

            if (rowsAffected > 0)
                crudResult = new CrudResult(CrudOperation.Delete);
            else
                crudResult = new CrudResult(CrudOperation.Delete, "Ошибка при удалении объекта из базы данных");
        }

        /// <summary>
        /// Создание таблиц БД, соотв. моделям из папки Models
        /// </summary>
        private void CreateTables()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var modelsPath = Path.Combine(Program.BaseDirectory(), "Models");
            var modelFiles = Directory.GetFiles(modelsPath, "*.cs");

            var createTablesQuery = new StringBuilder();

            foreach (var modelFile in modelFiles)
            {
                var className = Path.GetFileNameWithoutExtension(modelFile);
                var classType = assembly
                    .GetTypes()
                    .FirstOrDefault(t => t.Name.Contains(className));

                if (classType == default)
                    continue;

                createTablesQuery.AppendLine($"CREATE TABLE IF NOT EXISTS {className} (");

                foreach (var property in classType.GetProperties())
                {
                    var columnName = property.Name;
                    var columnType = GetSQLiteType(property.PropertyType);

                    createTablesQuery.AppendLine($"{columnName} {columnType},");
                }

                createTablesQuery.Length -= 3;
                createTablesQuery.AppendLine(");");
            }

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var str = createTablesQuery.ToString();

            using var command = new SqliteCommand(str, connection);
            command.ExecuteNonQuery();
        }

        private string GetSQLiteType(Type type)
        {
            var typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return "INTEGER";
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "REAL";
                case TypeCode.DateTime:
                case TypeCode.String:
                    return "TEXT";
                default:
                    if (type == typeof(byte[]))
                        return "BLOB";
                    if (type == typeof(Guid))
                        return "TEXT";
                    if (type.IsEnum)
                        return "INTEGER";

                    throw new NotSupportedException($"Тип {type} не поддерживается.");
            }
        }
    }
}


