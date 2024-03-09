DROP TABLE IF EXISTS TicketPurchases;
DROP TABLE IF EXISTS EventTickets;
DROP TABLE IF EXISTS TicketTypes;
DROP TABLE IF EXISTS Events;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Roles;

-- Create the table Roles
CREATE TABLE Roles (
    id INT PRIMARY KEY,
    type VARCHAR(255) UNIQUE NOT NULL
);

-- Insert roles
INSERT INTO Roles (id, type) VALUES
(1, 'administrator'),
(2, 'RPP'),
(3, 'producer');

-- Create the table Users
CREATE TABLE Users (
    Id INT PRIMARY KEY,
    RoleId INT REFERENCES Roles(id),
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
    Email VARCHAR(255) UNIQUE NOT NULL
);

-- Insert users
INSERT INTO Users (Id, RoleId, FirstName, LastName, Phone, Email) VALUES
(1, 1, 'Name1', 'Surname1', '123456789', 'user1@example.com'),
(2, 2, 'Name2', 'Surname2', '987654321', 'user2@example.com'),
(3, 3, 'Name3', 'Surname3', '555555555', 'user3@example.com');

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
