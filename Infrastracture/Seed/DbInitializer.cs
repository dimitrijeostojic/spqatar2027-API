using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // 🔹 Seed Stadiums
        if (!await context.Stadiums.AnyAsync())
        {
            var stadiums = StadiumSeeder.GetStadiums();

            await context.Stadiums.AddRangeAsync(stadiums);
            await context.SaveChangesAsync();
        }
    }
}
