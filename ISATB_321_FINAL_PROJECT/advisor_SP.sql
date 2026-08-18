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

