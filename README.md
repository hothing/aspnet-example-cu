# ASP.NET MVC with EF Core

Read an article [Tutorial: Get started with EF Core in an ASP.NET MVC web app](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-5.0)

And test the working example http://wagner.wang/ContosoUniversity
## Prerequisites

* Debian/Ubuntu Linux
* [Install .NET Core SDK (NET5)](https://docs.microsoft.com/en-us/dotnet/core/install/linux)
* SQLite 3.x
        sudo apt install sqlite3

* [Visual Studio Code](https://code.visualstudio.com/download) 

## Build from scratch

### Install EF Core tools

Install the CLI tool:

    dotnet tool install --global dotnet-ef

Fix-up the enviroment variables (Linux):

    export PATH=$PATH:$HOME/.dotnet/tools
    export DOTNET_ROOT=$HOME/.dotnet

### Generate ASP>NET MVC project

    dotnet new mvc -o cu-pum

### Add the packages

    dotnet add package Microsoft.EntityFrameworkCore.Sqlite
    dotnet add package Microsoft.EntityFrameworkCore.Relational
    dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design

### Create the models

    $ mkdir Models
    $ nano Models/Person.cs
    $ nano Models/Student.cs
    $ nano Models/Instructor.cs
    $ nano Models/Course.cs
    $ nano Models/Department.cs
    $ nano Models/Enrollment.cs
    $ nano Models/OfficeAssigment.cs
    $ nano Models/CourseAssigment.cs    

### Generate the controllers and the views

    dotnet-aspnet-codegenerator controller -name StudentController -dc SchoolContext -m Student -sqlite -l "_Layout" -outDir Controllers
    dotnet-aspnet-codegenerator controller -name CourseController -dc SchoolContext -m Course -sqlite -l "_Layout" -outDir Controllers
    dotnet-aspnet-codegenerator controller -name EnrollmentController -dc SchoolContext -m Enrollment -l "_Layout" -sqlite -outDir Controllers
    dotnet-aspnet-codegenerator controller -name DepartmentController -dc SchoolContext -m Department -l "_Layout" -sqlite -outDir Controllers
    dotnet-aspnet-codegenerator controller -name InstructorController -dc SchoolContext -m Instructor -l "_Layout" -sqlite -outDir Controllers

    dotnet-aspnet-codegenerator controller -name OfficeAssignmentController -dc SchoolContext -m OfficeAssignment -sqlite -l "_Layout" -outDir Controllers
    dotnet-aspnet-codegenerator controller -name CourseAssignmentController -dc SchoolContext -m CourseAssignment -sqlite -l "_Layout" -outDir Controllers
    dotnet-aspnet-codegenerator controller -name PersonController -dc SchoolContext -m Person -sqlite -l "_Layout" -outDir Controllers

### Edit the controllers and views

Default models are not good enough for using. They should be improved.