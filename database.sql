--Create database prn221
--use prn221

CREATE TABLE Role (
    id int PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(100) NOT NULL
)

INSERT INTO Role (Name) VALUES
( 'admin'),
('teacher');

-- Users table
CREATE TABLE [User] (
    id INT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(255),
    password NVARCHAR(255),
    roleId INT
	FOREIGN KEY (roleId) REFERENCES Role(id)
);

INSERT INTO [User] (username, Password, roleId)
VALUES 
    ('admin', '12345',  1),
    ('tuanvm', '12345', 2),
    ('chilp', '12345', 2),
    ('sonnt5', '12345', 2),
	('khuongpd', '12345', 2),
	('hailt', '12345', 2);


CREATE TABLE Class (
  id int IDENTITY NOT NULL PRIMARY KEY,
  name nvarchar(255) NOT NULL UNIQUE
);

Insert into Class(name) Values
('SE1701'),('SE1702'), ('SE1703'),('SE1704'), ('SE1705'),('SE1706') ;

CREATE TABLE Course (
  id int IDENTITY NOT NULL PRIMARY KEY,
  code nvarchar(255) NOT NULL UNIQUE,
  name nvarchar(255) NOT NULL UNIQUE
);
Insert into Course(code, name) Values
('PRN211', 'Basic Cross-Platform Application Programming With .NET'),
('PRN221', 'Advanced Cross-Platform Application Programming With .NET'),
('PRN231', 'Building Cross-Platform Back-End Application With .NET'),
('PRU211', 'C# Programming and Unity'),
('PRM392', 'Mobile Programming');

CREATE TABLE Room (
  id int PRIMARY KEY IDENTITY(1,1),
  name nvarchar(255) NOT NULL UNIQUE
);

INSERT INTO [dbo].[room] ([name])
VALUES 
  (N'D101'),
  (N'D102'),
  (N'D103'),
  (N'D201'),
  (N'D202'),
  (N'D203'),
  (N'A101'),
  (N'A102'),
  (N'A103'),
  (N'A201'),
  (N'A202'),
  (N'A203');

CREATE TABLE [Time] (
  id int IDENTITY NOT NULL PRIMARY KEY,
  [date] nvarchar(255)
);

Insert into [Time]([date]) Values
('Monday'),
('Tuesday'),
('Wednesday'),
('Thursday'),
('Friday'),
('Saturday'),
('Sunday');

CREATE TABLE Slot (
  id INT PRIMARY KEY IDENTITY(1,1),
  name NVARCHAR(255) NOT NULL UNIQUE,
  start_time TIME(0) NOT NULL,
  end_time TIME(0) NOT NULL
);

INSERT INTO Slot (name, start_time, end_time)
VALUES 
  ('Slot1', '07:30', '09:00'),
  ('Slot2', '09:10', '10:40'),
  ('Slot3', '10:50', '12:20'),
  ('Slot4', '12:50', '14:20'),
  ('Slot5', '14:30', '16:00'),
  ('Slot6', '16:10', '17:40');


CREATE TABLE [TimeslotType] (
  id INT PRIMARY KEY IDENTITY(1,1),
  name NVARCHAR(255) NOT NULL UNIQUE,
);

Insert into [TimeslotType](name) Values
('M1'),
('M2'),
('M3'),
('M4'),
('M5'),

('E1'),
('E2'),
('E3'),
('E4'),
('E5');


CREATE TABLE [Timeslot] (
  timeId INT,
  slotId INT,
  typeId INT
  FOREIGN KEY (typeId) REFERENCES [TimeslotType](id),
  FOREIGN KEY (timeId) REFERENCES [time](id),
  FOREIGN KEY (slotId) REFERENCES slot(id),

  PRIMARY KEY (timeId, slotId),
);

Insert into [Timeslot](timeId, slotId, typeId) Values
(1,1,1),
(3,1,1),
(5,1,1),

(1,2,2),
(3,2,2),
(5,2,2),

(1,3,3),
(3,3,3),
(5,3,3),

(2,1,4),
(4,1,4),
(4,2,4),

(2,2,5),
(2,3,5),
(4,3,5),

(1,4,6),
(3,4,6),
(5,4,6),

(1,5,7),
(3,5,7),
(5,5,7),


(1,6,8),
(3,6,8),
(5,6,8),

(2,4,9),
(4,4,9),
(4,5,9),

(2,5,10),
(2,6,10),
(4,6,10);

CREATE TABLE [Timetable] (
  id int PRIMARY KEY IDENTITY(1,1),
  roomId int NOT NULL,
  courseId int NOT NULL,
  classId int NOT NULL,
  teacherId int NOT NULL,
  [timeslotTypeId] int NOT NULL,
  FOREIGN KEY (roomid) REFERENCES room (id),
  FOREIGN KEY (courseid) REFERENCES course (id),
  FOREIGN KEY (classid) REFERENCES class (id),
  FOREIGN KEY (teacherid) REFERENCES [user] (id),
  FOREIGN KEY (timeslotTypeId) REFERENCES TimeslotType (id)
);

INSERT INTO [dbo].[Timetable] ([roomid],  [courseid], [classId], [teacherid], [timeslotTypeId])
VALUES 
  (1, 1, 1, 2, 1);
