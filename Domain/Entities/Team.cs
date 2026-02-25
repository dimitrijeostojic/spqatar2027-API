namespace Domain.Entities;

public class Team : Entity
{
    public string? TeamName { get; private set; }
    public string? FlagIcon { get; private set; }
    public int? GroupId { get; private set; }
    public Group? Group { get; private set; }

    public ICollection<Match> HomeMatches { get; set; } = [];
    public ICollection<Match> AwayMatches { get; set; } = [];

    public static Team Create(string teamName, string? flagIcon, Group group)
    {
        return new Team()
        {
            TeamName = teamName,
            FlagIcon = flagIcon,
            Group = group
        };
    }

    public Team UpdateTeamName(string teamName)
    {
        TeamName = teamName;
        return this;
    }
    public Team UpdateFlagIcon(string flagIcon)
    {
        FlagIcon = flagIcon;
        return this;
    }

    public Team UpdateGroup(int groupId)
    {
        GroupId = groupId;
        return this;
    }

}
