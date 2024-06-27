using System.Collections.Generic;
using System.Net;

namespace CardTracker.Domain.Abstractions;

public interface IBaseResponse<out T>
{
    string Message { get; }
    HttpStatusCode StatusCode { get; }
    T Data { get; }
}