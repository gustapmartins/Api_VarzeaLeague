using System.Diagnostics.CodeAnalysis;

namespace VarzeaLeague.Domain.Model.DatabaseSettings;

[ExcludeFromCodeCoverage]
public class VarzeaLeagueDatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;
}