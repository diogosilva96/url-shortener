using AutoFixture;

namespace Url.Shortener.Api.Tests.Common.Builder;

public abstract class TestBuilderBase<T>
{
    private readonly List<Action<T>> _configureActions;
    protected readonly Fixture Fixture;

    protected TestBuilderBase()
    {
        _configureActions = new();
        Fixture = new();
    }

    public T Build()
    {
        var buildee = CreateDefault();

        foreach (var action in _configureActions)
        {
            action.Invoke(buildee);
        }

        return buildee;
    }

    public TestBuilderBase<T> With(Action<T> configureAction)
    {
        _configureActions.Add(configureAction);

        return this;
    }

    protected abstract T CreateDefault();
}