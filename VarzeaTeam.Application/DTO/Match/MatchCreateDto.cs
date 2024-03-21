using MongoDB.Bson.Serialization.Attributes;
using VarzeaTeam.Domain.Model.Team;

namespace VarzeaTeam.Application.DTO.Match;

public class MatchCreateDto
{
    public int Id { get; set; }

    public TeamModel HomeTeam { get; set; }

    public TeamModel VisitingTeam { get; set; }

    public string Local { get; set; }

    public DateTime Date { get; set; }

    public TeamModel TeamWin { get; set; }

    public bool Play { get; set; }
}
