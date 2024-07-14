DROP TABLE IF EXISTS ticketpurchases;
DROP TABLE IF EXISTS eventtickets;
DROP TABLE IF EXISTS events;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS roles;

-- Create the table Roles
CREATE TABLE roles (
    id INT PRIMARY KEY,
    type VARCHAR(255) UNIQUE NOT NULL
);

-- Create the table Users
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    roleid INT NOT NULL REFERENCES roles(id),
    firstname VARCHAR(100) NOT NULL,
    lastname VARCHAR(100) NOT NULL,
    phone VARCHAR(20),
    email VARCHAR(255) UNIQUE NOT NULL,
    passwordsalt BYTEA NOT NULL,
    passwordhash BYTEA NOT NULL,
    tokenexpires TIMESTAMP,
    tokencreated TIMESTAMP,
    token VARCHAR(255)
);

-- Create the table Events
CREATE TABLE events (
    id INT PRIMARY KEY,
    userid INT REFERENCES users(id),
    name VARCHAR(255) NOT NULL,
    description VARCHAR(8000),
    date DATE NOT NULL,
    location VARCHAR(255),
    photo VARCHAR(8000),
    department VARCHAR(255)
);

-- Create the table EventTickets with id, entry, and price columns
CREATE TABLE eventtickets (
    id SERIAL PRIMARY KEY,
    eventid INT REFERENCES events(id),
    entry VARCHAR(255) NOT NULL,
    quantity INT NOT NULL,
    price DECIMAL(10, 2) NOT NULL
);

-- Create the table TicketPurchases to relate Users with EventTickets
CREATE TABLE ticketpurchases (
    userid INT REFERENCES users(id),
    eventid INT REFERENCES events(id),
    ticket_type INT REFERENCES eventtickets(id),
    quantity_purchased INT,
    used BOOLEAN DEFAULT FALSE,
    PRIMARY KEY (userid, eventid, ticket_type)
);

-- Insert roles
INSERT INTO roles (id, type) VALUES
(1, 'User'),
(2, 'RRPP'),
(3, 'Producer'),
(4, 'Administrator');

-- Insert users
INSERT INTO users (roleid, firstname, lastname, phone, email, passwordsalt, passwordhash, tokenexpires, tokencreated, token) 
VALUES 
(1, 'John', 'Doe', '123456789', 'john.doe@example.com', 'salt123', 'hash123', '2025-02-24', '2024-02-24', 'token123'),
(2, 'Alice', 'Smith', '987654321', 'alice.smith@example.com', 'salt456', 'hash456', '2025-02-24', '2024-02-24', 'token456'),
(3, 'Bob', 'Johnson', '555555555', 'bob.johnson@example.com', 'salt789', 'hash789', '2025-02-24', '2024-02-24', 'token789');

-- Insert events
INSERT INTO events (id, userid, name, description, date, location, photo, department) VALUES
(12, 1, 'Boris Brejcha b2b Ann Clue', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-02', 'Rambla, av Brasil', 'https://scontent.fmvd3-1.fna.fbcdn.net/v/t1.18169-9/18671209_1005554409580602_324848815674572030_n.jpg?_nc_cat=102&ccb=1-7&_nc_sid=5f2048&_nc_ohc=XCkMVA6HDYQAX9RoSCo&_nc_ht=scontent.fmvd3-1.fna&oh=00_AfAKiHCSbIIMJiy-tXUnJkOSoQ2Ib4QsBvl3l01QmcsgVA&oe=662DCD08','Montevideo'),
(10, 1, 'Senderos Phoro - Quebrada, Punta del Este', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-03', 'Quebrada, Maldonado', 'https://underground.com.uy/wp-content/uploads/2017/03/tapa-senderos-1.png','Maldonado'),
(11, 1, 'Kidd Keo - Montevideo Music Box','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-01',  'Montevideo Music Box', 'https://scontent.fmvd3-1.fna.fbcdn.net/v/t1.18169-9/17498749_741198912718445_1364156188934314741_n.jpg?_nc_cat=109&ccb=1-7&_nc_sid=5f2048&_nc_ohc=jlywwkX7xTIAX-79MPO&_nc_ht=scontent.fmvd3-1.fna&oh=00_AfApl7OwrFzY1onZqrwEwu6LFj5lDzTWC56cDgsTc0RNfw&oe=662DCCB5','Montevideo'),
(4, 1, 'EMILE - Molen Live, Agustin B2B Decker', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-04', 'Centro y yaguaron', 'https://api.entraste.com/sc/uploads/file/c2ad16cff9b81acb21b6e36c4075dee3','Montevideo'),
(5, 1, 'Error 909 - Punta del Este, Maldonado', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-03', 'Chacra la Quebrada, Maldonado', 'https://api.entraste.com/sc/uploads/file/0707fbb8b592d1592fa71885ff9bb2d0','Maldonado'),
(6, 1, 'EL KUELGUE - Teatro de verano 3/2024', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-11-01', 'Teatro de Verano', 'https://www.saladelmuseo.com.uy/media/zoo/images/cuadrada_1_1_9cab0ac3adb315094ea891fb9d9be683.png','Montevideo'),
(7, 1, 'Sound Festival - 2024','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-10-12',  'Rambla, av Brasil', 'https://imgproxy.ra.co/_/quality:66/aHR0cHM6Ly9pbWFnZXMucmEuY28vOTU0MzNkZGVlNjU0MTU3YjhhNzg5MWU5YzlmMmEzN2FjZmEwMTg5Zi5qcGc=','Montevideo'),
(8, 1, 'ACRU a Uruguay - Montevideo Music Box', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-01', 'Rambla y Av Brasil', 'https://pbs.twimg.com/media/FyrgnicWABE0XLE.jpg:large','Montevideo'),
(9, 1, 'YSY A 2024 - Antel Arena','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-11-01',  'Antel Arena', 'https://www.saladelmuseo.com.uy/media/zoo/images/YSY_Uruguay_feed_instagram_24_3ac18770ef6feb13962b430679c59552.jpg','Canelones'),
(1, 1, 'PHONOTEQUE LIVE PHORO APERTURA 2024 NO TE LO PIERDAS', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2020-12-12', 'Piedra Alta 1254', 'https://static1.squarespace.com/static/58a39fb503596e0fdbb001a6/58aa378bff7c50f538e999a1/5c599dba6e9a7f0c4232b02e/1549377032782/51398940_2545058242177592_4914365089764605952_o.jpg?format=1500w','Montevideo'),
(2, 2, 'Key on Tour - Buenos Aires 2024','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2020-12-14',  'Palermo 1412', 'https://api.entraste.com/sc/uploads/file/a62398317b510a52ef5370cbec261de7','Montevideo'),
(3, 3, 'Room - DJ Detected b2b DJ Koolt','Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '2024-12-15',  'Paysandu 1351, esq Rondo', 'https://static.ra.co/images/news/2019/dj-koolt-ade-ra-live-motd.jpg','Montevideo');

-- Insert associations of ticket types with events
INSERT INTO eventtickets (entry, eventid, quantity, price) VALUES
('General Admission',1, 100, 1000.00), -- 100 general admission tickets for event 1
('VIP',1, 50, 1500.00),                -- 50 VIP tickets for event 1
('Premium',1, 30, 2000.00),            -- 30 Premium tickets for event 1

('General Admission',2, 150, 1000.00), -- 150 general admission tickets for event 2
('VIP',2, 75, 1500.00),                -- 75 VIP tickets for event 2
('Mesa',2, 50, 2000.00),               -- 50 Mesa tickets for event 2

('General Admission', 3, 200, 1000.00), -- 200 general admission tickets for event 3
('VIP', 3, 100, 1500.00),               -- 100 VIP tickets for event 3
('Box', 3, 20, 3000.00),               -- 20 Box tickets for event 3

('General Admission', 12, 100, 1000.00), -- 100 general admission tickets for event 12
('VIP', 12, 50, 1500.00),               -- 50 VIP tickets for event 12
('Premium', 12, 30, 2000.00),           -- 30 Premium tickets for event 12

('General Admission', 10, 150, 1000.00), -- 150 general admission tickets for event 10
('VIP', 10, 75, 1500.00),                -- 75 VIP tickets for event 10
('Mesa', 10, 50, 2000.00),               -- 50 Mesa tickets for event 10

('General Admission', 11, 200, 1000.00), -- 200 general admission tickets for event 11
('VIP', 11, 100, 1500.00),               -- 100 VIP tickets for event 11
('Box', 11, 20, 3000.00),                -- 20 Box tickets for event 11

('General Admission', 4, 100, 1000.00),  -- 100 general admission tickets for event 4
('VIP', 4, 50, 1500.00),                 -- 50 VIP tickets for event 4
('Exclusive', 4, 25, 2000.00),           -- 25 Exclusive tickets for event 4

('General Admission', 5, 200, 1000.00),  -- 200 general admission tickets for event 5
('VIP', 5, 100, 1500.00),                -- 100 VIP tickets for event 5
('Lounge', 5, 30, 2500.00),              -- 30 Lounge tickets for event 5

('General Admission', 6, 150, 1000.00),  -- 150 general admission tickets for event 6
('VIP', 6, 75, 1500.00),                 -- 75 VIP tickets for event 6
('Table', 6, 40, 2000.00),               -- 40 Table tickets for event 6

('General Admission', 7, 250, 1000.00),  -- 250 general admission tickets for event 7
('VIP', 7, 125, 1500.00),                -- 125 VIP tickets for event 7
('Front Row', 7, 60, 3000.00),           -- 60 Front Row tickets for event 7

('General Admission', 8, 100, 1000.00),  -- 100 general admission tickets for event 8
('VIP', 8, 50, 1500.00),                 -- 50 VIP tickets for event 8
('Balcony', 8, 20, 2000.00),             -- 20 Balcony tickets for event 8

('General Admission', 9, 200, 1000.00),  -- 200 general admission tickets for event 9
('VIP', 9, 100, 1500.00),                -- 100 VIP tickets for event 9
('Stage Side', 9, 50, 2500.00);          -- 50 Stage Side tickets for event 9

-- Insert ticket purchases
INSERT INTO ticketpurchases (userid, eventid, ticket_type, quantity_purchased, used) VALUES
(1, 12, 1, 5, TRUE),   -- User1 purchased 5 general admission tickets for event 12, all used
(2, 12, 2, 2, FALSE),  -- User2 purchased 2 VIP tickets for event 12, none used
(3, 10, 4, 10, TRUE);  -- User3 purchased 10 general admission tickets for event 10, all used
