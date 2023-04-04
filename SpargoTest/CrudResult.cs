namespace SpargoTest
{
    public enum CrudOperation
    {
        Create,
        Read,
        Update,
        Delete
    }

    public class CrudResult
    {
        public bool Success => ErrorMessage == default ? true : false;

        public CrudOperation Operation { get; set; }
        
        public string? ErrorMessage { get; set; } = default;

        public CrudResult(CrudOperation operation, string errorMessage)
        {
            Operation = operation;
            ErrorMessage = errorMessage;
        }

        public CrudResult(CrudOperation operation)
        {
            Operation = operation;
        }
    }
}
