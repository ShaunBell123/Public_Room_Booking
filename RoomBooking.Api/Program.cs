using Microsoft.AspNetCore.Builder;
using RoomBooking.Application.Interfaces;
using RoomBooking.Infrastructure.Repositorys;
using RoomBooking.Infrastructure.Interfaces;
using RoomBooking.Infrastructure.Services;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IDbConnectionProvider, DbConnectionProvider>();

var app = builder.Build();


app.MapControllers();
app.Run();