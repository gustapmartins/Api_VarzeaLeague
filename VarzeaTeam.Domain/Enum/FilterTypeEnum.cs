using System.ComponentModel;

namespace VarzeaLeague.Domain.Enum;

public enum FilterTypeEnum
{
    [Description("Ongoing Matches")]
    Ongoing,

    [Description("Completed Matches")]
    Completed,

    [Description("Matches by Date")]
    ByDate
}
