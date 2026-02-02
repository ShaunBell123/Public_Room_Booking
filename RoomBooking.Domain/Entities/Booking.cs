namespace RoomBooking.Domain.Entities
{
    public class Booking
    {
        public int Id { get; private set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public Booking(
            int userId,
            int roomId,
            DateOnly startDate,
            DateOnly endDate,
            string status
            )
        {
            UserId = userId;
            RoomId = roomId;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            CreatedAt = DateTime.UtcNow;
        }

       public Booking(
           int id, 
           int userId, 
           int roomId, 
           DateOnly startDate, 
           DateOnly endDate, 
           string status, 
           DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            RoomId = roomId;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            CreatedAt = createdAt;
        }
        
    }
}