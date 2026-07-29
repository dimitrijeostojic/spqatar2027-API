using Domain.Entities;

namespace Infrastructure.Seed;

public static class StadiumSeeder
{
    public static List<Stadium> GetStadiums()
    {
        return new List<Stadium>
        {
            Stadium.Create("Madison Square Garden", "New York", 19812),
            Stadium.Create("Crypto.com Arena", "Los Angeles", 19068),
            Stadium.Create("Stark Arena", "Belgrade", 18700),
            Stadium.Create("O2 Arena", "London", 20000),
            Stadium.Create("Accor Arena", "Paris", 15300),
            Stadium.Create("Lusail Sports Arena", "Lusail", 15000),
            Stadium.Create("Aspire Dome", "Doha", 12000),
            Stadium.Create("Palau Sant Jordi", "Barcelona", 17000),
            Stadium.Create("Mercedes-Benz Arena", "Berlin", 14500),
            Stadium.Create("Scotiabank Arena", "Toronto", 19800)
        };
    }
}