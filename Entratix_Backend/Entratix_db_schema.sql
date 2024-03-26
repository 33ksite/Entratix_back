DROP TABLE IF EXISTS EventTickets CASCADE;
DROP TABLE IF EXISTS TicketPurchases CASCADE;
DROP TABLE IF EXISTS Events CASCADE;
DROP TABLE IF EXISTS Users CASCADE;
DROP TABLE IF EXISTS TicketTypes CASCADE;
DROP TABLE IF EXISTS Roles CASCADE;


-- Create the table Roles
CREATE TABLE Roles (
    id INT PRIMARY KEY,
    type VARCHAR(255) UNIQUE NOT NULL
);

-- Insert roles
INSERT INTO Roles (id, type) VALUES
(1, 'User'),
(2, 'RRPP'),
(3, 'Producer'),
(4, 'Administrator');

-- Create the table Users
CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,
    RoleId INT NOT NULL REFERENCES Roles(id),
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Phone VARCHAR(20),
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordSalt BYTEA NOT NULL,
    PasswordHash BYTEA NOT NULL,
    TokenExpires TIMESTAMP,
    TokenCreated TIMESTAMP,
    Token VARCHAR(255)
);

INSERT INTO Users (RoleId, FirstName, LastName, Phone, Email, PasswordSalt, PasswordHash, TokenExpires, TokenCreated, Token) 
VALUES 
(1, 'John', 'Doe', '123456789', 'john.doe@example.com', 'salt123', 'hash123', '2025-02-24', '2024-02-24', 'token123'),
(2, 'Alice', 'Smith', '987654321', 'alice.smith@example.com', 'salt456', 'hash456', '2025-02-24', '2024-02-24', 'token456'),
(3, 'Bob', 'Johnson', '555555555', 'bob.johnson@example.com', 'salt789', 'hash789', '2025-02-24', '2024-02-24', 'token789');


-- Create the table Events
CREATE TABLE Events (
    id INT PRIMARY KEY,
    userId INT REFERENCES Users(id),
    name VARCHAR(255) NOT NULL,
    date DATE NOT NULL,
    location VARCHAR(255),
    cost DECIMAL(10, 2),
    photo VARCHAR(255)
);

-- Insert events
INSERT INTO Events (id, userId, name, date, location, cost, photo) VALUES
(1, 3, 'Event1', '2020-12-01', 'Location1', 100.00, 'https://i.imgur.com/3GUDX4V.jpg'),
(2, 3, 'Event2', '2020-12-15', 'Location2', 150.00, 'https://www.metro951.com/wp-content/uploads/2023/11/ANUNCIO-FEED-DAVID-GUETTA.jpg');

-- Create the table TicketTypes
CREATE TABLE TicketTypes (
    id INT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    price DECIMAL(10, 2) NOT NULL
);

-- Insert ticket types
INSERT INTO TicketTypes (id, name, price) VALUES
(1, 'General Admission', 20.00),
(2, 'VIP', 50.00);

-- Create the table EventTickets to relate Events with TicketTypes
CREATE TABLE EventTickets (
    eventId INT REFERENCES Events(id),
    ticketTypeId INT REFERENCES TicketTypes(id),
    quantity INT NOT NULL,
    PRIMARY KEY (eventId, ticketTypeId)
);

-- Insert associations of ticket types with events
INSERT INTO EventTickets (eventId, ticketTypeId, quantity) VALUES
(1, 1, 100), -- 100 general admission tickets for event 1
(1, 2, 50),  -- 50 VIP tickets for event 1
(2, 1, 200); -- 200 general admission tickets for event 2;

-- Create the table TicketPurchases to relate Users with EventTickets
CREATE TABLE TicketPurchases (
    userId INT REFERENCES Users(id),
    eventId INT REFERENCES Events(id),
    ticketTypeId INT REFERENCES TicketTypes(id),
    quantity_purchased INT,
    used BOOLEAN, -- 0: Not used, 1: Used
    PRIMARY KEY (userId, eventId, ticketTypeId)
);

-- Insert ticket purchases
INSERT INTO TicketPurchases (userId, eventId, ticketTypeId, quantity_purchased, used) VALUES
(1, 1, 1, 5, TRUE),   -- User1 purchased 5 general admission tickets for event 1, all used
(2, 1, 2, 2, FALSE),  -- User2 purchased 2 VIP tickets for event 1, none used
(3, 2, 1, 10, TRUE);  -- User3 purchased 10 general admission tickets for event 2, all used
