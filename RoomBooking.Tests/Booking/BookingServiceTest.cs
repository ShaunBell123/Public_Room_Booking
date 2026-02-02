using Testcontainers.PostgreSql;
using RoomBooking.Infrastructure.Services;
using RoomBooking.Infrastructure.Repositorys;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace RoomBooking.Tests.Booking;

public sealed class BookingServiceTest : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17")
        .WithUsername("postgres")
        .WithPassword("password")
        .WithDatabase("testdb")
        .Build();

    private DbConnectionProvider _dbConnectionProvider = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _postgres.GetConnectionString()
            })
            .Build();

        _dbConnectionProvider = new DbConnectionProvider(configuration);
        await SeedDatabase();
    }

    public Task DisposeAsync()
    {
        return _postgres.DisposeAsync().AsTask();
    }

    private async Task SeedDatabase()
    {
        var sqlFilePath = Path.Combine(AppContext.BaseDirectory, "Booking", "SQL", "makingTables.sql");
        var makeTables = await File.ReadAllTextAsync(sqlFilePath);

        var insertDataFilePath = Path.Combine(AppContext.BaseDirectory, "Booking", "SQL", "seedData.sql");
        var insertData = await File.ReadAllTextAsync(insertDataFilePath);

        await using var conn = _dbConnectionProvider.GetConnection();
        await conn.OpenAsync();

        await using var command = new NpgsqlCommand(makeTables, conn);
        await command.ExecuteNonQueryAsync();

        await using var insertCommand = new NpgsqlCommand(insertData, conn);
        await insertCommand.ExecuteNonQueryAsync();
    }

    [Fact]
    public async Task ShouldAllowBackToBackBooking()
    {
        /*
         * Testing back-to-back booking where existing booking is from 1st to 2nd
         * Dec and the after booking is from 3nd to 4rd Dec.
         * New booking request is from 2nd to 3rd Dec.
         * This should be allowed as there is no overlap.
        */
        var repo = new BookingRepository(_dbConnectionProvider);

        var beforeBooking = new Domain.Entities.Booking(
            1,
            1,
            new DateOnly(2025, 12, 1),
            new DateOnly(2025, 12, 2),
            "Pending"
        );
        var createdBefore = await repo.CreateBookingAsync(beforeBooking);

        var afterBooking = new Domain.Entities.Booking(
            1,
            1,
            new DateOnly(2025, 12, 3),
            new DateOnly(2025, 12, 4),
            "Pending"
        );
        var createdAfter = await repo.CreateBookingAsync(afterBooking);


        var currentBooking = new Domain.Entities.Booking(
            1,
            1,
            new DateOnly(2025, 12, 2),
            new DateOnly(2025, 12, 3),
            "Pending"
        );
        var createdCurrent = await repo.CreateBookingAsync(currentBooking);

        Assert.NotNull(createdBefore);
        Assert.NotNull(createdCurrent);
        Assert.NotNull(createdAfter);

        Assert.Equal(new DateOnly(2025, 12, 2), createdCurrent.StartDate);
        Assert.Equal(new DateOnly(2025, 12, 3), createdCurrent.EndDate);
    }

    [Fact]
    public async Task ShouldPreventOverlappingBooking()
    {
        /*
         * Testing overlap where existing booking is from 4th to 6th Feb.
         * New booking request is from 3rd to 5th Feb
         * This should throw an exception.
         * stoping a overlap booking.
         */

        var repo = new BookingRepository(_dbConnectionProvider);

        var existingBooking = new Domain.Entities.Booking(
            1,
            1,
            new DateOnly(2025, 2, 4),
            new DateOnly(2025, 2, 6),
            "Pending"
        );
        var createdExisting = await repo.CreateBookingAsync(existingBooking);
        Assert.NotNull(createdExisting);

        var overlappingBooking = new Domain.Entities.Booking(
            1,
            1,
            new DateOnly(2025, 2, 3),
            new DateOnly(2025, 2, 5),
            "Pending"
        );

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await repo.CreateBookingAsync(overlappingBooking);
        });
    }

    [Fact]
    public async Task ShouldPreventOverlappingBookingReversed()
    {
        /*
         * Testing overlap where existing booking is from 4th to 6th April.
         * New booking request is from 5rd to 7th April
         * This should throw an exception. stoping a overlap booking.
         * This is reverse of previous test case.
         */

        var repo = new BookingRepository(_dbConnectionProvider);

        var existingBooking = new Domain.Entities.Booking(
            1,
            1,
            new DateOnly(2025, 4, 4),
            new DateOnly(2025, 4, 6),
            "Pending"
        );
        var createdExisting = await repo.CreateBookingAsync(existingBooking);
        Assert.NotNull(createdExisting);

        // Overlapping booking: 5th → 7th April
        var overlappingBooking = new Domain.Entities.Booking(
            1,
            1,
            new DateOnly(2025, 4, 5),
            new DateOnly(2025, 4, 7),
            "Pending"
        );

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await repo.CreateBookingAsync(overlappingBooking);
        });
    }
}
