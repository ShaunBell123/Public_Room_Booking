using Npgsql;

namespace RoomBooking.Infrastructure.Interfaces;
  public interface IDbConnectionProvider
  {
    NpgsqlConnection GetConnection();
  }


