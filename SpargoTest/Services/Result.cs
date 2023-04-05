namespace SpargoTest.Services
{
    /// <summary>
    /// Тип операции
    /// </summary>
    public enum CrudOperation
    {
        Create,
        Read,
        Update,
        Delete
    }

    /// <summary>
    /// Результат операции
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Индикатор успешной операции
        /// </summary>
        public bool Success => ErrorMessage == default;

        /// <summary>
        /// Тип операции
        /// </summary>
        public CrudOperation Operation { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string? ErrorMessage { get; set; } = default;

        /// <summary>
        /// Конструктор результата операции
        /// </summary>
        /// <param name="operation">Тип операции</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        public Result(CrudOperation operation, string errorMessage)
        {
            Operation = operation;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Конструктор результата операции
        /// </summary>
        /// <param name="operation">Тип операции</param>
        public Result(CrudOperation operation)
        {
            Operation = operation;
        }
    }
}
