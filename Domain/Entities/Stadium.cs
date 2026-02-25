namespace Domain.Entities;

public class Stadium : Entity
{
    public string? StadiumName { get; private set; }
    public string? City { get; private set; }
    public int Capacity { get; private set; }
    public ICollection<Match> Matches { get; private set; } = [];

    public static Stadium Create(string stadiumName, string city, int capacity)
    {
        return new Stadium
        {
            StadiumName = stadiumName,
            City = city,
            Capacity = capacity
        };
    }
}
