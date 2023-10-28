using Microsoft.Extensions.Caching.Memory;

namespace Url.Shortener.Api.UnitTests.Builder;

internal class MemoryCacheBuilder
{
    private readonly List<Action<IMemoryCache>> _setupActions;

    public MemoryCacheBuilder() => _setupActions = new();

    public IMemoryCache Build()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        foreach (var setupAction in _setupActions)
        {
            setupAction.Invoke(memoryCache);
        }

        return memoryCache;
    }

    public MemoryCacheBuilder Setup(string key, object value)
    {
        _setupActions.Add(memoryCache => memoryCache.Set(key, value));

        return this;
    }
}