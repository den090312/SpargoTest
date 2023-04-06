using System.Data.SqlClient;
using System.Reflection;
using System.Text;

using SpargoTest.Interfaces;
using SpargoTest.Services;

namespace SpargoTest.Repository
{
    /// <summary>
    /// Провайдер базы данных SQL Server Express
    /// </summary>
    public class SqlExpressProvider : IDatabaseProvider
    {
        /// <summary>
        /// Строка подключения к серверу
        /// </summary>
        private string _serverConnectionString = "Server=(localdb)\\mssqllocaldb;Trusted_Connection=True;";

        /// <summary>
        /// Имя базы данных
        /// </summary>
        private string _databaseName = "SpargoTest";

        /// <summary>
        /// Строка подключения к базе данных SQL Server Express
        /// </summary>
        public string DatabaseConnectionString => $"{_serverConnectionString}Database={_databaseName};";

        /// <summary>
        /// Полное имя файла базы данных
        /// </summary>
        public string DatabaseFileName => Path.Combine(Tools.BaseDirectory(), $"{_databaseName}.mdf");

        /// <summary>
        /// Полное имя лога базы данных
        /// </summary>
        public string LogFileName => Path.Combine(Tools.BaseDirectory(), $"{_databaseName}_log.ldf");

        /// <summary>
        /// Конструктор провайдера базы данных SQL Server Express
        /// </summary>
        /// <param name="createTables">Создание базы данных</param>
        /// <param name="createTables">Создание таблиц</param>
        public SqlExpressProvider()
        {
            if (!ServerOnline())
            {
                Tools.Terminal.Output("Не удалось соединиться с сервером");

                return;
            }
        }

        /// <summary>
        /// Инициализация базы данных SQL Server Express
        /// </summary>
        public void Initialize()
        {
            //ToDo: настроить вывод ошибок через класс Result

            CreateDatabase();
            CreateTables();
        }

        /// <summary>
        /// Тест подключения к серверу
        /// </summary>
        /// <returns>Результат теста поключения</returns>
        public bool ServerOnline()
        {
            using var connection = new SqlConnection(_serverConnectionString);

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
        /// Записать объект в базу данных SQL Server Express
        /// </summary>
        /// <typeparam name="T">Тип записываемого объекта</typeparam>
        /// <param name="obj">Объект для записи</param>
        /// <param name="result">Возможнеы ошибки при записи</param>
        public void Add<T>(T obj, out Result result)
        {
            var type = typeof(T);
            var properties = type.GetProperties().Where(p => p.Name != "Id");
            var fieldNames = properties.Select(p => p.Name);
            var parameterNames = fieldNames.Select(f => "@" + f);
            var commandText = $"INSERT INTO {type.Name} ({string.Join(", ", fieldNames)}) VALUES ({string.Join(", ", parameterNames)})";
            var parameters = new List<SqlParameter>();

            foreach (var property in properties)
                parameters.Add(new SqlParameter("@" + property.Name, property.GetValue(obj)));

            var rowsAffected = ExecuteNonQuery(commandText, parameters.ToArray(), out result);

            if (rowsAffected > 0)
                result = new Result(CrudOperation.Create);
            else
                result = new Result(CrudOperation.Create, "Ошибка при добавлении объекта в базу данных");
        }

        /// <summary>
        /// Получение объекта из базы данных SQL Server Express
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id">Идентификатор объекта</param>
        /// <param name="result"></param>
        /// <returns>Получаемый объект</returns>
        public T? Get<T>(int Id, out Result result)
        {
            var data = new Dictionary<string, object>();

            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                var parameters = new[] { new SqlParameter("@Id", Id) };
                var reader = ExecuteReader<T>(connection, parameters, $"SELECT * FROM {typeof(T).Name} WHERE Id = @id", out result);

                if (reader == null)
                    return default;

                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        data.Add(reader.GetName(i), reader.GetValue(i));
                }
                else
                {
                    result = new Result(CrudOperation.Read, "Объект не найден в БД");

                    return default;
                }
            }

            var item = Activator.CreateInstance<T>();

            foreach (var property in typeof(T).GetProperties())
            {
                if (data.ContainsKey(property.Name) && data[property.Name] != DBNull.Value)
                    property.SetValue(item, data[property.Name]);
            }

            result = new Result(CrudOperation.Read);
            
            return item;
        }

        /// <summary>
        /// Получить перечень всех объектов из базы данных SQL Server Express
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="result">Возможные ошибки при получении объектов</param>
        /// <returns>Перечень объектов</returns>
        public IEnumerable<T> GetAll<T>(out Result result)
        {
            var data = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                var reader = ExecuteReader<T>(connection, $"SELECT * FROM {typeof(T).Name}", out result);

                if (reader == null)
                    return Enumerable.Empty<T>();

                while (reader.Read())
                {
                    var rowData = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                        rowData.Add(reader.GetName(i), reader.GetValue(i));

                    data.Add(rowData);
                }
            }

            var objects = new List<T>();

            foreach (var itemData in data)
            {
                var item = Activator.CreateInstance<T>();

                foreach (var property in typeof(T).GetProperties())
                {
                    if (itemData.ContainsKey(property.Name) && itemData[property.Name] != DBNull.Value)
                        property.SetValue(item, itemData[property.Name]);
                }

                objects.Add(item);
            }

            result = new Result(CrudOperation.Read);

            return objects;
        }

        /// <summary>
        /// Удаление объекта из базы данных SQL Server Express
        /// </summary>
        /// <typeparam name="T">Тип удаляемого объекта</typeparam>
        /// <param name="Id">Идентификатор удаляемого объекта</param>
        /// <param name="result">Результат удаления</param>
        public void Remove<T>(int Id, out Result result)
        {
            if (Id == default)
            {
                result = new Result(CrudOperation.Delete, "Некорректное значение идентификатора");

                return;
            }

            var commandText = $"DELETE FROM {typeof(T).Name} WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", Id) };

            var rowsAffected = ExecuteNonQuery(commandText, parameters, out result);

            if (rowsAffected > 0)
                result = new Result(CrudOperation.Delete);
            else
                result = new Result(CrudOperation.Delete, "Ошибка при удалении объекта из базы данных");
        }

        /// <summary>
        /// Создание базы данных SQL Server Express
        /// </summary>
        private void CreateDatabase()
        {
            Result result;

            var count = ExecuteScalar($"SELECT COUNT(*) FROM sys.databases WHERE name = '{_databaseName}'", out result);
            
            if (count != null && (int)count > 0)
                ExecuteNonQuery($"DROP DATABASE {_databaseName}", out result);

            if (File.Exists(DatabaseFileName))
                File.Delete(DatabaseFileName);

            if (File.Exists(LogFileName))
                File.Delete(LogFileName);
            
            ExecuteNonQuery($"CREATE DATABASE {_databaseName} ON PRIMARY (NAME={_databaseName}, FILENAME='{DatabaseFileName}')", out result);
        }

        /// <summary>
        /// Получение склярного значения
        /// </summary>
        /// <param name="query">Код запроса</param>
        /// <param name="result">Результат операции</param>
        /// <returns>Скларяное значение</returns>
        private object? ExecuteScalar(string query, out Result result)
        {
            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                connection.Open();

                using var command = new SqlCommand(query, connection);

                try
                {
                    result = new Result(CrudOperation.Read);

                    return command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    result = new Result(CrudOperation.Read, ex.Message);

                    return null;
                }
            }
        }

        /// <summary>
        /// Получение считывателя данных БД
        /// </summary>
        /// <typeparam name="T">Тип считываемого объекта</typeparam>
        /// <param name="connection">Соединение</param>
        /// <param name="parameters">Параметры</param>
        /// <param name="query">Код выполнения запроса на считывание</param>
        /// <param name="result">Результат<</param>
        /// <returns>Считыватель</returns>
        private SqlDataReader? ExecuteReader<T>(SqlConnection connection, SqlParameter[] parameters, string query, out Result result)
        {
            connection.Open();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddRange(parameters);

            try
            {
                result = new Result(CrudOperation.Read);

                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                result = new Result(CrudOperation.Read, ex.Message);

                return null;
            }
        }

        /// <summary>
        /// Получение считывателя данных БД
        /// </summary>
        /// <typeparam name="T">Тип считываемого объекта</typeparam>
        /// <param name="query">Код выполнения запроса на считывание</param>
        /// <param name="result">Результат<</param>
        /// <returns>Считыватель</returns>
        private SqlDataReader? ExecuteReader<T>(SqlConnection connection, string query, out Result result)
        {
            connection.Open();

            using var command = new SqlCommand(query, connection);

            try
            {
                result = new Result(CrudOperation.Read);

                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                result = new Result(CrudOperation.Read, ex.Message);

                return null;
            }
        }

        /// <summary>
        /// Выполнение кода SQL
        /// </summary>
        /// <param name="query">Код выполнения</param>
        /// <param name="parameters">Параметры</param>
        /// <param name="result">Результат выполнения</param>
        /// <returns>Количество затронутых строк в БД</returns>
        private int ExecuteNonQuery(string query, SqlParameter[] parameters, out Result result)
        {
            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                connection.Open();
                
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddRange(parameters);

                try
                {
                    result = new Result(CrudOperation.Read);

                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    result = new Result(CrudOperation.Read, ex.Message);
                    
                    return 0;
                }
            }
        }

        /// <summary>
        /// Выполнение кода SQL
        /// </summary>
        /// <param name="query">Код выполнения</param>
        /// <param name="result">Результат выполнения</param>
        /// <returns>Количество затронутых строк в БД</returns>
        private int ExecuteNonQuery(string query, out Result result)
        {
            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                connection.Open();

                using var command = new SqlCommand(query, connection);

                try
                {
                    result = new Result(CrudOperation.Read);

                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    result = new Result(CrudOperation.Read, ex.Message);

                    return 0;
                }
            }
        }

        /// <summary>
        /// Создание таблиц БД, соотв. моделям из папки Program.ModelsFolderName
        /// </summary>
        private void CreateTables()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var modelsPath = Path.Combine(Tools.BaseDirectory(), Tools.ModelsFolderName);
            var modelFiles = Directory.GetFiles(modelsPath, "*.cs");

            var createTablesQuery = new StringBuilder();
            var foreignKeysQuery = new StringBuilder();

            foreach (var modelFile in modelFiles)
            {
                var className = Path.GetFileNameWithoutExtension(modelFile);

                if (TableExists(className))
                    continue;

                var classType = assembly
                    .GetTypes()
                    .FirstOrDefault(t => t.FullName != null
                        && t.FullName.Contains($"{Tools.ModelsFolderName}.{className}"));

                if (classType == default)
                    continue;

                createTablesQuery.AppendLine($"CREATE TABLE {className} (");

                foreach (var property in classType.GetProperties())
                    SetForeignKeys(createTablesQuery, foreignKeysQuery, className, property);

                createTablesQuery.Length -= 3;
                createTablesQuery.AppendLine(");");
            }

            if (createTablesQuery.Length == 0)
                return;

            ExecuteNonQuery(createTablesQuery.ToString(), out Result result);

            if (foreignKeysQuery.Length > 0)
                ExecuteNonQuery(foreignKeysQuery.ToString(), out result);
        }

        /// <summary>
        /// Установка внешних ключей
        /// </summary>
        /// <param name="createTablesQuery">Код создания таблиц</param>
        /// <param name="foreignKeysQuery">Код создания внешних ключей</param>
        /// <param name="className">Имя модели для маппинга в БД</param>
        /// <param name="property">Свойство модели для маппинга в БД</param>
        private void SetForeignKeys(StringBuilder createTablesQuery, StringBuilder foreignKeysQuery, string className, PropertyInfo property)
        {
            var columnName = property.Name;
            var columnType = GetSqlServerType(property.PropertyType);

            if (columnName == "Id")
                createTablesQuery.AppendLine($"{columnName} {columnType} IDENTITY(1,1) PRIMARY KEY,");
            else
                createTablesQuery.AppendLine($"{columnName} {columnType},");

            if (columnName.EndsWith("Id") && columnName != "Id")
            {
                var referencedTableName = columnName.Substring(0, columnName.Length - 2);
                foreignKeysQuery.AppendLine($"ALTER TABLE {className} " +
                    $"ADD CONSTRAINT FK_{className}_{referencedTableName} " +
                    $"FOREIGN KEY ({columnName}) " +
                    $"REFERENCES {referencedTableName}(Id) " +
                    $"ON DELETE CASCADE;");
            }
        }

        /// <summary>
        /// Проверка существования таблицы в БД
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <returns></returns>
        private bool TableExists(string tableName)
        {
            var scalarResult = ExecuteScalar($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'", out Result result);

            if (!result.Success)
                return false;

            if (scalarResult is int count)
                return count > 0;
            
            return false;
        }

        /// <summary>
        /// Преобразование типов .NET в SQL Server
        /// </summary>
        /// <param name="type">Тип .NET</param>
        /// <returns>Тип SQL Server</returns>
        private string GetSqlServerType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            
            string sqlType;

            switch (Type.GetTypeCode(underlyingType ?? type))
            {
                case TypeCode.Boolean:
                    sqlType = "BIT";
                    break;
                case TypeCode.Byte:
                    sqlType = "TINYINT";
                    break;
                case TypeCode.Char:
                    sqlType = "NCHAR(1)";
                    break;
                case TypeCode.Int16:
                    sqlType = "SMALLINT";
                    break;
                case TypeCode.Int32:
                    sqlType = "INT";
                    break;
                case TypeCode.Int64:
                    sqlType = "BIGINT";
                    break;
                case TypeCode.SByte:
                    sqlType = "SMALLINT";
                    break;
                case TypeCode.UInt16:
                    sqlType = "INT";
                    break;
                case TypeCode.UInt32:
                    sqlType = "BIGINT";
                    break;
                case TypeCode.UInt64:
                    sqlType = "DECIMAL(20)";
                    break;
                case TypeCode.Single:
                    sqlType = "REAL";
                    break;
                case TypeCode.Double:
                    sqlType = "FLOAT";
                    break;
                case TypeCode.Decimal:
                    sqlType = "DECIMAL(18, 0)";
                    break;
                case TypeCode.DateTime:
                    sqlType = "DATETIME2";
                    break;
                case TypeCode.String:
                    sqlType = "NVARCHAR(MAX)";
                    break;
                default:
                    if (type == typeof(byte[]))
                        sqlType = "VARBINARY(MAX)";
                    else if (type == typeof(Guid))
                        sqlType = "UNIQUEIDENTIFIER";
                    else if (type.IsEnum)
                        sqlType = "INT";
                    else
                        throw new NotSupportedException($"Тип {type} не поддерживается.");

                    break;
            }

            return underlyingType != null ? $"{sqlType} NULL" : sqlType;
        }
    }
}


