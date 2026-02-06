using System.Net;
using System.Runtime.Serialization;

namespace MPP.Back.Application.Common.Responses
{
    /// <summary>
    /// Representa una respuesta exitosa estándar para la API.
    /// </summary>
    /// <typeparam name="T">Tipo del objeto de datos devuelto.</typeparam>
    public class SuccessResponse<T>
    {
        [DataMember(Name = "code")]
        public int StatusCode { get; set; }
        [DataMember(Name = "data")]
        public T? Data { get; set; }
        public SuccessResponse(T? data)
        {
            StatusCode = (int)HttpStatusCode.OK;
            Data = data;
        }
    }
}
