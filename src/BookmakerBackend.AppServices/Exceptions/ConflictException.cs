namespace BookmakerBackend.AppServices.Exceptions;

/// <summary>
/// Ошибка "Конфликт запроса с настоящим состоянием сервера."
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}