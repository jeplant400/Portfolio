/*
    Author: Emrys Quick
    Purpose: Writing queries
    Create Date: 12/9/2024
    Last Edit Date: 12/9/2024
*/



DROP PROCEDURE CountByMajorYear
GO

DROP PROCEDURE CountByMajorYearPassing
GO

-- QUERY 1: Emrys Quick
    DROP PROCEDURE StudentTranscript
    GO

    CREATE PROCEDURE StudentTranscript @StudentID INT
    AS
    SELECT StudentFName, StudentLName, CourseLabel, Grade, AcadPeriodName
        FROM StudentEnrollment
        INNER JOIN Students
            ON Students.StudentID = StudentEnrollment.StudentID
        INNER JOIN CourseOfferings
            ON CourseOfferings.CourseOfferingID = StudentEnrollment.CourseOfferingID
        INNER JOIN Courses
            ON Courses.CourseID = CourseOfferings.CourseID
        INNER JOIN Grades
            ON Grades.GradeID = StudentEnrollment.GradeID
        INNER JOIN AcadPeriods
            ON AcadPeriods.AcadPeriodID = CourseOfferings.AcadPeriodID
        INNER JOIN OrderedAcadPeriods
            ON OrderedAcadPeriods.AcadPeriodID = AcadPeriods.AcadPeriodID
        WHERE StudentEnrollment.StudentID = @StudentID
        ORDER BY OrderedAcadPeriods.AcadPeriodID
    GO

    EXEC StudentTranscript @StudentID = 148
    GO


-- QUERY 2: Emrys Quick

    CREATE PROCEDURE CountByMajorYear
    AS
    SELECT StudentYear, MajorName, COUNT(Students.MajorID) AS [MajorCount]
        FROM Students
        INNER JOIN Majors
            ON Majors.MajorID = Students.MajorID
        GROUP BY StudentYear, MajorName

    EXEC CountByMajorYear
    GO


-- QUERY 3: Emrys Quick
    
    DROP PROCEDURE CountByMajorYearPassing
    GO

    CREATE PROCEDURE CountByMajorYearPassing @AcadPeriodID INT
    AS
        WITH StudentCount AS
        (
            SELECT COUNT(StudentEnrollment.StudentID) AS StudentPassingCount, StudentYear, MajorName
                FROM StudentEnrollment
                INNER JOIN CourseOfferings
                    ON CourseOfferings.CourseOfferingID = StudentEnrollment.CourseOfferingID
                INNER JOIN Students
                    ON Students.StudentID = StudentEnrollment.StudentID     
                INNER JOIN Majors
                    ON Majors.MajorID = Students.MajorID      
                WHERE CourseOfferings.AcadPeriodID = @AcadPeriodID
                    AND
                    GradeID <= 5
                    AND
                    AcadPeriodID = @AcadPeriodID
                GROUP BY Students.StudentID, StudentYear, MajorName
        )
        SELECT *
            FROM test
            WHERE StudentPassingCount >= 5

    EXEC CountByMajorYearPassing @AcadPeriodID = 2
    GO

-- QUERY 4: Emrys Quick

    WITH CourseStudentTotal AS
    (
        SELECT Courses.CourseID, COUNT(StudentID) AS [TotalStudents]
            FROM StudentEnrollment
            INNER JOIN CourseOfferings
                ON CourseOfferings.CourseOfferingID = StudentEnrollment.CourseOfferingID
            INNER JOIN Courses
                ON Courses.CourseID = CourseOfferings.CourseID
            INNER JOIN Subjects
                ON Subjects.SubjectID = Courses.SubjectID
            WHERE (SubjectAbbr = 'CSCI'
                OR
                SubjectAbbr = 'ISAT')
                 AND GradeID <=5
            GROUP BY Courses.CourseID
    )
        SELECT Title, GradeID, TotalStudents, COUNT(CourseOfferings.CourseOfferingID) AS [PassingCount], (CAST(COUNT(StudentEnrollment.StudentID) AS DECIMAL (4,2)) / CAST(TotalStudents AS DECIMAL(4,2)) * 100) AS PercentPassing, TotalStudents - COUNT(*) AS FailingCount, (100 - (CAST(COUNT(StudentEnrollment.StudentID) AS DECIMAL (4,2)) / CAST(TotalStudents AS DECIMAL(4,2)) * 100)) AS PercentFailing
        FROM CourseStudentTotal
        INNER JOIN CourseOfferings
            ON CourseOfferings.CourseID = CourseStudentTotal.CourseID
        INNER JOIN StudentEnrollment
            ON StudentEnrollment.CourseOfferingID = CourseOfferings.CourseOfferingID
        INNER JOIN Students
            ON Students.StudentID = StudentEnrollment.StudentID
        INNER JOIN Courses
            ON Courses.CourseID = CourseOfferings.CourseID
        INNER JOIN Subjects
            ON Subjects.SubjectID = Courses.SubjectID
        WHERE (SubjectAbbr = 'CSCI'
            OR
              SubjectAbbr = 'ISAT')
            AND
             GradeID <= 5
        GROUP BY Title, GradeID, TotalStudents

-- Query 5: Emrys Quick

    SELECT CourseOfferings.CourseOfferingID, Courses.CourseID, CourseLabel, COUNT(CourseOfferings.CourseOfferingID) AS [StudentCount]
        FROM StudentEnrollment
        INNER JOIN CourseOfferings
            ON CourseOfferings.CourseOfferingID = StudentEnrollment.CourseOfferingID
        INNER JOIN Courses
            ON Courses.CourseID = CourseOfferings.CourseID
        INNER JOIN Subjects
            ON Subjects.SubjectID = Courses.SubjectID
        WHERE SubjectAbbr = 'ISAT'
            OR
            SubjectAbbr = 'CSCI'
        GROUP BY CourseOfferings.CourseOfferingID, Courses.CourseID, CourseLabel
        HAVING COUNT(CourseOfferings.CourseOfferingID) < 10
    
-- QUERY 6: James Plant
DROP Procedure GPAPast
GO

CREATE PROCEDURE GPAPast @OrderedPeriodID INT
AS
BEGIN
    SELECT 
        Students.StudentGPALetter, 
        COUNT(DISTINCT Students.StudentID) AS GPACount 
    FROM 
        StudentEnrollment
    INNER JOIN Grades 
		ON StudentEnrollment.GradeID = Grades.GradeID
    INNER JOIN CourseOfferings 
		ON CourseOfferings.CourseOfferingID = StudentEnrollment.CourseOfferingID
    INNER JOIN AcadPeriods
		ON AcadPeriods.AcadPeriodID = CourseOfferings.AcadPeriodID
    INNER JOIN OrderedAcadPeriods
		ON OrderedAcadPeriods.AcadPeriodID = AcadPeriods.AcadPeriodID
    INNER JOIN 
        Students ON StudentEnrollment.StudentID = Students.StudentID
    WHERE OrderedPeriodID > @OrderedPeriodID
    GROUP BY Students.StudentGPALetter
    ORDER BY Students.StudentGPALetter
END
GO

EXEC GPAPast @OrderedPeriodID = 2

-- QUERY 7: James Plant
SELECT StudentID, ROUND(SUM(ActualCreditHours * Points) /SUM(ActualCreditHours), 2) as GPA
	FROM StudentEnrollment
	INNER JOIN Grades
		ON StudentEnrollment.GradeID = Grades.GradeID
	INNER JOIN CourseOfferings
		ON CourseOfferings.CourseOfferingID = StudentEnrollment.CourseOfferingID	
	GROUP BY StudentID
	ORDER BY StudentID
GO
DROP PROCEDURE GPA
GO

CREATE PROCEDURE Probation @OrderedPeriodID INT
AS
	SELECT StudentID, SUM(ActualCreditHours * Points) /SUM(ActualCreditHours) as GPA
		FROM StudentEnrollment
		INNER JOIN Grades
			ON StudentEnrollment.GradeID = Grades.GradeID
		INNER JOIN CourseOfferings
			ON CourseOfferings.CourseOfferingID = StudentEnrollment.CourseOfferingID
		INNER JOIN AcadPeriods
			ON AcadPeriods.AcadPeriodID = CourseOfferings.AcadPeriodID
		INNER JOIN OrderedAcadPeriods
			ON OrderedAcadPeriods.AcadPeriodID = AcadPeriods.AcadPeriodID
		WHERE OrderedPeriodID > @OrderedPeriodID 
		GROUP BY StudentID
		HAVING ROUND(SUM(ActualCreditHours * Points) / SUM(ActualCreditHours), 2) < 2.0
		ORDER BY StudentID
GO


EXEC Probation @OrderedPeriodID = 2


-- QUERY 8: James Plant
SELECT StudentID, ROUND(SUM(ActualCreditHours * Points) /SUM(ActualCreditHours), 2) as GPA
	FROM StudentEnrollment
	INNER JOIN Grades
		ON StudentEnrollment.GradeID = Grades.GradeID
	INNER JOIN CourseOfferings
		ON CourseOfferings.CourseOfferingID = StudentEnrollment.CourseOfferingID	
	GROUP BY StudentID
	ORDER BY StudentID
GO

	CREATE PROCEDURE StudentsLeftProbation @PreviousPeriodID INT, @CurrentPeriodID INT
	AS
	BEGIN
    WITH PreviousGPA AS (
        SELECT StudentID, ROUND(SUM(ActualCreditHours * Points) / SUM(ActualCreditHours), 2) AS GPA
        FROM StudentEnrollment
        INNER JOIN Grades 
			ON StudentEnrollment.GradeID = Grades.GradeID
        INNER JOIN CourseOfferings 
			ON CourseOfferings.CourseOfferingID = StudentEnrollment.CourseOfferingID
        INNER JOIN AcadPeriods 
			ON AcadPeriods.AcadPeriodID = CourseOfferings.AcadPeriodID
        INNER JOIN OrderedAcadPeriods 
			ON OrderedAcadPeriods.AcadPeriodID = AcadPeriods.AcadPeriodID
        WHERE OrderedPeriodID = @PreviousPeriodID
        GROUP BY StudentID
        HAVING ROUND(SUM(ActualCreditHours * Points) / SUM(ActualCreditHours), 2) < 2.0),

    CurrentGPA AS (
        SELECT StudentID, ROUND(SUM(ActualCreditHours * Points) / SUM(ActualCreditHours), 2) AS GPA
        FROM StudentEnrollment
        INNER JOIN Grades 
			ON StudentEnrollment.GradeID = Grades.GradeID
        INNER JOIN CourseOfferings 
			ON CourseOfferings.CourseOfferingID = StudentEnrollment.CourseOfferingID
        INNER JOIN AcadPeriods 
			ON AcadPeriods.AcadPeriodID = CourseOfferings.AcadPeriodID
        INNER JOIN OrderedAcadPeriods 
			ON OrderedAcadPeriods.AcadPeriodID = AcadPeriods.AcadPeriodID
        WHERE OrderedPeriodID = @CurrentPeriodID
        GROUP BY StudentID
        HAVING ROUND(SUM(ActualCreditHours * Points) / SUM(ActualCreditHours), 2) >= 2.0)

    SELECT CurrentGPA.StudentID, PreviousGPA.GPA AS PreviousGPA, CurrentGPA.GPA AS CurrentGPA
    FROM PreviousGPA
    INNER JOIN CurrentGPA 
		ON PreviousGPA.StudentID = CurrentGPA.StudentID
    ORDER BY CurrentGPA.StudentID;
END
GO

EXEC StudentsLeftProbation  @PreviousPeriodID = 20, @CurrentPeriodID = 3
GO

-- QUERY 9: Emrys Quick

WITH StudentData AS 
(
    SELECT AcadPeriods.AcadPeriodID, children.MajorID, StudentEnrollment.StudentID, Students.StudentYear, children.StudentGPANum
        FROM Students children
        INNER JOIN StudentEnrollment
            ON StudentEnrollment.StudentID = children.StudentID
        INNER JOIN CourseOfferings
            ON CourseOfferings.CourseOfferingID = StudentEnrollment.CourseOfferingID
        INNER JOIN AcadPeriods
            ON AcadPeriods.AcadPeriodID = CourseOfferings.AcadPeriodID
        INNER JOIN Students
            ON Students.StudentID = StudentEnrollment.StudentID
),
ToGraduate AS
(
    SELECT DISTINCT sdata.StudentID, sdata.AcadPeriodID
        FROM StudentData sdata
        WHERE sdata.StudentYear = 4
            AND
              StudentGPANum >= 2
			AND StudentYear NOT IN
			(
				SELECT 1
					FROM StudentData sd
					WHERE sd.StudentID = sdata.StudentID
						AND
						  sd.AcadPeriodID > sdata.AcadPeriodID
			)
)
    SELECT sdat.StudentYear,
		   MajorName,
		   COUNT(DISTINCT sdat.StudentID) AS AllStudents,
           sdat.StudentGPANum,
           COUNT(DISTINCT CASE WHEN Students.StudentGPANum < 2 THEN sdat.StudentID END) AS OnProbation,
           COUNT(DISTINCT CASE WHEN (sdat.StudentYear = 4) AND (sdat.StudentGPANum > 2) THEN sdat.StudentID END) AS Graduating
        FROM StudentData sdat 
		INNER JOIN Students
			ON Students.StudentID = sdat.StudentID
		INNER JOIN StudentEnrollment
			ON StudentEnrollment.StudentID = Students.StudentID
		INNER JOIN Majors
			ON Majors.MajorID = Students.MajorID
        GROUP BY sdat.StudentYear, Students.StudentGPANum, MajorName, sdat.StudentGPANum, sdat.AcadPeriodID, Students.MajorID
		ORDER BY sdat.StudentYear, Students.MajorID, sdat.StudentGPANum, sdat.AcadPeriodID
















DROP Procedure CountByMajorGPAProbGrad
GO

CREATE PROCEDURE CountByMajorGPAProbGrad
    AS
    SELECT MajorName, COUNT(childs.MajorID) AS [MajorCount], AVG(CAST(StudentGPANum AS DECIMAL (4,2))) AS GPA,
	(
		SELECT COUNT(*)
			FROM Students
			WHERE StudentGPANum < 2.00
				AND
				  StudentYear = childs.StudentYear
				AND
				  MajorName = maj.MajorName
	) AS OnProbation,
	(
		SELECT COUNT(*)
			FROM Students
			WHERE StudentGPANum >= 2
				AND
				  StudentYear = childs.StudentYear
	) AS Graduating
        FROM Students childs
        INNER JOIN Majors maj
            ON maj.MajorID = childs.MajorID
		WHERE childs.StudentYear != 5
        GROUP BY childs.StudentYear, MajorName, StudentGPANum

EXEC CountByMajorGPAProbGrad
GO

/*
   QUERY 10: Display list of students repeating courses.
   Need the courses, the semesters in which it was taken,
   and both grades. If course is currently being taken,
   grade should be “In Progress” 
*/

    SELECT StudentEnrollment.StudentID, Title, MAX(Grade) AS [FirstTake], MAX(AcadPeriodID) AS FirstSemesterTaken, MIN(Grade) AS [SecondTake], MIN(AcadPeriodID) AS SecondSemesterTaken
        FROM StudentEnrollment
        INNER JOIN Grades
            ON Grades.GradeID = StudentEnrollment.GradeID
        INNER JOIN CourseOfferings
            ON CourseOfferings.CourseOfferingID = StudentEnrollment.CourseOfferingID
        INNER JOIN Courses
            ON Courses.CourseID = CourseOfferings.CourseID
        WHERE SubjectID != 10
        GROUP BY StudentEnrollment.StudentID, Title, Courses.CourseID/* , Grade */
        HAVING COUNT(Courses.CourseID) > 1 
    GO

-- EXTRA QUERY

    DROP PROCEDURE ErdeiSemester
    GO

    CREATE PROCEDURE ErdeiSemester  @AcadPeriodName VARCHAR(15)
    AS
    SELECT CourseOfferingID, Courses.CourseID, Title, AcadPeriodName
        FROM CourseOfferings
        INNER JOIN AcadPeriods
            ON AcadPeriods.AcadPeriodID = CourseOfferings.AcadPeriodID
        INNER JOIN Courses
            ON Courses.CourseID = CourseOfferings.CourseID
        INNER JOIN Professors
            ON Professors.ProfessorID = CourseOfferings.ProfessorID
        WHERE AcadPeriodName = @AcadPeriodName
            AND
              Professors.ProfessorID = 139
    GO


    EXEC ErdeiSemester @AcadPeriodName = 'Fall 2019'
