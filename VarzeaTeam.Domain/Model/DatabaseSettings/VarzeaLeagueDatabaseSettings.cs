namespace VarzeaLeague.Domain.Model.DatabaseSettings;

public class VarzeaLeagueDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string VarzeaLeagueCollectionName { get; set; } = null!;
}