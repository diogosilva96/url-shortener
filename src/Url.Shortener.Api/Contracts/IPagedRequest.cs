namespace Url.Shortener.Api.Contracts;

internal interface IPagedRequest
{
    public int PageSize { get; init; }
    
    public int Page { get; init; }
}