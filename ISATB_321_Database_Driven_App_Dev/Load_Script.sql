/*
    Author: Quick, Emrys
*/









SET IDENTITY_INSERT Advisors ON
GO

    INSERT INTO [Advisors] (AdvisorID, AdvisorFName, AdvisorLName, AdvisorEmail)
        VALUES
        (1,N'Candice',N'Sutton',N'cblawat@uscb.edu'),
        (2,N'Chelsea',N'Ponce',N'cponce@uscb.edu'),
        (3,N'Rosie',N'Vailloncourt',N'rosie.vailloncourt@uscb.edu'),
        (4,N'Ronald',N'Erdei',N'erdei@uscb.edu'),
        (5,N'Brian',N'Canada',N'bcanada@uscb.edu'),
        (6,N'Paul',N'Comitz',N'pcomitz@uscb.edu'),
        (7,N'John',N'Thrasher',N'thrashw@uscb.edu'),
        (8,N'Xuwei',N'Liang',N'xliang@uscb.edu'),
        (9,N'Xiaomei',N'Zhang',N'xiaomei@uscb.edu'),
        (10,N'Manuel',N'Sanders',N'mjsander@uscb.edu'),
        (11,N'Davide',N'Fusi',N'dfusi@uscb.edu')
    GO

SET IDENTITY_INSERT Advisors OFF
GO





SET IDENTITY_INSERT Students ON
GO

    INSERT INTO [Students] (StudentID, StudentFName, StudentLName, Year)
        VALUES
        (1,N'Alex',N'Robertson',0),
        (2,N'Corey',N'Gray',0),
        (3,N'David',N'Andrews',0),
        (4,N'Rory',N'Anderson',0),
        (5,N'Josh',N'Brooks',0),
        (6,N'Crew',N'Andrews',0),
        (7,N'Major',N'Boyer',0),
        (8,N'Travis',N'Hendrix',0),
        (9,N'Louis',N'Pearson',0),
        (10,N'Eliseo',N'Salazar',0),
        (11,N'Rowan',N'Lane',0),
        (12,N'Lesley',N'Mcdonald',0),
        (13,N'Willy',N'Davies',0),
        (14,N'Leslie',N'Jenkins',0),
        (15,N'Bailey',N'Adams',0),
        (16,N'Danni',N'Mcmillan',1),
        (17,N'Skylar',N'Small',1),
        (18,N'Lynn',N'Chang',1),
        (19,N'Val',N'Stout',1),
        (20,N'Skyler',N'Merritt',1),
        (21,N'Natasha',N'Kaur',1),
        (22,N'Evie',N'Lawson',1),
        (23,N'Lilly',N'Sutton',1),
        (24,N'Victoria',N'Holland',1),
        (25,N'Molly',N'Anderson',1),
        (26,N'Paula',N'Pierce',1),
        (27,N'Tamia',N'Good',1),
        (28,N'Kaelyn',N'Spears',1),
        (29,N'Emory',N'Emerson',1),
        (30,N'Lana',N'Davenport',1),
        (31,N'James',N'Rose',2),
        (32,N'Cody',N'Miller',2),
        (33,N'Harley',N'Andrews',2),
        (34,N'Jenson',N'Stone',2),
        (35,N'Rhys',N'Matthews',2),
        (36,N'Micah',N'Stephens',2),
        (37,N'Marcus',N'Sawyer',2),
        (38,N'Carmelo',N'Lara',2),
        (39,N'Roderick',N'Holloway',2),
        (40,N'Leo',N'Berg',2),
        (41,N'Tyler',N'Kennedy',2),
        (42,N'Tyler',N'Hudson',2),
        (43,N'Jordan',N'Turner',2),
        (44,N'Quinn',N'Gordon',2),
        (45,N'Lynn',N'Jackson',2),
        (46,N'Ashley',N'Yates',3),
        (47,N'Fran',N'Marks',3),
        (48,N'Raylee',N'York',3),
        (49,N'Jaime',N'Harding',3),
        (50,N'Casey',N'Carlson',3),
        (51,N'Martha',N'Thomas',3),
        (52,N'Rachel',N'Hart',3),
        (53,N'Rosie',N'Jenkins',3),
        (54,N'Scarlett',N'Thomas',3),
        (55,N'Amelia',N'Saunders',3),
        (56,N'Kate',N'Haynes',3),
        (57,N'Rose',N'Walter',3),
        (58,N'Emmaline',N'Cross',3),
        (59,N'Deanna',N'Mcconnell',3),
        (60,N'Sawyer',N'Kinney',3),
        (61,N'Callum',N'Williams',4),
        (62,N'Aiden',N'Wells',4),
        (63,N'Riley',N'Allen',4),
        (64,N'Robert',N'Wright',4),
        (65,N'Kian',N'Hall',4),
        (66,N'Beau',N'Carney',4),
        (67,N'Hector',N'Benson',4),
        (68,N'Jayden',N'Valentine',4),
        (69,N'Fletcher',N'Gregory',4),
        (70,N'Korbin',N'Ross',4),
        (71,N'Alex',N'Jenkins',4),
        (72,N'Mia',N'Griffiths',4),
        (73,N'Alex',N'Booth',4),
        (74,N'Poppy',N'Russell',4),
        (75,N'Faith',N'Green',4);

SET IDENTITY_INSERT Students OFF
GO





SET IDENTITY_INSERT Locations ON
GO

INSERT INTO [Locations] (LocationID, Description)
VALUES 
    (1, N'LIBR 241'),
    (2, N'SCITECH 145'),
    (3, N'201 Grayson House Room 0'),
    (4, N'HARG 162A'),
    (5, N'SCITECH 231'),
    (6, N'LIBR Academic Advising Office'),
    (7, N'SCITECH 144'),
    (8, N'SCITECH 143'),
    (9, N'HARG 222'),
    (10, N'110 Boundary St Room 104')
GO

SET IDENTITY_INSERT Locations OFF
GO



SET IDENTITY_INSERT Times ON
GO

    INSERT INTO [Times] (TimeID, StartTime, EndTime)
        VALUES
        (1,N'8:00AM',N'8:15AM'),
        (2,N'8:15AM',N'8:30AM'),
        (3,N'8:30AM',N'8:45AM'),
        (4,N'8:45AM',N'9:00AM' ),
        (5,N'9:00AM',N'9:15AM' ),
        (6,N'9:15AM',N'9:30AM' ),
        (7,N'9:30AM',N'9:45AM' ),
        (8,N'9:45AM',N'10:00AM' ),
        (9,N'10:00AM',N'10:15AM' ),
        (10,N'10:15AM',N'10:30AM' ),
        (11,N'10:30AM',N'10:45AM' ),
        (12,N'10:45AM',N'11:00AM' ),
        (13,N'11:00AM',N'11:15AM' ),
        (14,N'11:15AM',N'11:30AM' ),
        (15,N'11:30AM',N'11:45AM' ),
        (16,N'11:45AM',N'12:00PM' ),
        (17,N'12:00PM',N'12:15PM' ),
        (18,N'12:15PM',N'12:30PM' ),
        (19,N'12:30PM',N'12:45PM' ),
        (20,N'12:45PM',N'1:00PM' ),
        (21,N'1:00PM',N'1:15PM' ),
        (22,N'1:15PM',N'1:30PM' ),
        (23,N'1:30PM',N'1:45PM' ),
        (24,N'1:45PM',N'2:00PM' ),
        (25,N'2:00PM',N'2:15PM' ),
        (26,N'2:15PM',N'2:30PM' ),
        (27,N'2:30PM',N'2:45PM' ),
        (28,N'2:45PM',N'3:00PM' ),
        (29,N'3:00PM',N'3:15PM' ),
        (30,N'3:15PM',N'3:30PM' ),
        (31,N'3:30PM',N'3:45PM' ),
        (32,N'3:45PM',N'4:00PM' ),
        (33,N'4:00PM',N'4:15PM' ),
        (34,N'4:15PM',N'4:30PM' ),
        (35,N'4:30PM',N'4:45PM' ),
        (36,N'4:45PM',N'5:00PM' ),
        (37,N'5:00PM',N'5:15PM' ),
        (38,N'5:15PM',N'5:30PM' ),
        (39,N'5:30PM',N'5:45PM' ),
        (40,N'5:45PM',N'6:00PM' ),
        (41,N'6:00PM',N'6:15PM' ),
        (42,N'6:15PM',N'6:30PM' ),
        (43,N'6:30PM',N'6:45PM' ),
        (44,N'6:45PM',N'7:00PM' )
    GO

SET IDENTITY_INSERT Times OFF



SET IDENTITY_INSERT Availability ON
GO

INSERT INTO [Availability] (AvailabilityID, AdvisorID, Date, TimeID, LocationID, IsTaken)
VALUES
    (1, 1, '2025-05-01', 1, 1, 0),
    (2, 2, '2025-05-02', 10, 2, 0),
    (3, 3, '2025-05-03', 27, 3, 1)
GO

SET IDENTITY_INSERT Availability OFF


SET IDENTITY_INSERT Meetings ON
GO

INSERT INTO [Meetings] (MeetingID, StudentID, AvailabilityID)
VALUES
    (1, 3, 1),
    (2, 2, 3),
    (3, 3, 2)
GO

SET IDENTITY_INSERT Meetings OFF


/*
(1, 1, '2025-05-01 08:00:00', 1, 1, 0),
    (2, 2, '2025-05-02 10:15:00', 10, 2, 0),
    (3, 3, '2025-05-03 02:30:00', 27, 3, 1)
	*/


--test meetings
/*
SET IDENTITY_INSERT Meetings ON
GO

INSERT INTO [Meetings] (MeetingID, StudentID, AvailabilityID)
VALUES
    (1, 3, 5),
    (2, 2, 4),
    (3, 3, 2)
GO

SET IDENTITY_INSERT Meetings OFF
*/