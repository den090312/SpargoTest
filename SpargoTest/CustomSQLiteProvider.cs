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

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(commandText, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    var rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        crudResult = new CrudResult(CrudOperation.Create);
                    else
                        crudResult = new CrudResult(CrudOperation.Create, "Ошибка при добавлении объекта в базу данных");
                }
            }
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

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand($"SELECT * FROM {typeof(T).Name}", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
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
                    }
                }
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

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(commandText, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@Id", Id));

                    var rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        crudResult = new CrudResult(CrudOperation.Delete);
                    else
                        crudResult = new CrudResult(CrudOperation.Delete, "Ошибка при удалении объекта из базы данных");
                }
            }
        }
    }
}


