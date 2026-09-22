namespace Core.Models.App_Models
{
    public sealed class DataError
    {
        public string PublicMessage { get; init; } = string.Empty;
        public string TechnicalMessage { get; init; } = string.Empty;
        public string MethodName { get; init; } = string.Empty;
        public string ExceptionType { get; init; } = "Exception";
    }
}
