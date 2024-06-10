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
    description VARCHAR(8000),
    date DATE NOT NULL,
    location VARCHAR(255),
    cost DECIMAL(10, 2),
    photo VARCHAR(8000),
    department VARCHAR(255)
);

-- Insert events
INSERT INTO Events (id, userId, name, description, date, location, cost, photo, department) VALUES
(12, 1, 'Boris Brejcha b2b Ann Clue', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-02', 'Rambla, av Brasil', 2000.00, 'https://scontent.fmvd3-1.fna.fbcdn.net/v/t1.18169-9/18671209_1005554409580602_324848815674572030_n.jpg?_nc_cat=102&ccb=1-7&_nc_sid=5f2048&_nc_ohc=XCkMVA6HDYQAX9RoSCo&_nc_ht=scontent.fmvd3-1.fna&oh=00_AfAKiHCSbIIMJiy-tXUnJkOSoQ2Ib4QsBvl3l01QmcsgVA&oe=662DCD08','Montevideo'),
(10, 1, 'Senderos Phoro - Quebrada, Punta del Este', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-03', 'Quebrada, Maldonado', 800.00, 'https://underground.com.uy/wp-content/uploads/2017/03/tapa-senderos-1.png','Maldonado'),
(11, 1, 'Kidd Keo - Montevideo Music Box','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-01',  'Montevideo Music Box', 800.00, 'https://scontent.fmvd3-1.fna.fbcdn.net/v/t1.18169-9/17498749_741198912718445_1364156188934314741_n.jpg?_nc_cat=109&ccb=1-7&_nc_sid=5f2048&_nc_ohc=jlywwkX7xTIAX-79MPO&_nc_ht=scontent.fmvd3-1.fna&oh=00_AfApl7OwrFzY1onZqrwEwu6LFj5lDzTWC56cDgsTc0RNfw&oe=662DCCB5','Montevideo'),
(4, 1, 'EMILE - Molen Live, Agustin B2B Decker', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-04', 'Centro y yaguaron', 400.00, 'https://api.entraste.com/sc/uploads/file/c2ad16cff9b81acb21b6e36c4075dee3','Montevideo'),
(5, 1, 'Error 909 - Punta del Este, Maldonado', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-03', 'Chacra la Quebrada, Maldonado', 900.00, 'https://api.entraste.com/sc/uploads/file/0707fbb8b592d1592fa71885ff9bb2d0','Maldonado'),
(6, 1, 'EL KUELGUE - Teatro de verano 3/2024', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-11-01', 'Teatro de Verano', 1300.00, 'https://www.saladelmuseo.com.uy/media/zoo/images/cuadrada_1_1_9cab0ac3adb315094ea891fb9d9be683.png','Montevideo'),
(7, 1, 'Sound Festival - 2024','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-10-12',  'Rambla, av Brasil', 1500.00, 'https://imgproxy.ra.co/_/quality:66/aHR0cHM6Ly9pbWFnZXMucmEuY28vOTU0MzNkZGVlNjU0MTU3YjhhNzg5MWU5YzlmMmEzN2FjZmEwMTg5Zi5qcGc=','Montevideo'),
(8, 1, 'ACRU a Uruguay - Montevideo Music Box', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-01', 'Rambla y Av Brasil', 1000.00, 'https://pbs.twimg.com/media/FyrgnicWABE0XLE.jpg:large','Montevideo'),
(9, 1, 'YSY A 2024 - Antel Arena','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-11-01',  'Antel Arena', 1300.00, 'https://www.saladelmuseo.com.uy/media/zoo/images/YSY_Uruguay_feed_instagram_24_3ac18770ef6feb13962b430679c59552.jpg','Canelones'),
(1, 1, 'PHONOTEQUE LIVE PHORO APERTURA 2024 NO TE LO PIERDAS', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2020-12-12', 'Piedra Alta 1254', 100.00, 'https://static1.squarespace.com/static/58a39fb503596e0fdbb001a6/58aa378bff7c50f538e999a1/5c599dba6e9a7f0c4232b02e/1549377032782/51398940_2545058242177592_4914365089764605952_o.jpg?format=1500w','Montevideo'),
(2, 2, 'Key on Tour - Buenos Aires 2024','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2020-12-14',  'Palermo 1412', 150.00, 'https://api.entraste.com/sc/uploads/file/a62398317b510a52ef5370cbec261de7','Montevideo'),
(3, 3, 'Room - DJ Detected b2b DJ Koolt','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-15',  'Paysandu 1351, esq Rondo', 150.00, 'https://static.ra.co/images/news/2019/dj-koolt-ade-ra-live-motd.jpg','Montevideo');

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
