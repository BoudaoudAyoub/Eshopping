namespace Eshopping.Domain.Exceptions;

public sealed class ExceptionRoot
{
    public string Instance { get; set; } = string.Empty;
    public string ControllerName { get; set; } = string.Empty;
    public string ActionName { get; set; } = string.Empty;
    public DateTime ExceptionLogTime { get; set; } = default!;
    public List<Error> Errors { get; set; } = default!;
}

public sealed class Error
{
    public int StatusCode { get; set; } = 400;
    public string[] ErrorMessages { get; set; } = default!;
}