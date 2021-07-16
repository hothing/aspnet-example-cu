# Notes

## ASP.NET

The 'ViewModel' is main way for data exchange between 'Controller' and 'View'. For the simple cases the 'Model' and 'ViewModel' are the same. 
The 'ViewData' member is additional one-direction way to deliver data into 'View' and it should be used for generated HTML-chunks. 

Oh. Could it be that for sending data into view and for reciving data from view the different 'ViewModel'-s should be used?
My answer is yes. The recieveing data can be represented in two ways: as bound object and as method argumnets. The second way is prefferable.

## Department debits and course credits

I do not understand how it works and what is deal.

[Source #1](https://sr.ithaka.org/publications/university-budget-models-and-indirect-costs/)

[Source #2](https://ctlr.msu.edu/COStudentAccounts/TuitionCalculatorFall20.aspx)

What was found in the Michigan State Univercity site: 

> The sample budgets below represent average costs for the 2020-2021 academic year, or what is often referred to as the cost of attendance. These budgets are based on 15 credits/semester for undergraduate students and 9 credits/semester for graduate students.

A small code piece from real university site to calculate: [univeristy-budget-credits-calc-example.js](univeristy-budget-credits-calc-example.js)



