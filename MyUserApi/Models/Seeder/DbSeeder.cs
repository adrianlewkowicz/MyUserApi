using MyUserApi.Models;
using MyUserApi.Data;

public static class DbSeeder
{
    public static void Seed(MyDbContext context)
    {
        if (!context.Users.Any())
        {
            var users = Enumerable.Range(1, 50).Select(i => new User
            {
                Email = $"user{i}@mail.com",
                FirstName = $"FirstName{i}",
                LastName = $"LastName{i}"
            }).ToList();

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
