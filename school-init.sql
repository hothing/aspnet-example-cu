PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);
INSERT INTO __EFMigrationsHistory VALUES('20210705134605_Full','5.0.7');
CREATE TABLE IF NOT EXISTS "Person" (
    "ID" INTEGER NOT NULL CONSTRAINT "PK_Person" PRIMARY KEY AUTOINCREMENT,
    "LastName" TEXT NOT NULL,
    "FirstName" TEXT NOT NULL
);
INSERT INTO Person VALUES(1,'Alexander','Carson');
INSERT INTO Person VALUES(2,'Alonso','Meredith');
INSERT INTO Person VALUES(3,'Anand','Arturo');
INSERT INTO Person VALUES(4,'Barzdukas','Gytis');
INSERT INTO Person VALUES(5,'Li','Yan');
INSERT INTO Person VALUES(6,'Justice','Peggy');
INSERT INTO Person VALUES(7,'Norman','Laura');
INSERT INTO Person VALUES(8,'Olivetto','Nino');
INSERT INTO Person VALUES(9,'Goodwin','Erich');
INSERT INTO Person VALUES(10,'Perkinson','Eis');
INSERT INTO Person VALUES(11,'Dow','John');
CREATE TABLE IF NOT EXISTS "Instructor" (
    "ID" INTEGER NOT NULL CONSTRAINT "PK_Instructor" PRIMARY KEY AUTOINCREMENT,
    "HireDate" TEXT NOT NULL,
    CONSTRAINT "FK_Instructor_Person_ID" FOREIGN KEY ("ID") REFERENCES "Person" ("ID") ON DELETE CASCADE
);
INSERT INTO Instructor VALUES(9,'2021-07-05 00:00:00');
INSERT INTO Instructor VALUES(10,'2021-07-05 00:00:00');
INSERT INTO Instructor VALUES(11,'2021-07-05 00:00:00');
CREATE TABLE IF NOT EXISTS "Student" (
    "ID" INTEGER NOT NULL CONSTRAINT "PK_Student" PRIMARY KEY AUTOINCREMENT,
    "EnrollmentDate" TEXT NOT NULL,
    CONSTRAINT "FK_Student_Person_ID" FOREIGN KEY ("ID") REFERENCES "Person" ("ID") ON DELETE CASCADE
);
INSERT INTO Student VALUES(1,'2005-09-01 00:00:00');
INSERT INTO Student VALUES(2,'2002-09-01 00:00:00');
INSERT INTO Student VALUES(3,'2003-09-01 00:00:00');
INSERT INTO Student VALUES(4,'2002-09-01 00:00:00');
INSERT INTO Student VALUES(5,'2002-09-01 00:00:00');
INSERT INTO Student VALUES(6,'2001-09-01 00:00:00');
INSERT INTO Student VALUES(7,'2003-09-01 00:00:00');
INSERT INTO Student VALUES(8,'2005-09-01 00:00:00');
CREATE TABLE IF NOT EXISTS "Department" (
    "DepartmentID" INTEGER NOT NULL CONSTRAINT "PK_Department" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NULL,
    "Budget" money NOT NULL,
    "StartDate" TEXT NOT NULL,
    "InstructorID" INTEGER NULL,
    "RowVersion" BLOB NULL,
    CONSTRAINT "FK_Department_Instructor_InstructorID" FOREIGN KEY ("InstructorID") REFERENCES "Instructor" ("ID") ON DELETE RESTRICT
);
INSERT INTO Department VALUES(1,'AT2001',100,'2021-08-01 00:00:00',9,NULL);
INSERT INTO Department VALUES(2,'MT2100',100,'2021-08-01 00:00:00',10,NULL);
INSERT INTO Department VALUES(3,'BC3303',200,'2021-08-01 00:00:00',11,NULL);
CREATE TABLE IF NOT EXISTS "OfficeAssignment" (
    "InstructorID" INTEGER NOT NULL CONSTRAINT "PK_OfficeAssignment" PRIMARY KEY,
    "Location" TEXT NULL,
    CONSTRAINT "FK_OfficeAssignment_Instructor_InstructorID" FOREIGN KEY ("InstructorID") REFERENCES "Instructor" ("ID") ON DELETE CASCADE
);
INSERT INTO OfficeAssignment VALUES(9,'201');
CREATE TABLE IF NOT EXISTS "Course" (
    "CourseID" INTEGER NOT NULL CONSTRAINT "PK_Course" PRIMARY KEY,
    "Title" TEXT NULL,
    "Credits" INTEGER NOT NULL,
    "DepartmentID" INTEGER NOT NULL,
    CONSTRAINT "FK_Course_Department_DepartmentID" FOREIGN KEY ("DepartmentID") REFERENCES "Department" ("DepartmentID") ON DELETE CASCADE
);
INSERT INTO Course VALUES(5,'AT.TAU',5,1);
CREATE TABLE IF NOT EXISTS "CourseAssignment" (
    "InstructorID" INTEGER NOT NULL,
    "CourseID" INTEGER NOT NULL,
    CONSTRAINT "PK_CourseAssignment" PRIMARY KEY ("CourseID", "InstructorID"),
    CONSTRAINT "FK_CourseAssignment_Course_CourseID" FOREIGN KEY ("CourseID") REFERENCES "Course" ("CourseID") ON DELETE CASCADE,
    CONSTRAINT "FK_CourseAssignment_Instructor_InstructorID" FOREIGN KEY ("InstructorID") REFERENCES "Instructor" ("ID") ON DELETE CASCADE
);
INSERT INTO CourseAssignment VALUES(9,5);
CREATE TABLE IF NOT EXISTS "Enrollment" (
    "EnrollmentID" INTEGER NOT NULL CONSTRAINT "PK_Enrollment" PRIMARY KEY AUTOINCREMENT,
    "CourseID" INTEGER NOT NULL,
    "StudentID" INTEGER NOT NULL,
    "Grade" INTEGER NULL,
    CONSTRAINT "FK_Enrollment_Course_CourseID" FOREIGN KEY ("CourseID") REFERENCES "Course" ("CourseID") ON DELETE CASCADE,
    CONSTRAINT "FK_Enrollment_Student_StudentID" FOREIGN KEY ("StudentID") REFERENCES "Student" ("ID") ON DELETE CASCADE
);
DELETE FROM sqlite_sequence;
INSERT INTO sqlite_sequence VALUES('Person',11);
INSERT INTO sqlite_sequence VALUES('Student',8);
INSERT INTO sqlite_sequence VALUES('Instructor',11);
INSERT INTO sqlite_sequence VALUES('Department',3);
CREATE INDEX "IX_Course_DepartmentID" ON "Course" ("DepartmentID");
CREATE INDEX "IX_CourseAssignment_InstructorID" ON "CourseAssignment" ("InstructorID");
CREATE INDEX "IX_Department_InstructorID" ON "Department" ("InstructorID");
CREATE INDEX "IX_Enrollment_CourseID" ON "Enrollment" ("CourseID");
CREATE INDEX "IX_Enrollment_StudentID" ON "Enrollment" ("StudentID");
COMMIT;
