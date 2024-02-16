namespace Url.Shortener.Api.Contracts;

public class PagedResult<T>
{
    public required T Data { get; init; }
    
    public required int Page { get; init; }
    
    public required int PageCount { get; init; }
    
    public required int PageSize { get; init; }
}