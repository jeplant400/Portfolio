--stored procedures




USE b320
GO



-------------------------------------------- Get (i.e., SELECT) SProc

DROP PROCEDURE IF EXISTS sp_GetAdvisor;
GO

CREATE PROCEDURE sp_GetAdvisor
	@AdvisorID INT = NULL
AS
BEGIN
	IF (@AdvisorID IS NULL)
		SELECT *
		FROM Advisors;
	ELSE
		SELECT AdvisorID, AdvisorFName, AdvisorLName, AdvisorEmail
		FROM Advisors
		WHERE AdvisorID = @AdvisorID;
END
GO

-------------------------------------------- Insert SProc

DROP PROCEDURE IF EXISTS sp_InsertAdvisor;
GO

CREATE PROCEDURE sp_InsertAdvisor
	@AdvisorFName VARCHAR(25),
	@AdvisorLName VARCHAR(40) = NULL,
	@AdvisorEmail VARCHAR(50) = NULL
AS
BEGIN
	INSERT INTO [Advisors]
	([AdvisorFName], [AdvisorLName], [AdvisorEmail])
	VALUES
	(@AdvisorFName, @AdvisorLName, @AdvisorEmail)
END
GO

-------------------------------------------- Update SProc

DROP PROCEDURE IF EXISTS sp_UpdateAdvisor;
GO

CREATE PROCEDURE sp_UpdatePet
	@AdvisorID INT,
	@AdvisorFName VARCHAR(25),
	@AdvisorLName VARCHAR(40) = NULL,
	@AdvisorEmail VARCHAR(50) = NULL
AS
BEGIN
	UPDATE [Advisors]
	SET [AdvisorFName] =  @AdvisorFName, 
		[AdvisorLName] = @AdvisorLName, 
		[AdvisorEmail] = @AdvisorEmail 
	WHERE AdvisorID = @AdvisorID
END
GO

-------------------------------------------- Delete SProc

DROP PROCEDURE IF EXISTS sp_DeleteAdvisor;
GO

CREATE PROCEDURE sp_DeletePet
	@AdvisorID INT
AS
BEGIN
	DELETE 
	FROM [Advisors]
	WHERE AdvisorID = @AdvisorID
END
GO








DROP PROCEDURE IF EXISTS sp_GetAdvisorID
GO

CREATE Procedure sp_GetAdvisorID
	@AdvisorFName VARCHAR(25),
	@AdvisorLName VARCHAR(40)
AS
	BEGIN
		SELECT AdvisorID
			FROM Advisors
			WHERE AdvisorFName = @AdvisorFName
				AND
				  AdvisorLName = @AdvisorLName
	END
GO






----------------------------------------------------------------------------------





-- GET Students
DROP PROCEDURE IF EXISTS sp_GetStudent
GO
CREATE PROCEDURE sp_GetStudent
    @StudentID INT = NULL
AS
BEGIN
    IF @StudentID IS NULL
        SELECT *
            FROM Students
    ELSE
        SELECT *
            FROM Students
            WHERE StudentID = @StudentID
END
GO

-- INSERT Student
DROP PROCEDURE IF EXISTS sp_InsertStudent
GO
CREATE PROCEDURE sp_InsertStudent
    @StudentFName VARCHAR(25),
    @StudentLName VARCHAR(50),
    @Year INT
AS
BEGIN
    INSERT INTO Students (StudentFName, StudentLName, Year)
    VALUES (@StudentFName, @StudentLName, @Year);
    SELECT SCOPE_IDENTITY() AS NewStudentID
END
GO

-- UPDATE Student
DROP PROCEDURE IF EXISTS sp_UpdateStudent
GO
CREATE PROCEDURE sp_UpdateStudent
    @StudentID INT,
    @StudentFName VARCHAR(25),
    @StudentLName VARCHAR(50),
    @Year INT
AS
BEGIN
    UPDATE Students
    SET StudentFName = @StudentFName,
        StudentLName = @StudentLName,
        Year = @Year
    WHERE StudentID = @StudentID
END
GO

-- DELETE Student
DROP PROCEDURE IF EXISTS sp_DeleteStudent
GO
CREATE PROCEDURE sp_DeleteStudent
    @StudentID INT
AS
BEGIN
    DELETE FROM Students
        WHERE StudentID = @StudentID
END
GO



----------------------------------------------------




-- GET Meetings
DROP PROCEDURE IF EXISTS sp_GetMeeting
GO
CREATE PROCEDURE sp_GetMeeting
    @MeetingID INT = NULL
AS
BEGIN
    IF @MeetingID IS NULL
        SELECT * FROM Meetings
    ELSE
        SELECT * FROM Meetings WHERE MeetingID = @MeetingID
END
GO

-- INSERT Meeting
DROP PROCEDURE IF EXISTS sp_InsertMeeting
GO
CREATE PROCEDURE sp_InsertMeeting
    @StudentID INT,
    @AvailabilityID INT
AS
BEGIN
    INSERT INTO Meetings (StudentID, AvailabilityID)
    VALUES (@StudentID, @AvailabilityID)
    SELECT SCOPE_IDENTITY() AS NewMeetingID
END
GO

-- UPDATE Meeting
DROP PROCEDURE IF EXISTS sp_UpdateMeeting;
GO
CREATE PROCEDURE sp_UpdateMeeting
    @MeetingID INT,
    @StudentID INT,
    @AvailabilityID INT
AS
BEGIN
    UPDATE Meetings
    SET StudentID = @StudentID,
        AvailabilityID = @AvailabilityID
    WHERE MeetingID = @MeetingID
END
GO

-- DELETE Meeting
DROP PROCEDURE IF EXISTS sp_DeleteMeeting
GO
CREATE PROCEDURE sp_DeleteMeeting
    @MeetingID INT
AS
BEGIN
    DELETE FROM Meetings WHERE MeetingID = @MeetingID
END
GO



------------------------------------------




-- GET Availability
DROP PROCEDURE IF EXISTS sp_GetAvailability
GO
CREATE PROCEDURE sp_GetAvailability
    @AvailabilityID INT = NULL
AS
BEGIN
    IF @AvailabilityID IS NULL
        SELECT * FROM Availability
    ELSE
        SELECT * FROM Availability WHERE AvailabilityID = @AvailabilityID
END
GO

-- INSERT Availability
DROP PROCEDURE IF EXISTS sp_InsertAvailability
GO
CREATE PROCEDURE sp_InsertAvailability
    @AdvisorID INT,
    @Date DATE,
    @TimeID INT,
    @LocationID INT,
    @IsTaken BIT
AS
BEGIN
    INSERT INTO Availability (AdvisorID, Date, TimeID, LocationID, IsTaken)
    VALUES (@AdvisorID, @Date, @TimeID, @LocationID, @IsTaken)
    SELECT SCOPE_IDENTITY() AS NewAvailabilityID
END
GO

-- UPDATE Availability
DROP PROCEDURE IF EXISTS sp_UpdateAvailability
GO
CREATE PROCEDURE sp_UpdateAvailability
    @AvailabilityID INT,
    @AdvisorID INT,
    @Date DATE,
    @TimeID INT,
    @LocationID INT,
    @IsTaken BIT
AS
BEGIN
    UPDATE Availability
    SET AdvisorID = @AdvisorID,
        Date = @Date,
        TimeID = @TimeID,
        LocationID = @LocationID,
        IsTaken = @IsTaken
    WHERE AvailabilityID = @AvailabilityID
END
GO

-- DELETE Availability
DROP PROCEDURE IF EXISTS sp_DeleteAvailability;
GO
CREATE PROCEDURE sp_DeleteAvailability
    @AvailabilityID INT
AS
BEGIN
    DELETE FROM Availability WHERE AvailabilityID = @AvailabilityID
END
GO

-- Availability TRIGGER

DROP TRIGGER IF EXISTS tgrCheckTaken
GO

CREATE TRIGGER tgrCheckTaken ON Availability INSTEAD OF INSERT
AS
	DECLARE @AvailabilityID INT,
     		@AdvisorID INT,
	    	@Date DATE,
		    @TimeID INT,
		    @LocationID INT,
			@IsTaken BIT

	SET @IsTaken =
	(
		SELECT IsTaken
			FROM Availability
			WHERE AvailabilityID = @AvailabilityID
	)

	IF @IsTaken = 0
		BEGIN
			UPDATE Availability
				SET AdvisorID = @AdvisorID,
				    Date = @Date,
				    TimeID = @TimeID,
					LocationID = @LocationID,
					IsTaken = @IsTaken
				WHERE AvailabilityID = @AvailabilityID
		END
	ELSE
		THROW 50027, 'Availability already taken.', 1
	GO


------------------------------------------------




-- GET Times
DROP PROCEDURE IF EXISTS sp_GetTime
GO
CREATE PROCEDURE sp_GetTime
    @TimeID INT = NULL
AS
BEGIN
    IF @TimeID IS NULL
        SELECT * FROM Times
    ELSE
        SELECT * FROM Times WHERE TimeID = @TimeID
END
GO

-- INSERT Time
DROP PROCEDURE IF EXISTS sp_InsertTime
GO
CREATE PROCEDURE sp_InsertTime
    @StartTime TIME,
    @EndTime TIME
AS
BEGIN
    INSERT INTO Times (StartTime, EndTime)
    VALUES (@StartTime, @EndTime)
    SELECT SCOPE_IDENTITY() AS NewTimeID
END
GO

-- UPDATE Time
DROP PROCEDURE IF EXISTS sp_UpdateTime
GO
CREATE PROCEDURE sp_UpdateTime
    @TimeID INT,
    @StartTime TIME,
    @EndTime TIME
AS
BEGIN
    UPDATE Times
    SET StartTime = @StartTime,
        EndTime = @EndTime
    WHERE TimeID = @TimeID
END
GO

-- DELETE Time
DROP PROCEDURE IF EXISTS sp_DeleteTime
GO
CREATE PROCEDURE sp_DeleteTime
    @TimeID INT
AS
BEGIN
    DELETE FROM Times WHERE TimeID = @TimeID
END
GO






--------------------------------------




-- GET Locations
DROP PROCEDURE IF EXISTS sp_GetLocation
GO
CREATE PROCEDURE sp_GetLocation
    @LocationID INT = NULL
AS
BEGIN
    IF @LocationID IS NULL
        SELECT * FROM Locations
    ELSE
        SELECT * FROM Locations WHERE LocationID = @LocationID
END
GO

-- INSERT Location
DROP PROCEDURE IF EXISTS sp_InsertLocation
GO
CREATE PROCEDURE sp_InsertLocation
    @Description VARCHAR(40)
AS
BEGIN
    INSERT INTO Locations (Description)
    VALUES (@Description);
    SELECT SCOPE_IDENTITY() AS NewLocationID
END
GO

-- UPDATE Location
DROP PROCEDURE IF EXISTS sp_UpdateLocation
GO
CREATE PROCEDURE sp_UpdateLocation
    @LocationID INT,
    @Description VARCHAR(40)
AS
BEGIN
    UPDATE Locations
    SET Description = @Description
    WHERE LocationID = @LocationID
END
GO

-- DELETE Location
DROP PROCEDURE IF EXISTS sp_DeleteLocation
GO
CREATE PROCEDURE sp_DeleteLocation
    @LocationID INT
AS
BEGIN
    DELETE FROM Locations WHERE LocationID = @LocationID
END
GO



---------------------------------