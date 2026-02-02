This is a RoomBooking application that currently has one endpoint: `booking/add`. This endpoint checks if there are any existing bookings for a room at a given time. If a conflict is found, it sends a warning to the user; otherwise, it adds the booking to the database.

In the future, the application will use a CI/CD pipeline to automate testing. Currently, the tests are integration tests that utilize a Testcontainer for PostgreSQL locally.
