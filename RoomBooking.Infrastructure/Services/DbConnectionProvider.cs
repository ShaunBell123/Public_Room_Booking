using Npgsql;
using Microsoft.Extensions.Configuration;
using RoomBooking.Infrastructure.Interfaces;


namespace RoomBooking.Infrastructure.Services;


public sealed class DbConnectionProvider : IDbConnectionProvider
{
  private readonly string _connectionString;

  public DbConnectionProvider(IConfiguration configuration)
  {
    _connectionString = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Connection string not found");
  }

  public NpgsqlConnection GetConnection()
  {
    return new NpgsqlConnection(_connectionString);
  }
}
