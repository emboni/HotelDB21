﻿CREATE TABLE po22_Hotel(
     Hotel_No  int  NOT NULL PRIMARY KEY,
     Name      VARCHAR(30)     NOT NULL,
     Address   VARCHAR(50)  NOT NULL
);

CREATE TABLE po22_Room(
	 Room_No   int    NOT NULL,
     Hotel_No  int    NOT NULL,
     Types     CHAR(1)   DEFAULT 'S',
     Price     FLOAT,
	CONSTRAINT checkType 
	CHECK (Types IN ('D','F','S') OR Types IS NULL),
	CONSTRAINT checkPrice 
	CHECK (price BETWEEN 0 AND 9999),

	FOREIGN KEY (Hotel_No) REFERENCES po22_Hotel (Hotel_No)
	ON UPDATE CASCADE ON DELETE NO ACTION,
	Primary KEY (Room_No, Hotel_No)
);

CREATE TABLE po22_Guest (
     Guest_No  int  NOT NULL PRIMARY KEY,
     Name      VARCHAR(30)      NOT NULL,
     Address   VARCHAR(50)   NOT NULL
);

CREATE TABLE po22_Booking(
     Booking_id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	 Hotel_No  int   NOT NULL,
     Guest_No  int   NOT NULL,
     Date_From DATE  NOT NULL,
     Date_To   DATE  NOT NULL,
     Room_No   int   NOT NULL,
     FOREIGN KEY(Guest_No) REFERENCES po22_Guest(Guest_No),
	 FOREIGN KEY(Room_No, Hotel_No) REFERENCES po22_Room(Room_No, Hotel_No)
);


		
ALTER TABLE po22_Booking 
	ADD CONSTRAINT incorrect_dates
       CHECK ((Date_To > Date_From) AND (Date_From <= '2014-01-01'));
GO
