# Notes

## ASP.NET

The 'ViewModel' is main way for data exchange between 'Controller' and 'View'. For the simple cases the 'Model' and 'ViewModel' are the same. 
The 'ViewData' member is additional one-direction way to deliver data into 'View' and it should be used for generated HTML-chunks. 

Oh. Could it be that for sending data into view and for reciving data from view the different 'ViewModel'-s should be used?
My answer is yes. The recieveing data can be represented in two ways: as bound object and as method argumnets. The second way is prefferable.

Uhu. Without 'mediator' between DbContext and Controller we will use a lot of duplicated code in the controllers.

## Build from scratch

### Install EF Core tools

Install the CLI tool:

    dotnet tool install --global dotnet-ef

Fix-up the enviroment variables (Linux):

    export PATH=$PATH:$HOME/.dotnet/tools
    export DOTNET_ROOT=$HOME/.dotnet

### Generate ASP.NET MVC project

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

## Department debits and course credits

I do not understand how it works and what is deal.

[Source #1](https://sr.ithaka.org/publications/university-budget-models-and-indirect-costs/)

[Source #2](https://ctlr.msu.edu/COStudentAccounts/TuitionCalculatorFall20.aspx)

What was found in the Michigan State Univercity site: 

> The sample budgets below represent average costs for the 2020-2021 academic year, or what is often referred to as the cost of attendance. These budgets are based on 15 credits/semester for undergraduate students and 9 credits/semester for graduate students.

A small code piece from real university site to calculate: [univeristy-budget-credits-calc-example.js](univeristy-budget-credits-calc-example.js)



