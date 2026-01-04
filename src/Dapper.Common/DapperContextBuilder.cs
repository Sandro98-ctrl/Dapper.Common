using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Common;

public sealed class DapperContextBuilder(IServiceCollection services)
{
    private bool _providerConfigured;

    internal IServiceCollection Services { get; } = services;

    internal void MarkProviderConfigured()
    {
        if (_providerConfigured)
            throw new InvalidOperationException("Only one database provider can be configured.");

        _providerConfigured = true;
    }
}