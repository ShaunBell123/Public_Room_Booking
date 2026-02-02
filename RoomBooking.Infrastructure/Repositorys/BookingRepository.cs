using RoomBooking.Domain.Entities;
using RoomBooking.Infrastructure.Interfaces;
using RoomBooking.Application.Interfaces;

namespace RoomBooking.Infrastructure.Repositorys
{
    public class BookingRepository : IBookingRepository
    {
        public readonly IDbConnectionProvider DbConnectionProvider;

        public BookingRepository(IDbConnectionProvider dbConnectionProvider)
        {
            this.DbConnectionProvider = dbConnectionProvider;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            const string sql = """
                    INSERT INTO bookings (room_id, user_id, start_date, end_date, status, created_at)
                    SELECT 
                        r.id AS room_id,
                        @UserId AS user_id,
                        @StartDate AS start_date,
                        @EndDate AS end_date,
                        'confirmed' AS status,
                        NOW() AS created_at
                    FROM rooms r
                    WHERE r.id = @RoomId
                    AND NOT EXISTS (
                        SELECT 1
                        FROM bookings b
                        WHERE b.room_id = @RoomId
                            -- Combine booking date with room times for overlap check
                            AND (b.start_date + r.arrival_time) < (@EndDate + r.departure_time)
                            AND (b.end_date + r.departure_time) > (@StartDate + r.arrival_time)
                    )
                    RETURNING id, room_id, user_id, start_date, end_date, status, created_at;
                """;

            try
            {
                await using var connection = DbConnectionProvider.GetConnection();
                await connection.OpenAsync();

                await using var cmd = connection.CreateCommand();
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@UserId", booking.UserId);
                cmd.Parameters.AddWithValue("@RoomId", booking.RoomId);
                cmd.Parameters.AddWithValue("@StartDate", booking.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", booking.EndDate);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new Booking(
                        id: reader.GetInt32(reader.GetOrdinal("id")),
                        userId: reader.GetInt32(reader.GetOrdinal("user_id")),
                        roomId: reader.GetInt32(reader.GetOrdinal("room_id")),
                        startDate: DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("start_date"))),
                        endDate: DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("end_date"))),
                        status: reader.GetString(reader.GetOrdinal("status")),
                        createdAt: reader.GetDateTime(reader.GetOrdinal("created_at"))
                    );
                }
                else
                {
                    throw new InvalidOperationException("Booking overlaps with an existing booking.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                //TODO: Log exception
                throw;
            }
        } //end of method


        // public async Task<List<Booking>> GetAllBookingsAsync()
        // {
        //     var bookings = new List<Booking>();

        //     const string sql = """
        //                            SELECT id, room_id, user_id, start_date, end_date, start_time, end_time, status, created_at
        //                            FROM bookings;
        //                        """;

        //     await using var dataSource = NpgsqlDataSource.Create(_connectionString);
        //     await using var cmd = dataSource.CreateCommand(sql);
        //     await using var reader = await cmd.ExecuteReaderAsync();

        //     while (await reader.ReadAsync())
        //     {
        //         bookings.Add(new Booking
        //         {
        //             Id = reader.GetInt32(reader.GetOrdinal("id")),
        //             RoomId = reader.GetInt32(reader.GetOrdinal("room_id")),
        //             UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
        //             StartDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("start_date"))),
        //             EndDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("end_date"))),
        //             StartTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("start_time"))),
        //             EndTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("end_time"))),
        //             Status = reader.GetString(reader.GetOrdinal("status")),
        //             CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
        //         });
        //     }

        //     return bookings;
        // } //end of method 

        // public async Task<Booking?> GetBookingByIdAsync(int id)
        // {
        //     const string sql = """
        //                            SELECT id, room_id, user_id, start_date, end_date, start_time, end_time, status, created_at
        //                            FROM bookings
        //                            WHERE id = @id;
        //                        """;

        //     await using var dataSource = NpgsqlDataSource.Create(_connectionString);
        //     await using var cmd = dataSource.CreateCommand(sql);
        //     cmd.Parameters.AddWithValue("@id", id);

        //     await using var reader = await cmd.ExecuteReaderAsync();

        //     if (!await reader.ReadAsync())
        //         return null;

        //     return new Booking
        //     {
        //         Id = reader.GetInt32(reader.GetOrdinal("id")),
        //         RoomId = reader.GetInt32(reader.GetOrdinal("room_id")),
        //         UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
        //         StartDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("start_date"))),
        //         EndDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("end_date"))),
        //         StartTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("start_time"))),
        //         EndTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("end_time"))),
        //         Status = reader.GetString(reader.GetOrdinal("status")),
        //         CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
        //     };

        // } // end of method 

    } //end class
}
