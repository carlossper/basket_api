namespace BasketApi.Exceptions
{
    /// <summary>
    /// Base Exception for BasketAPI's error handling.
    /// </summary>
    public class BasketApiBaseException : Exception
    {
        public BasketApiBaseException() { }

        public BasketApiBaseException(string errorMessage) : base (errorMessage) { }

        public BasketApiBaseException(string message, Exception innerException) : base (message, innerException) { }
    }
}
