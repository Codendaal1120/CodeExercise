namespace CodeExercise.Model;

/// <summary>
/// Generic search results container
/// </summary>
public class SearchResults<T>
{
    public T? Value { get; }
    public bool Success { get; }
    public string? ErrorMessage { get; }

    /// <summary/>
    /// <exception cref="ArgumentNullException"></exception>
    private SearchResults(T value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Success = true;
    }

    /// <summary/>
    /// <exception cref="ArgumentNullException"></exception>
    private SearchResults(string errorMessage)
    {
        Success = false;
        ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
    }

    public static SearchResults<T> Fail(string errorMessage)
    {
        return new SearchResults<T>(errorMessage);
    }

    public static SearchResults<T> Succeed(T value)
    {
        return new SearchResults<T>(value);
    }
}