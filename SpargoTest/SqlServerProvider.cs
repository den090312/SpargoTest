using System.Data.SqlClient;
using System.Reflection;
using System.Text;

using SpargoTest.Interfaces;

namespace SpargoTest
{
    /// <summary>
    /// Провайдер базы данных SQL Server
    /// </summary>
    public class SqlServerProvider : IDatabaseProvider
    {
        /// <summary>
        /// Строка подключения к базе данных SQL Server
        /// </summary>
        private readonly string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=SpargoTest;Trusted_Connection=True;";

        /// <summary>
        /// Конструктор провайдера базы данных SQL Server
        /// </summary>
        /// <param name="createTables">Создание таблиц</param>
        public SqlServerProvider(bool createTables = false)
        {
            if (createTables)
                CreateTables();
        }

        public bool ConnectionIsOk()
        {
            using var connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Записать объект в базу данных SQL Server
        /// </summary>
        /// <typeparam name="T">Тип записываемого объекта</typeparam>
        /// <param name="obj">Объект для записи</param>
        /// <param name="crudResult">Возможнеы ошибки при записи</param>
        public void Add<T>(T obj, out CrudResult crudResult)
        {
            var type = typeof(T);

            var properties = type.GetProperties().Where(p => p.Name != "Id");

            var fieldNames = properties.Select(p => p.Name);
            var parameterNames = fieldNames.Select(f => "@" + f);

            var commandText = $"INSERT INTO {type.Name} ({string.Join(", ", fieldNames)}) VALUES ({string.Join(", ", parameterNames)})";

            var parameters = new List<SqlParameter>();

            foreach (var property in properties)
                parameters.Add(new SqlParameter("@" + property.Name, property.GetValue(obj)));

            var rowsAffected = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using var command = new SqlCommand(commandText, connection);
                command.Parameters.AddRange(parameters.ToArray());

                rowsAffected = command.ExecuteNonQuery();
            }

            if (rowsAffected > 0)
                crudResult = new CrudResult(CrudOperation.Create);
            else
                crudResult = new CrudResult(CrudOperation.Create, "Ошибка при добавлении объекта в базу данных");
        }

        /// <summary>
        /// Получение объекта из базы данных SQL Server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id">Идентификатор объекта</param>
        /// <param name="crudResult"></param>
        /// <returns>Получаемый объект</returns>
        public T? Get<T>(int Id, out CrudResult crudResult)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand($"SELECT * FROM {typeof(T).Name} WHERE Id = @id", connection);
            command.Parameters.AddWithValue("@id", Id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var item = Activator.CreateInstance<T>();

                foreach (var property in typeof(T).GetProperties())
                {
                    var value = reader[property.Name];

                    if (value != DBNull.Value)
                        property.SetValue(item, value);
                }

                crudResult = new CrudResult(CrudOperation.Read);

                return item;
            }

            crudResult = new CrudResult(CrudOperation.Read, "Объект не найден в БД");

            return default;
        }

        /// <summary>
        /// Получить перечень всех объектов из базы данных SQL Server
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="crudResult">Возможные ошибки при получении объектов</param>
        /// <returns>Перечень объектов</returns>
        public IEnumerable<T> GetAll<T>(out CrudResult crudResult)
        {
            var result = new List<T>();
            
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            
            using var command = new SqlCommand($"SELECT * FROM {typeof(T).Name}", connection);
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
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                using var command = new SqlCommand(commandText, connection);
                command.Parameters.Add(new SqlParameter("@Id", Id));
                
                rowsAffected = command.ExecuteNonQuery();
            }

            if (rowsAffected > 0)
                crudResult = new CrudResult(CrudOperation.Delete);
            else
                crudResult = new CrudResult(CrudOperation.Delete, "Ошибка при удалении объекта из базы данных");
        }

        /// <summary>
        /// Создание таблиц БД, соотв. моделям из папки Program.ModelsFolderName
        /// </summary>
        private void CreateTables()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var modelsPath = Path.Combine(Program.BaseDirectory(), Program.ModelsFolderName);
            var modelFiles = Directory.GetFiles(modelsPath, "*.cs");

            var createTablesQuery = new StringBuilder();

            foreach (var modelFile in modelFiles)
            {
                var className = Path.GetFileNameWithoutExtension(modelFile);

                if (DatabaseExists(className))
                    continue;

                var classType = assembly
                    .GetTypes()
                    .FirstOrDefault(t => t.FullName != null
                        && t.FullName.Contains($"{Program.ModelsFolderName}.{className}"));

                if (classType == default)
                    continue;

                createTablesQuery.AppendLine($"CREATE TABLE {className} (");

                foreach (var property in classType.GetProperties())
                {
                    var columnName = property.Name;
                    var columnType = GetSqlType(property.PropertyType);

                    if (columnName == "Id")
                        createTablesQuery.AppendLine($"{columnName} {columnType} IDENTITY(1,1) PRIMARY KEY,");
                    else
                        createTablesQuery.AppendLine($"{columnName} {columnType},");
                }

                createTablesQuery.Length -= 3;
                createTablesQuery.AppendLine(");");
            }

            if (createTablesQuery.Length == 0)
                return;

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(createTablesQuery.ToString(), connection);
            command.ExecuteNonQuery();
        }

        private bool DatabaseExists(string tableName)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'", connection);
            var result = (int)command.ExecuteScalar();

            return result > 0 ? true : false;
        }

        private string GetSqlType(Type type)
        {
            var typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return "BIT";
                case TypeCode.Byte:
                    return "TINYINT";
                case TypeCode.Char:
                    return "NCHAR(1)";
                case TypeCode.Int16:
                    return "SMALLINT";
                case TypeCode.Int32:
                    return "INT";
                case TypeCode.Int64:
                    return "BIGINT";
                case TypeCode.SByte:
                    return "SMALLINT";
                case TypeCode.UInt16:
                    return "INT";
                case TypeCode.UInt32:
                    return "BIGINT";
                case TypeCode.UInt64:
                    return "DECIMAL(20)";
                case TypeCode.Single:
                    return "REAL";
                case TypeCode.Double:
                    return "FLOAT";
                case TypeCode.Decimal:
                    return "DECIMAL(18, 0)";
                case TypeCode.DateTime:
                    return "DATETIME2";
                case TypeCode.String:
                    return "NVARCHAR(MAX)";
                default:
                    if (type == typeof(byte[]))
                        return "VARBINARY(MAX)";
                    if (type == typeof(Guid))
                        return "UNIQUEIDENTIFIER";
                    if (type.IsEnum)
                        return "INT";

                    throw new NotSupportedException($"Тип {type} не поддерживается.");
            }
        }
    }
}


