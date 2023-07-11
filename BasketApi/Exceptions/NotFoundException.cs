namespace BasketApi.Exceptions
{
    /// <summary>
    /// Resource not found Exception for BasketAPI's error handling.
    /// </summary>
    public class NotFoundException : BasketApiBaseException
    {
        public NotFoundException() : base() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}