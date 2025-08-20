﻿namespace Eshopping.Domain.Exceptions;
public class DomainException : Exception
{
    public int StatusCode { get; set; }
    public IDictionary<int, string[]>? Errors { get; }

    public DomainException() { }

    public DomainException(string message, IDictionary<int, string[]>? errors, int statusCode = 400)
        : base(message)
    {
        StatusCode = statusCode;
        Errors = errors;
    }

    public DomainException(string message) : this(message, null!) { }

    public DomainException(string message, int statusCode) : this(message, null!, statusCode) { }

    public DomainException(IDictionary<int, string[]> errors) : this(string.Empty, errors) { }

    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}
