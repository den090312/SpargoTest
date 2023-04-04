namespace SpargoTest
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
    public class CrudResult
    {
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
        public CrudResult(CrudOperation operation, string errorMessage)
        {
            Operation = operation;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Конструктор результата операции
        /// </summary>
        /// <param name="operation">Тип операции</param>
        public CrudResult(CrudOperation operation)
        {
            Operation = operation;
        }
    }
}
