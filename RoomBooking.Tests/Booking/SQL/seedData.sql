-- ===========================
-- SEED OWNERS
-- ===========================

INSERT INTO owners (auth0_id, name, email)
VALUES
('auth0|owner1', 'Alice Owner', 'alice@example.com'),
('auth0|owner2', 'Bob Owner', 'bob@example.com');

-- ===========================
-- SEED USERS
-- ===========================

INSERT INTO users (auth0_id, name, email)
VALUES
('auth0|user1','User 1','user1@example.com'),
('auth0|user2','User 2','user2@example.com'),
('auth0|user3','User 3','user3@example.com');

-- ===========================
-- SEED ROOMS
-- ===========================

INSERT INTO rooms (owner_id, name, description, type, location, capacity, price_per_night, amenities, arrival_time, departure_time)
VALUES
(1, 'Luxury Suite', 'Spacious suite with sea view', 'Suite', 'Paris', 2, 150.00, ARRAY['WiFi','TV','Minibar'], '14:00:00', '11:00:00'),
(2, 'Economy Room', 'Simple room with garden view', 'Economy', 'London', 2, 80.00, ARRAY['WiFi'], '15:00:00', '10:00:00');

-- ===========================
-- SEED BOOKINGS
-- ===========================
-- Using room's arrival_time and departure_time for start_time and end_time

-- INSERT INTO bookings (room_id, user_id, start_date, end_date, status)
-- VALUES
-- (1, 1, '2025-02-01', '2025-02-03', 'confirmed'),
-- (1, 2, '2025-02-05', '2025-02-07', 'confirmed'),
-- (2, 3, '2025-02-02', '2025-02-04', 'confirmed');
-- ===========================
-- EXAMPLE COMMENT INPUT
-- ===========================
-- Suppose user wants to book 'Luxury Suite' from 2025-02-03 to 2025-02-05
-- with comment: "Need late check-in due to flight"
-- You would insert:

-- INSERT INTO bookings (room_id, user_id, start_date, end_date, status, comment)
-- VALUES (1, 3, '2025-02-03', '2025-02-05', 'confirmed', 'Need late check-in due to flight');