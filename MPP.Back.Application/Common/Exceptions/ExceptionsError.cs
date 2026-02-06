using MPP.Back.Application.Common.Responses;

namespace MPP.Back.Application.Common.Exceptions
{
    /// <summary>
    /// Excepción personalizada que encapsula un ErrorResponse estándar de la API.
    /// </summary>
    public class ExceptionsError : Exception
    {
        public ErrorResponse Error { get; }
        public ExceptionsError(int statusCode, string message)
        : base(message)
        {
            Error = new ErrorResponse(statusCode, message);
        }
    }
}
