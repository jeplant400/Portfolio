




DROP TABLE IF EXISTS Meetings
GO

DROP TABLE IF EXISTS Availability
GO

DROP TABLE IF EXISTS Students
GO

DROP TABLE IF EXISTS Times
GO

DROP TABLE IF EXISTS Locations
GO

DROP TABLE IF EXISTS Advisors
GO


-- Advisors
CREATE TABLE Advisors (
    AdvisorID INT IDENTITY(1,1) PRIMARY KEY,
    AdvisorFName VARCHAR(25),
    AdvisorLName VARCHAR(40),
    AdvisorEmail VARCHAR(50)
)
GO


-- Students
CREATE TABLE Students (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    StudentFName VARCHAR(25),
    StudentLName VARCHAR(50),
    Year INT

)
GO


-- Locations
CREATE TABLE Locations (
    LocationID INT IDENTITY(1,1) PRIMARY KEY,
    Description VARCHAR(40)
)
GO


-- Times
CREATE TABLE Times (
    TimeID INT IDENTITY(1,1) PRIMARY KEY,
    StartTime TIME,
    EndTime TIME
)
GO


-- Availability
CREATE TABLE Availability (
    AvailabilityID INT IDENTITY(1,1) PRIMARY KEY,
    AdvisorID INT,
    Date DATETIME,
    TimeID INT,
    LocationID INT,
    IsTaken BIT,
	--Date was DATE and not DATETIME but was changed bc of C#

    FOREIGN KEY (AdvisorID) REFERENCES Advisors(AdvisorID),
    FOREIGN KEY (TimeID) REFERENCES Times(TimeID),
    FOREIGN KEY (LocationID) REFERENCES Locations(LocationID)
)
GO


-- Meetings
CREATE TABLE Meetings (
    MeetingID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT,
    AvailabilityID INT,

    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (AvailabilityID) REFERENCES Availability(AvailabilityID)
)
GO

ALTER TABLE Availability
ADD CONSTRAINT UQ_Availability UNIQUE (AdvisorID, Date, TimeID, LocationID);

ALTER TABLE Meetings
ADD CONSTRAINT UQ_Meetings UNIQUE (StudentID, AvailabilityID);

ALTER TABLE Meetings
ADD CONSTRAINT UQ_Meetings_AvailabilityID UNIQUE (AvailabilityID);