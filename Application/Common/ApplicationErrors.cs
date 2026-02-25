using Core;

namespace Application.Common;

public static class ApplicationErrors
{
    public static readonly Error NotFound = new("Application.NotFound", "The requested resource was not found.");
    public static readonly Error DifferentTeams = new("Application.DifferentTeams", "Teams must be different");
    public static readonly Error ExistsTeamConflict = new("Application.ExistsTeamConflict", "One of the teams already has a match at that time");
    public static readonly Error ExistsStadiumConflict = new("Application.ExistsStadiumConflict", "Stadium is already occupied");
    public static readonly Error ExistsSameMatch = new("Application.ExistsSameMatch", "Match between these teams already exists in group");
}

