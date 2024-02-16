namespace Url.Shortener.Api.Contracts;

public class ListUrlRequest : IPagedRequest
{
    public required int PageSize { get; init; }
    
    public required int Page { get; init; }
}