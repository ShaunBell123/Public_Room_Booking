-- ===========================
-- DROP TABLES IF THEY EXIST
-- ===========================
DROP TABLE IF EXISTS bookings;
DROP TABLE IF EXISTS rooms;
DROP TABLE IF EXISTS owners;
DROP TABLE IF EXISTS users;

-- ===========================
-- CREATE TABLES
-- ===========================

CREATE TABLE owners (
    id SERIAL PRIMARY KEY,
    auth0_id TEXT UNIQUE NOT NULL,
    name TEXT NOT NULL,
    email TEXT UNIQUE NOT NULL,
    created_at TIMESTAMP DEFAULT NOW()
);

CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    auth0_id TEXT UNIQUE NOT NULL,
    name TEXT NOT NULL,
    email TEXT UNIQUE NOT NULL,
    created_at TIMESTAMP DEFAULT NOW()
);

CREATE TABLE rooms (
    id SERIAL PRIMARY KEY,
    owner_id INT REFERENCES owners(id) ON DELETE CASCADE,
    name TEXT NOT NULL,
    description TEXT,
    type TEXT NOT NULL,
    location TEXT NOT NULL,
    capacity INT NOT NULL DEFAULT 1,
    price_per_night NUMERIC(10,2),
    amenities TEXT[],
    arrival_time TIME DEFAULT '14:00:00',
    departure_time TIME DEFAULT '11:00:00',
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);

CREATE TABLE bookings (
    id SERIAL PRIMARY KEY,
    room_id INT REFERENCES rooms(id) ON DELETE CASCADE,
    user_id INT REFERENCES users(id) ON DELETE CASCADE, 
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    status TEXT DEFAULT 'confirmed',               
    created_at TIMESTAMP DEFAULT NOW()
);
