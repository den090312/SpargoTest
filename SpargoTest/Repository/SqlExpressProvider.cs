using System.Data.SqlClient;
using System.Dynamic;
using System.Reflection;
using System.Text;

using SpargoTest.DTO;
using SpargoTest.Interfaces;
using SpargoTest.Models;
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
        private static readonly string _serverConnectionString = "Server=(localdb)\\mssqllocaldb;Trusted_Connection=True;";

        /// <summary>
        /// Имя базы данных
        /// </summary>
        private readonly string _databaseName = "SpargoTest";

        /// <summary>
        /// Строка подключения к базе данных SQL Server Express
        /// </summary>
        private string DatabaseConnectionString => $"{_serverConnectionString}Database={_databaseName};";

        /// <summary>
        /// Полное имя файла базы данных
        /// </summary>
        private string DatabaseFileName => Path.Combine(Tools.BaseDirectory(), $"{_databaseName}.mdf");

        /// <summary>
        /// Полное имя лога базы данных
        /// </summary>
        private string LogFileName => Path.Combine(Tools.BaseDirectory(), $"{_databaseName}_log.ldf");

        /// <summary>
        /// Инициализация базы данных SQL Server Express
        /// <param name="result">Результат инициализации</param>
        /// </summary>
        public void Initialize(out Result result)
        {
            CreateDatabase(out result);

            if (!result.Success)
                return;

            CreateTables(out result);
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
        /// <param name="result">Возможные ошибки при записи</param>
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

            var rowsAffected = ExecuteNonQuery(commandText, DatabaseConnectionString, out result, parameters.ToArray());

            if (rowsAffected > 0)
                result = new Result(CrudOperation.Create);
            else
                result = new Result(CrudOperation.Create, result.ErrorMessage ?? "Ошибка при добавлении объекта в базу данных");
        }

        /// <summary>
        /// Получение объекта из базы данных SQL Server Express
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id">Идентификатор объекта</param>
        /// <param name="result">Возможные ошибки при получении объекта</param>
        /// <returns>Получаемый объект</returns>
        public T? Get<T>(int Id, out Result result)
        {
            var parameters = new[] { new SqlParameter("@Id", Id) };
            var data = GetData($"SELECT * FROM {typeof(T).Name} WHERE Id = @Id", DatabaseConnectionString, out result, parameters).FirstOrDefault();

            if (data == null || !result.Success)
                return default;

            var item = Activator.CreateInstance<T>();
            SetProperties(data, ref item);
            result = new Result(CrudOperation.Read);

            return item;
        }

        /// <summary>
        /// Получить список продуктов по идентификатору аптеки
        /// </summary>
        /// <param name="pharmacyId">Идентификатор аптеки</param>
        /// <returns>Список продуктов</returns>
        public IEnumerable<ProductDto> GetProductsByPharmacy(int pharmacyId)
        {
            var query = "SELECT Product.Id AS Id, Product.Name AS Name, COUNT(Consignment.Id) AS ProductCount FROM Product " +
                "JOIN Consignment ON Product.Id = Consignment.ProductId " +
                "JOIN Warehouse ON Consignment.WarehouseId = Warehouse.Id " +
                "WHERE Warehouse.PharmacyId = @PharmacyId " +
                "GROUP BY Product.Id, Product.Name";
            
            var parameters = new[] { new SqlParameter("@PharmacyId", pharmacyId) };
            var data = GetData(query, DatabaseConnectionString, out Result result, parameters);
            var objects = new List<ProductDto>();

            if (!result.Success)
                return objects;

            SetData(data, ref objects);

            return objects;
        }

        /// <summary>
        /// Получить перечень всех объектов из базы данных SQL Server Express
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="result">Возможные ошибки при получении объектов</param>
        /// <returns>Перечень объектов</returns>
        public IEnumerable<T> GetAll<T>(out Result result)
        {
            var data = GetData($"SELECT * FROM {typeof(T).Name}", DatabaseConnectionString, out result);
            var objects = new List<T>();

            if (!result.Success)
                return objects;

            SetData(data, ref objects);

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
            var rowsAffected = ExecuteNonQuery(commandText, DatabaseConnectionString, out result, parameters);

            if (rowsAffected > 0)
                result = new Result(CrudOperation.Delete);
            else
                result = new Result(CrudOperation.Delete, result.ErrorMessage ?? "Ошибка при удалении объекта из базы данных");
        }

        /// <summary>
        /// Установка свойства
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="data">Данные для свойства</param>
        /// <param name="item">Объект для свойства</param>
        private static void SetProperties<T>(Dictionary<string, object> data, ref T? item)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                if (data.ContainsKey(property.Name) && data[property.Name] != DBNull.Value)
                    property.SetValue(item, data[property.Name]);
            }
        }

        /// <summary>
        /// Установка данных
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="data">Данные</param>
        /// <param name="objects">Коллекция для установки данных</param>
        private static void SetData<T>(IEnumerable<Dictionary<string, object>> data, ref List<T> objects)
        {
            foreach (var itemData in data)
            {
                var item = Activator.CreateInstance<T>();
                SetProperties(itemData, ref item);

                if (item != null)
                    objects.Add(item);
            }
        }

        /// <summary>
        /// Получение данных
        /// </summary>
        /// <param name="query">Строка запроса</param>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="result">Результат получения</param>
        /// <param name="parameters">Параметры запроса</param>
        /// <returns>Перечень данных</returns>
        private IEnumerable<Dictionary<string, object>> GetData(string query, string connectionString, out Result result, SqlParameter[]? parameters = null)
        {
            var data = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(connectionString))
            {
                var reader = GetReader(connection, parameters, query, out result);

                if (reader == null || !result.Success)
                    return Enumerable.Empty<Dictionary<string, object>>();

                while (reader.Read())
                {
                    var rowData = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                        rowData.Add(reader.GetName(i), reader.GetValue(i));

                    data.Add(rowData);
                }
            }

            if (data.Count == 0)
                result.ErrorMessage = "Нет данных";

            return data;
        }

        /// <summary>
        /// Создание базы данных SQL Server Express
        /// </summary>
        private void CreateDatabase(out Result result)
        {
            result = new Result(CrudOperation.Create);
            var count = GetScalar($"SELECT COUNT(*) FROM sys.databases WHERE name = '{_databaseName}'", _serverConnectionString);

            if (count is int value && value > 0)
                ExecuteNonQuery($"DROP DATABASE {_databaseName}", _serverConnectionString, out result);

            if (!result.Success)
                return;

            ExecuteNonQuery($"CREATE DATABASE {_databaseName} ON PRIMARY (NAME={_databaseName}, FILENAME='{DatabaseFileName}')"
                , _serverConnectionString
                , out result);
        }

        /// <summary>
        /// Получение склярного значения
        /// </summary>
        /// <param name="query">Код запроса</param>
        /// <param name="connectionString">Строка подключения</param>
        /// <returns>Скларяное значение</returns>
        private object? GetScalar(string query, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using var command = new SqlCommand(query, connection);

                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Получение считывателя данных БД
        /// </summary>
        /// <param name="connection">Соединение</param>
        /// <param name="parameters">Параметры</param>
        /// <param name="query">Код выполнения запроса на считывание</param>
        /// <param name="result">Результат<</param>
        /// <returns>Считыватель</returns>
        private SqlDataReader? GetReader(SqlConnection connection, SqlParameter[]? parameters, string query, out Result result)
        {
            connection.Open();

            using var command = new SqlCommand(query, connection);

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            try
            {
                result = new Result(CrudOperation.Read);

                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                result = new Result(CrudOperation.Read, ex.Message != string.Empty ? ex.Message : "Ошибка получения считывателя БД");

                return null;
            }
        }

        /// <summary>
        /// Выполнение кода SQL
        /// </summary>
        /// <param name="query">Код выполнения</param>
        /// <param name="parameters">Параметры</param>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="result">Результат выполнения</param>
        /// <returns>Количество затронутых строк в БД</returns>
        private int ExecuteNonQuery(string query, string connectionString, out Result result, SqlParameter[]? parameters = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                using var command = new SqlCommand(query, connection);

                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                try
                {
                    result = new Result(CrudOperation.Update);

                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    result = new Result(CrudOperation.Update, ex.Message != string.Empty ? ex.Message : "Ошибка выполнения кода SQL");
                    
                    return 0;
                }
            }
        }

        /// <summary>
        /// Создание таблиц БД, соотв. моделям из папки Program.ModelsFolderName
        /// <param name="result">Результат выполнения</param>
        /// </summary>
        private void CreateTables(out Result result)
        {
            result = new Result(CrudOperation.Create);

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
                    SetKeys(createTablesQuery, foreignKeysQuery, className, property);

                createTablesQuery.Length -= 3;
                createTablesQuery.AppendLine(");");
            }

            if (createTablesQuery.Length == 0)
                return;

            ExecuteNonQuery(createTablesQuery.ToString(), DatabaseConnectionString, out result);

            if (!result.Success)
                return;

            if (foreignKeysQuery.Length > 0)
                ExecuteNonQuery(foreignKeysQuery.ToString(), DatabaseConnectionString, out result);
        }

        /// <summary>
        /// Установка ключей
        /// </summary>
        /// <param name="createTablesQuery">Код создания первичных ключей</param>
        /// <param name="foreignKeysQuery">Код создания внешних ключей</param>
        /// <param name="className">Имя модели для маппинга в БД</param>
        /// <param name="property">Свойство модели для маппинга в БД</param>
        private void SetKeys(StringBuilder createTablesQuery, StringBuilder foreignKeysQuery, string className, PropertyInfo property)
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
            var scalarResult = GetScalar($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'", DatabaseConnectionString);

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


