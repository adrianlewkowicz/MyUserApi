using Xunit;
using Microsoft.EntityFrameworkCore;
using MyUserApi.Models;
using MyUserApi.Services;
using MyUserApi.Data;
using System.Threading.Tasks;
using System.Linq;
using System;

public class UserServiceTests : IDisposable
{
    private readonly MyDbContext _context;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new MyDbContext(options);
        _userService = new UserService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateUserAsync_AddsUser()
    {
        var user = new User { Email = "test@example.com", FirstName = "John", LastName = "Doe" };

        var result = await _userService.CreateUserAsync(user);

        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
        Assert.Single(_context.Users);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsCorrectUser()
    {
        var created = await _userService.CreateUserAsync(new User
        {
            Email = "test2@example.com",
            FirstName = "Jane",
            LastName = "Doe"
        });

        var fetched = await _userService.GetUserAsync(created.Id);

        Assert.NotNull(fetched);
        Assert.Equal("Jane", fetched.FirstName);
    }

    [Fact]
    public async Task GetUserAsync_ReturnsNull_IfNotFound()
    {
        var user = await _userService.GetUserAsync(999);
        Assert.Null(user);
    }

    [Fact]
    public async Task GetUsersAsync_ReturnsPaginated()
    {
        for (int i = 1; i <= 20; i++)
        {
            await _userService.CreateUserAsync(new User
            {
                Email = $"user{i}@mail.com",
                FirstName = $"First{i}",
                LastName = $"Last{i}"
            });
        }

        var page1 = await _userService.GetUsersAsync(1, 10);
        var page2 = await _userService.GetUsersAsync(2, 10);

        Assert.Equal(10, page1.Count());
        Assert.Equal("user1@mail.com", page1.First().Email);
        Assert.Equal("user11@mail.com", page2.First().Email);
    }


    [Fact]
    public async Task UpdateUserAsync_UpdatesFields()
    {
        var created = await _userService.CreateUserAsync(new User
        {
            Email = "change@example.com",
            FirstName = "Before",
            LastName = "Update"
        });

        created.FirstName = "After";
        created.LastName = "Changed";

        var updated = await _userService.UpdateUserAsync(created);

        Assert.Equal("After", updated.FirstName);
        Assert.Equal("Changed", updated.LastName);
    }

    [Fact]
    public async Task UpdateUserAsync_Throws_WhenNotFound()
    {
        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _userService.UpdateUserAsync(new User
            {
                Id = 999,
                Email = "x@x.com",
                FirstName = "x",
                LastName = "x"
            })
        );

       Assert.Equal("User with id 999 not found.", ex.Message);
    }

    [Fact]
    public async Task CreateUserAsync_Throws_OnInvalidEmail()
    {
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            _userService.CreateUserAsync(new User
            {
                Email = "bademail",
                FirstName = "Bad",
                LastName = "Email"
            })
        );

        Assert.Contains("Invalid email format", ex.Message);
    }

    [Fact]
    public async Task DeleteUserAsync_RemovesUser()
    {
        var user = await _userService.CreateUserAsync(new User
        {
            Email = "delete@me.com",
            FirstName = "To",
            LastName = "Delete"
        });

        var result = await _userService.DeleteUserAsync(user.Id);

        Assert.True(result);
        Assert.Empty(_context.Users);
    }

    [Fact]
    public async Task DeleteUserAsync_ReturnsFalse_IfNotFound()
    {
        var result = await _userService.DeleteUserAsync(999);
        Assert.False(result);
    }
}
