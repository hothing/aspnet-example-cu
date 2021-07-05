# ASP>NET MVC with EF Core

## Prerequisites

## Build from scratch

### Generate ASP>NET MVC project

    dotnet new mvc -o cu-pum

### Add the packages

    dotnet add package Microsoft.EntityFrameworkCore.Sqlite
    dotnet add package Microsoft.EntityFrameworkCore.Relational
    dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design

### Create the models

### Generate the controllers and the views

    dotnet-aspnet-codegenerator controller -name StudentController -dc SchoolContext -m Student -sqlite -l "_Layout" -outDir Controllers
    dotnet-aspnet-codegenerator controller -name CourseController -dc SchoolContext -m Course -sqlite -l "_Layout" -outDir Controllers
    dotnet-aspnet-codegenerator controller -name EnrollmentController -dc SchoolContext -m Enrollment -l "_Layout" -sqlite -outDir Controllers
    dotnet-aspnet-codegenerator controller -name DepartmentController -dc SchoolContext -m Department -l "_Layout" -sqlite -outDir Controllers
    dotnet-aspnet-codegenerator controller -name InstructorController -dc SchoolContext -m Instructor -l "_Layout" -sqlite -outDir Controllers

    dotnet-aspnet-codegenerator controller -name OfficeAssignmentController -dc SchoolContext -m OfficeAssignment -sqlite -l "_Layout" -outDir Controllers
    dotnet-aspnet-codegenerator controller -name CourseAssignmentController -dc SchoolContext -m CourseAssignment -sqlite -l "_Layout" -outDir Controllers
    dotnet-aspnet-codegenerator controller -name PersonController -dc SchoolContext -m Person -sqlite -l "_Layout" -outDir Controllers